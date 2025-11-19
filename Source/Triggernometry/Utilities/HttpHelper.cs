using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Triggernometry.Utilities
{
    public static class HttpHelper
    {
        private static readonly HttpClient client = new HttpClient()
        {
            Timeout = TimeSpan.FromSeconds(10)
        };

        private const string USER_AGENT = "Triggernometry";

        // GET string
        public static async Task<string> GetStringAsync(string url, CancellationToken? ct = null)
        {
            using (var req = new HttpRequestMessage(HttpMethod.Get, url))
            {
                req.Headers.UserAgent.ParseAdd(USER_AGENT);

                var resp = await client.SendAsync(req, ct ?? CancellationToken.None);

                resp.EnsureSuccessStatusCode();
                return await resp.Content.ReadAsStringAsync();
            }
        }

        // GET bytes
        public static async Task<byte[]> GetBytesAsync(string url, CancellationToken? ct = null)
        {
            using (var req = new HttpRequestMessage(HttpMethod.Get, url))
            {
                req.Headers.UserAgent.ParseAdd(USER_AGENT);

                var resp = await client.SendAsync(req, ct ?? CancellationToken.None);

                resp.EnsureSuccessStatusCode();
                return await resp.Content.ReadAsByteArrayAsync();
            }
        }

        /// <summary>
        /// Downloads a file and optionally backs up the old version.
        /// If backupPath is null, no backup is created.
        /// </summary>
        public static async Task DownloadAndReplaceAsync(string url, string filePath, string backupPath = null)
        {
            string tempPath = filePath + ".tmp";

            // download tmp file
            byte[] data = await client.GetByteArrayAsync(url);
            File.WriteAllBytes(tempPath, data);

            // backup old file
            if (backupPath != null && File.Exists(filePath))
            {
                if (File.Exists(backupPath))
                    File.Delete(backupPath);
                File.Move(filePath, backupPath);
            }
            else if (backupPath == null && File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            File.Move(tempPath, filePath);
        }

        /// <summary>
        /// Retrieves the remote file's metadata using HEAD (preferred)
        /// and falls back to a Range 0–0 request if required.
        /// <br/>Returns the full file length and last-modified timestamp when available.
        /// </summary>
        public static async Task<(long? contentLength, DateTime? lastModified)> GetMetadataAsync(string url, CancellationToken? ct = null)
        {
            ct = ct ?? CancellationToken.None;

            long? contentLength = null;
            DateTime? lastModified = null;

            // Try HEAD
            try
            {
                using (var req = new HttpRequestMessage(HttpMethod.Head, url))
                {
                    req.Headers.UserAgent.ParseAdd(USER_AGENT);
                    var resp = await client.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct.Value);

                    if (resp.IsSuccessStatusCode)
                    {
                        if (resp.Content.Headers.ContentLength.HasValue)
                            contentLength = resp.Content.Headers.ContentLength.Value;

                        if (resp.Content.Headers.LastModified.HasValue)  // github does not have it
                            lastModified = resp.Content.Headers.LastModified.Value.UtcDateTime;
                    }
                }
            }
            catch { }

            // Metadata from HEAD is not complete: Try Range 0-0
            if (contentLength == null || lastModified == null)
            {
                try
                {
                    using (var req = new HttpRequestMessage(HttpMethod.Get, url))
                    {
                        req.Headers.UserAgent.ParseAdd(USER_AGENT);
                        req.Headers.Range = new System.Net.Http.Headers.RangeHeaderValue(0, 0);

                        var resp = await client.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct.Value);

                        if (resp.IsSuccessStatusCode || resp.StatusCode == System.Net.HttpStatusCode.PartialContent)
                        {
                            // Content-Range: bytes 0-0/289876
                            if (resp.Content.Headers.ContentRange != null)
                            {
                                if (resp.Content.Headers.ContentRange.Length.HasValue)
                                    contentLength = resp.Content.Headers.ContentRange.Length.Value;
                            }

                            if (resp.Content.Headers.LastModified.HasValue)  // github does not have it
                                lastModified = resp.Content.Headers.LastModified.Value.UtcDateTime;
                        }
                    }
                }
                catch { }
            }

            return (contentLength, lastModified);
        }


    }
}