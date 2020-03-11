using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Communify.Communities.Common;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Communify.Testing.AzureFunctions.Adapters.Development
{
    public class DevelopmentFunctionTestAdapter : IFunctionTestAdapter, IFunctionTestInitialiser
    {
        private TestServer _functionTestServer;
        private HttpClient _httpClient;

        public async Task<HttpResponse> ExecuteHttpTrigger<TRequestPayload, TResponsePayload>(string functionName, string method, TRequestPayload payload, 
            params KeyValuePair<string, string>[] queryString)
        {
            var response = await ExecuteHttpTriggerFunction(functionName, method, payload, queryString);


            if (response.Content != null)
            {
                if (response.IsSuccessStatusCode)
                {
                    var responsePayload = await response.Content.ReadAsAsync<TResponsePayload>();
                    return new HttpResponse<TResponsePayload>((int) response.StatusCode,
                        response.HeadersAsDictionary(), responsePayload);
                }

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var errorPayload = await response.Content.ReadAsAsync<IEnumerable<ValidationError>>();
                    return new HttpResponse<IEnumerable<ValidationError>>((int) response.StatusCode,
                        response.HeadersAsDictionary(), errorPayload);
                }
            }

            return new HttpResponse((int)response.StatusCode, response.HeadersAsDictionary());
       
        }

        public void Initialise<TStartup>(Assembly functionAssembly = null) where TStartup : FunctionsStartup, new()
        {
            _functionTestServer = FunctionTestServerFactory.Run<TStartup>(functionAssembly);
            _httpClient = _functionTestServer.CreateClient();
        }

        private async Task<HttpResponseMessage> ExecuteHttpTriggerFunction<TPayload>(string functionName, string method,
            TPayload payload,
            params KeyValuePair<string, string>[] queryStrings)
        {
            string url = $"{_functionTestServer.BaseAddress}/{functionName}{await queryStrings.GetQuerystring()}";

            return method.ToUpperInvariant() switch
            {
                "PUT" => await _httpClient.PutAsJsonAsync(payload, $"{_functionTestServer.BaseAddress}/{functionName}"),
                "POST" => await _httpClient.PostAsJsonAsync(payload, $"{_functionTestServer.BaseAddress}/{functionName}"),
                "GET" => await _httpClient.GetAsync(url),
                _ => throw new InvalidOperationException($"Unsupported HttpMethod: {method}")
            };

        }

        public void Dispose()
        {
            _functionTestServer?.Dispose();
            _httpClient?.Dispose();
        }
    }

    public static class Extensions2
    {
        public static async Task<HttpResponseMessage> PostAsJsonAsync<TPayload>(this HttpClient httpClient, TPayload payload, string url)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8);
            return await httpClient.PostAsync(url, stringContent);
        }

        public static async Task<HttpResponseMessage> PutAsJsonAsync<TPayload>(this HttpClient httpClient, TPayload payload, string url)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8);
            return await httpClient.PutAsync(url, stringContent);
        }

        public static IDictionary<string, string> HeadersAsDictionary(this HttpResponseMessage response)
        {
            return response.Headers?.ToDictionary(h => h.Key, pair => pair.Value.FirstOrDefault());
        }

    }

    public sealed class NullPayload
    {

    }
}
