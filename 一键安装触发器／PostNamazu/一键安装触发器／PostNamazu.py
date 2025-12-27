import os
import ctypes
import re
import requests
from typing import Optional, List
import webbrowser
import pyperclip

def is_admin() -> bool:
    try:
        return ctypes.windll.shell32.IsUserAnAdmin()
    except:
        return False

class Plugin:

    def __init__(self, enabled: bool | str, full_path: str) -> None:
        if isinstance(enabled, str):
            self.enabled = 'True' if enabled.strip().lower() == 'true' else 'False'
        else:
            self.enabled = 'True' if enabled else 'False'
        self.full_path = full_path
        self.folder_path, self.name = os.path.split(self.full_path)

    @staticmethod
    def parse(rawXml: str) -> 'Plugin':
        enabled_match = re.search(r'Enabled="(.*?)"', rawXml)
        enabled = enabled_match.group(1) if enabled_match else 'False'

        path_match = re.search(r'Path="(.*?)"', rawXml)
        full_path = path_match.group(1) if path_match else ''

        return Plugin(enabled, full_path)

    def to_xml_string(self) -> str:
        return f"\n        <Plugin Enabled=\"{self.enabled}\" Path=\"{self.full_path}\" />"
    
    # 从远程地址更新并覆盖本地插件。若提供 file_name，则更新并覆盖本地插件同文件夹下的指定名称文件（如触发器汉化）。
    def update(self, remote_path: str, file_name: Optional[str] = None) -> None:
        path = self.full_path if file_name is None else os.path.join(self.folder_path, file_name)
        file_name = file_name if file_name else self.name
        print(f"\n正在从 {remote_path} 下载 {file_name} ...")

        try:
            # 设置 15 秒超时，防止死等
            response = requests.get(remote_path, timeout=15)
            response.raise_for_status()
            with open(path, 'wb') as f:
                f.write(response.content)
            print(f"已成功安装或更新：{file_name}")
            
        except requests.exceptions.ConnectionError:
            print(f"[错误] 下载 {file_name} 失败：网络连接无法建立。")
            print("   >>> 建议：请检查你的网络设置，或尝试开启/切换网络加速器，并确认此安装程序为网盘最新版本。")
        except requests.exceptions.Timeout:
            print(f"[错误] 下载 {file_name} 失败：连接超时。")
            print("   >>> 建议：请检查你的网络设置，或尝试开启/切换网络加速器，并确认此安装程序为网盘最新版本。")
        except requests.exceptions.HTTPError as e:
            print(f"[错误] 下载 {file_name} 失败：服务器返回错误 ({e.response.status_code})。")
            print("   >>> 建议：请检查你的网络设置，或尝试开启/切换网络加速器，并确认此安装程序为网盘最新版本。")
        except IOError as e:
            print(f"[错误] 数据写入 {file_name} 失败：\n{e}\n")
        except Exception as e:
            print(f"[错误] 处理 {file_name} 时发生未知错误：\n{e}")
    

    def delete(self) -> None:
        try:
            os.remove(self.full_path)
        except:
            pass


def get_plugin_from_list(plugin_name: str, plugin_list: List[Plugin]) -> Optional[Plugin]:
    for plugin in plugin_list:
        if plugin_name.lower() in plugin.name.lower():
            return plugin
    return None


