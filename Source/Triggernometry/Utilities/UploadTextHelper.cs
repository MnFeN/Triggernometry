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
        /// <param name="body">要上传的文本</param>
        /// <returns>上传结果字符串</returns>
        /// <exception cref="WebException">网络错误或服务器返回非 200 状态码。</exception>
        /// <exception cref="InvalidOperationException">服务器返回了错误信息。</exception>
        /// <exception cref="Exception">其他异常。</exception>
        public static string UploadText(string body)
        {
            const string url = "https://uploadtext-wxmxyorltv.cn-hangzhou.fcapp.run/";
            byte[] data = Encoding.UTF8.GetBytes(body);
            var req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "text/plain; charset=utf-8";
            req.ContentLength = data.Length;

            using (var stream = req.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            try
            {
                using (var resp = (HttpWebResponse)req.GetResponse())
                using (var reader = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
                {
                    string result = reader.ReadToEnd();

                    // 如果服务器返回的内容本身是错误信息
                    if (resp.StatusCode != HttpStatusCode.OK)
                        throw new WebException($"服务器返回状态码 {(int)resp.StatusCode} ({resp.StatusDescription})");
                    return result;
                }
            }
            catch (WebException ex)
            {
                // 如果服务器确实有响应，读取错误信息附加上去
                try
                {
                    using (var resp = (HttpWebResponse)ex.Response)
                    using (var reader = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
                    {
                        string serverErr = reader.ReadToEnd();
                        throw new WebException($"网络或服务器错误：{serverErr}", ex);
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
            try 
            { 
                _ = UploadText(body); 
            } 
            catch (Exception ex)
            {
                RealPlugin.plug.UnfilteredAddToLog(RealPlugin.DebugLevelEnum.Warning, ex.Message);
            }
        }
    }
}