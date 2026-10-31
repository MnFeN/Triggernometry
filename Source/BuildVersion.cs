using System.Reflection;

[assembly: AssemblyVersion(BuildVersion.Value)]
[assembly: AssemblyFileVersion(BuildVersion.Value)]

/// <summary>主插件和代理共用的发布版本号。</summary>
internal static class BuildVersion
{
    internal const string Value = "2.4.1.1"; 
}
