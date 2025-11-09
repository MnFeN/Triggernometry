using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Triggernometry.Utilities
{
    public class UploadTextHelper
    {
        /// <summary>
        /// 向阿里云函数计算上传文本的回调方法。<br />
        /// </summary>
        /// <returns>上传结果；失败则返回错误消息</returns>
        public static string UploadText(string body)
        {
            const string url = "https://uploadtext-wxmxyorltv.cn-hangzhou.fcapp.run/";
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(body);
                var req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "POST";
                req.ContentType = "text/plain; charset=utf-8";
                req.ContentLength = data.Length;

                using (var stream = req.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                using (var resp = (HttpWebResponse)req.GetResponse())
                using (var reader = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                // 如果服务器返回了错误内容，尝试读取并附加到异常中
                try
                {
                    using (var resp = (HttpWebResponse)ex.Response)
                    using (var reader = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
                    {
                        string serverErr = reader.ReadToEnd();
                        throw new WebException($"服务器返回错误: {serverErr}", ex);
                    }
                }
                catch
                {
                    throw; // 原样抛出
                }
            }
        }

        public static void UploadTextCallback(object _, string body)
        {
            _ = UploadText(body);
        }
    }
}