using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Communify.Communities.ApiFunctions
{
    public static class Extensions
    {
        /// <summary>
        /// Returns the deserialized request body.
        /// </summary>
        /// <typeparam name="T">Type used for deserialization of the request body.</typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public static async Task<T> GetJsonBody<T>(this HttpRequest request)
        {
            var requestBody = await ReadUnseekableStreamAsStringAsync(request);

            return JsonConvert.DeserializeObject<T>(requestBody);
        }

        private const int DefaultBufferSize = 1024;

        /// <summary>
        /// This has been lifted from the Functions SDK and tweaked as the upgrade to .NET Core 3.1 caused an incompatibility
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static async Task<string> ReadUnseekableStreamAsStringAsync(this HttpRequest request)
        {
            using var reader = new StreamReader(
                request.Body,
                encoding: Encoding.UTF8,
                detectEncodingFromByteOrderMarks: true,
                bufferSize: DefaultBufferSize,
                leaveOpen: true);
            var buffer = new Memory<char>(new char[DefaultBufferSize]);
            await reader.ReadAsync(buffer);
            return buffer.ToString().Trim();
        }
    }
}