REMOTE_FOLDER = "https://1824544011.v.123pan.cn/1824544011/Triggernometry_Release_CN/"
REMOTE_RESOURCE = "https://www.123865.com/s/1xRXjv-n9qBH"
def set_up_config(config_path: str, plugin_path: str):

    try:
        with open(config_path, 'r', encoding='utf-8') as file:
            xml_data = file.read()
    except FileNotFoundError:
        raise FileNotFoundError(f"错误：配置文件{config_path}不存在。你可能选择了错误的 ACT 版本，或程序未置于根目录。")

    matches = list(re.finditer(r'<ActPlugins>(.*?)</ActPlugins>', xml_data, re.DOTALL))

    if not matches:
        print("配置文件中未找到 <ActPlugins>。")
        return
    
    match = matches[-1] # 呆萌整合版开头有另外一处 <ActPlugins>
    act_plugins_content = match.group(1)
    raw_plugins = re.findall(r'<Plugin .*?/>', act_plugins_content)
    act_plugins = [Plugin.parse(plugin_xml) for plugin_xml in raw_plugins]

    # requires
    xiv_parser = get_plugin_from_list("FFXIV_ACT_Plugin", act_plugins)
    if xiv_parser is None:
        raise Exception("你尚未安装依赖项 FFXIV_ACT_Plugin 解析插件。")
    overlay = get_plugin_from_list("Overlay", act_plugins)
    if overlay is None:
        raise Exception("你尚未安装依赖项 ngld/OverlayPlugin 悬浮窗/解析插件。")

    # 检查 Overlay 是否在解析插件后面
    if act_plugins.index(overlay) < act_plugins.index(xiv_parser):
        # 如果 Overlay 在前，解析插件在后，手动调换它们
        act_plugins.remove(overlay)
        xiv_idx = act_plugins.index(xiv_parser)
        act_plugins.insert(xiv_idx + 1, overlay)
        print("检测到插件顺序异常，已自动将 OverlayPlugin 调整至 FFXIV 解析插件之后。")

    # mlm
    mlm_trig = get_plugin_from_list("MlmTr", act_plugins)
    if mlm_trig is None:
        mlm_trig = get_plugin_from_list("莫灵喵", act_plugins)
    if mlm_trig is not None:
        mlm_trig.delete()
        act_plugins.remove(mlm_trig)
        old_trig_config_path = os.path.join(os.path.dirname(config_path), mlm_trig.name[:-4] + "config.xml")
        pyperclip.copy(old_trig_config_path)
        print(f"检测到莫灵喵版旧触发器，此版本的配置无法自动还原。\n"
              f"但你可以稍后在触发器插件页面点击导入，选择旧配置文件 {old_trig_config_path} 导入以恢复全部本地触发器。\n"
              f"此路径已存入剪贴板。")

    # Triggernometry
    trig = get_plugin_from_list("Triggernometry", act_plugins)
    if trig is None:
        trig = Plugin('True', os.path.join(plugin_path, "Triggernometry.dll"))
        overlayIdx = act_plugins.index(overlay)
        act_plugins.insert(overlayIdx + 1, trig)

    trig.update(REMOTE_FOLDER + "Triggernometry.dll")
    trig.update(REMOTE_FOLDER + "zh-CN.triglations.xml", "zh-CN.triglations.xml")

    # PostNamazu
    updateNamazu = False
    print("你是否需要安装或更新鲶鱼精邮差（PostNamazu）？\n此插件是绝大多数副本触发器的前置工具，用于发送默语提示、本地标点以及更高级的功能。\n你安装后可以随时手动禁用，也可以跳过此项并随时安装。\n"
            "0. 否\n"
            "1. 是\n")
    while True:
        match input().strip():
            case "0":
                updateNamazu = False
                break
            case "1":
                updateNamazu = True
                break
            case _:
                print("输入必须是 0 / 1。请重新输入。")
    if updateNamazu:
        namazu = get_plugin_from_list("PostNamazu", act_plugins)
        if namazu is None:
            namazu = Plugin('True', os.path.join(plugin_path, "PostNamazu.dll"))
            act_plugins.append(namazu)
        namazu.update(REMOTE_FOLDER + "PostNamazu.dll")

    modified_content = ''.join([p.to_xml_string() for p in act_plugins]) + "\n    "
    new_xml_data = xml_data.replace(act_plugins_content, modified_content)

    # edit config
    with open(config_path, 'w', encoding='utf-8') as file:
        file.write(new_xml_data)
    with open(config_path + '_', 'w', encoding='utf-8') as file:
        file.write(new_xml_data)
    print("\n已完成初始化。")

def main():
    print("自动安装触发器/鲶鱼精邮差 by 阿洛\n"
          "源码：https://github.com/MnFeN/Triggernometry/tree/readme/一键安装触发器／PostNamazu\n"
          "---------------------------------\n"
          "如果安装遇到问题，或链接打不开，请检查此工具是否为网盘最新版本。\n"
          "如果 ACT 使用上遇到问题，请按照远程触发器自检或进群 612703030 反馈，反复用此程序安装不会解决问题。\n"
          "---------------------------------")
    if not is_admin():
        input("请右键使用管理员模式打开程序，否则无法存储插件文件。")
        return
    while True:
        try:
            act_type = input("\n请选择你使用的 ACT 版本：\n1. 原版\n2. 呆萌整合（或原版 portable 模式）\n3. CafeACT\n").strip()
            match act_type:
                case "1":
                    home = os.getenv('APPDATA')
                    config_path = os.path.join(home, "Advanced Combat Tracker", "Config", "Advanced Combat Tracker.config.xml")
                    plugin_path = os.path.join(home, "Advanced Combat Tracker", "Plugins")
                    input("已选择 1. 原版，请确保：\n\n"
                          "1. 当前没有正在运行的 ACT；\n"
                          "2. 你需要确保已经安装了 FF14 解析插件、ngld/OverlayPlugin 悬浮窗，并重启过 ACT。\n\n"
                          "确认以上内容后回车继续。\n")
                case "2":
                    current_location = os.getcwd()
                    config_path = os.path.join(current_location, "Config", "Advanced Combat Tracker.config.xml")
                    plugin_path = os.path.join(current_location, "Plugins")
                    input("已选择 2. 呆萌整合（或原版 portable 模式），请确保：\n\n"
                          "1. 该程序已置于 ACT 根目录下；\n"
                          "2. 当前没有正在运行的 ACT。\n\n"
                          "确认以上内容后回车继续。\n")
                case "3":
                    current_location = os.getcwd()
                    config_path = os.path.join(current_location, "AppData", "Advanced Combat Tracker", "Config", "CafeACT.config.xml")
                    plugin_path = os.path.join(current_location, "Plugins")
                    input("已选择 3. CafeACT，请确保：\n\n"
                          "1. 该程序已置于 ACT 根目录下；\n"
                          "2. 当前没有正在运行的 ACT；\n"
                          "3. 你需要确保在插件中心安装了 FF14 解析插件、ngld/OverlayPlugin 悬浮窗、Triggernometry，并重启过 ACT。\n\n"
                          "确认以上内容后回车继续。\n")
                case _:
                    print("输入必须是 1 / 2 / 3。请重新输入。")
                    continue
            set_up_config(config_path, plugin_path)
            input("你可以直接退出程序，或按回车键打开资源网盘。\n\n注：7.0 后的副本在远程触发器中自动更新，直接开启 ACT 即可，不需要手动下载。\n网盘中为旧副本或其他类型触发器。")
            webbrowser.open(REMOTE_RESOURCE, new=2)
            break
        except Exception as e:
            input(f"发生错误: {e}")

if __name__ == "__main__":
    main()
