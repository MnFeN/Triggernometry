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

        /// <summary>
        /// Downloads the resource at the specified URL by <see cref="HttpClient"/> and returns its content as a string.
        /// </summary>
        /// <exception cref="OperationCanceledException"> Thrown when the provided <paramref name="ct"/> was cancelled by the caller. </exception>
        /// <exception cref="TimeoutException"> Thrown when the HTTP request was cancelled due to timeout. </exception>
        /// <exception cref="HttpRequestException"> Thrown when the server returns a non-success status code. </exception>
        public static async Task<string> GetStringAsync(string url, CancellationToken? ct = null)
        {
            var token = ct ?? CancellationToken.None;
            try
            {
                using (var req = new HttpRequestMessage(HttpMethod.Get, url))
                {
                    req.Headers.UserAgent.ParseAdd(USER_AGENT);
                    using (var resp = await client.SendAsync(req, token))
                    {
                        resp.EnsureSuccessStatusCode();
                        return await resp.Content.ReadAsStringAsync();
                    }
                }
            }
            catch (TaskCanceledException ex)
            {
                if (token.IsCancellationRequested)
                    throw new OperationCanceledException(ex.Message, token); // cancelled by caller
                else
                    throw new TimeoutException(ex.Message); // cancelled due to HTTP timeout
            }
        }


        /// <summary>
        /// Downloads the resource at the specified URL by <see cref="HttpClient"/> and returns its raw bytes.
        /// </summary>
        /// <exception cref="OperationCanceledException"> Thrown when the provided <paramref name="ct"/> was cancelled by the caller. </exception>
        /// <exception cref="TimeoutException"> Thrown when the HTTP request was cancelled due to timeout. </exception>
        /// <exception cref="HttpRequestException"> Thrown when the server returns a non-success status code. </exception>
        public static async Task<byte[]> GetBytesAsync(string url, CancellationToken? ct = null)
        {
            var token = ct ?? CancellationToken.None;
            try
            {
                using (var req = new HttpRequestMessage(HttpMethod.Get, url))
                {
                    req.Headers.UserAgent.ParseAdd(USER_AGENT);
                    using (var resp = await client.SendAsync(req, token))
                    {
                        resp.EnsureSuccessStatusCode();
                        return await resp.Content.ReadAsByteArrayAsync();
                    }
                }
            }
            catch (TaskCanceledException ex)
            {
                if (token.IsCancellationRequested)
                    throw new OperationCanceledException(ex.Message, token); // cancelled by caller
                else
                    throw new TimeoutException(ex.Message); // cancelled due to HTTP timeout
            }
        }

        /// <summary>
        /// Downloads a file and optionally backs up the old version.
        /// If backupPath is null, no backup is created.
        /// </summary>
        /// <exception cref="OperationCanceledException"> Thrown when the provided <paramref name="ct"/> was cancelled by the caller. </exception>
        /// <exception cref="TimeoutException"> Thrown when the HTTP request was cancelled due to timeout. </exception>
        /// <exception cref="HttpRequestException"> Thrown when the server returns a non-success status code. </exception>
        public static async Task DownloadAndReplaceAsync(string url, string filePath, string backupPath = null)
        {
            string tempPath = filePath + ".tmp";

            // download tmp file
            byte[] data = await GetBytesAsync(url);
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
        /// <exception cref="OperationCanceledException"> Thrown when the provided <paramref name="ct"/> was cancelled by the caller. </exception>
        /// <exception cref="TimeoutException"> Thrown when the HTTP request was cancelled due to timeout. </exception>
        /// <exception cref="HttpRequestException"> Thrown when the server returns a non-success status code. </exception>
        public static async Task<(long? contentLength, DateTime? lastModified)> GetMetadataAsync(string url, CancellationToken? ct = null)
        {
            var token = ct ?? CancellationToken.None;

            long? contentLength = null;
            DateTime? lastModified = null;

            // Try HEAD
            using (var req = new HttpRequestMessage(HttpMethod.Head, url))
            {
                req.Headers.UserAgent.ParseAdd(USER_AGENT);
                try
                {
                    using (var resp = await client.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, token))
                    {
                        resp.EnsureSuccessStatusCode();
                        if (resp.Content.Headers.ContentLength.HasValue)
                            contentLength = resp.Content.Headers.ContentLength.Value;
                        if (resp.Content.Headers.LastModified.HasValue)
                            lastModified = resp.Content.Headers.LastModified.Value.UtcDateTime;
                    }
                }
                catch (TaskCanceledException ex)
                {
                    if (token.IsCancellationRequested)
                        throw new OperationCanceledException(ex.Message, token); // cancelled by caller
                    else
                        throw new TimeoutException(ex.Message); // cancelled due to HTTP timeout
                }
                catch { /* HEAD failure → fallback to Range */ }
            }

            // GitHub Raw does NOT support Last-Modified
            if (contentLength != null && (lastModified != null || url.ToLower().Contains("raw.githubusercontent.com")))
                return (contentLength, lastModified);

            // Metadata still incomplete → Try GET with Range 0–0
            using (var req = new HttpRequestMessage(HttpMethod.Get, url))
            {
                req.Headers.UserAgent.ParseAdd(USER_AGENT);
                req.Headers.Range = new System.Net.Http.Headers.RangeHeaderValue(0, 0);
                try
                {
                    using (var resp = await client.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, token))
                    {
                        resp.EnsureSuccessStatusCode();
                        if (resp.Content.Headers.ContentRange?.Length.HasValue == true)
                            contentLength = resp.Content.Headers.ContentRange.Length.Value;
                        if (resp.Content.Headers.LastModified.HasValue)
                            lastModified = resp.Content.Headers.LastModified.Value.UtcDateTime;
                    }
                }
                catch (TaskCanceledException ex)
                {
                    if (token.IsCancellationRequested)
                        throw new OperationCanceledException(ex.Message, token); // cancelled by caller
                    else
                        throw new TimeoutException(ex.Message); // cancelled due to HTTP timeout
                }
            }
            return (contentLength, lastModified);
        }


    }
}