using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Communify.Testing.AzureFunctions.Adapters.Development
{
    public static class Extensions
    {
        public static bool IsAsync(this MethodInfo methodInfo)
        {
            var attType = typeof(AsyncStateMachineAttribute);

            // Obtain the custom attribute for the method. 
            // The value returned contains the StateMachineType property. 
            // Null is returned if the attribute isn't present for the method. 
            var attribute = (AsyncStateMachineAttribute)methodInfo.GetCustomAttribute(attType);

            return (attribute != null);
        }

        public static async Task<object> InvokeAsync(this MethodInfo methodInfo, object obj, params object[] parameters)
        {
            var task = (Task)methodInfo.Invoke(obj, parameters);
            await task.ConfigureAwait(false);
            var resultProperty = task.GetType().GetProperty("Result"); // TODO: optimise to be faster
            return resultProperty?.GetValue(task);
        }
  
        public static IHostBuilder ConfigureFunctionsHost<TStartup>(this IHostBuilder hostBuilder, IEnumerable<MethodInfo> functions) where TStartup : FunctionsStartup, new()
        {
            var startup = new TStartup();
            return hostBuilder.ConfigureWebJobs(startup.Configure)
                .ConfigureServices(collection =>
                {
                    // This means we can test the Dependency Injection by making the function
                    // part of the dependency tree (which it is in the real functions framework).
                    foreach (var fm in functions)
                    {
                        collection.AddScoped(fm.DeclaringType);
                    }

                    collection.AddSingleton(CreateLogger());
                });
        }

        public static IDictionary<string, MethodInfo> ToFunctionMethodDictionary<TTrigger>(
            this Assembly functionAssembly) where TTrigger : Attribute
        {
            return functionAssembly.ToFunctionMethodDictionary()
                .Where(kvp => kvp.Value.GetParameters().First().GetCustomAttributes(typeof(TTrigger)).Any())
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }
        public static IDictionary<string, MethodInfo> ToFunctionMethodDictionary(this Assembly functionAssembly)
        {

            var assyTypes = functionAssembly.GetTypes();

            return assyTypes.SelectMany(t => t.GetMethods().Where(mi => mi.GetCustomAttribute<FunctionNameAttribute>() != null))
                .ToDictionary(mi => mi.GetCustomAttribute<FunctionNameAttribute>().Name);

        }
        public static async Task<string> GetQuerystring(this IEnumerable<KeyValuePair<string, string>> queryStringValues)
        {
            if (queryStringValues != null && queryStringValues.ToList().Any())
            {
                using var content = new FormUrlEncodedContent(queryStringValues);
                return "?" + await content.ReadAsStringAsync();
            }

            return string.Empty;
        }

        public static async Task<HttpResponseMessage> ThrowIfUnsuccessful(this Task<HttpResponseMessage> responseTask, IContentDeserializer errorDeserializer)
        {
            var response = await responseTask;
            if (response.IsSuccessStatusCode)
            {
                return response;
            }

            dynamic content = null;
            if (response.Content != null)
            {
                try
                {
                    content = await errorDeserializer.Deserialize(response.Content);
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"Exception encountered trying to deserialize exception content: {exception.Message} - probably that the http request had an unexpected result");
                }

            }

            throw new HttpException((int)(response.StatusCode), $"{response.StatusCode} at {response.RequestMessage.Method} to {response.RequestMessage.RequestUri}", content);
        }

        private static ILogger CreateLogger()
        {
            var lf = LoggerFactory.Create(builder => builder.AddConsole());
            return lf.CreateLogger("AzureFunctionsTesting");
        }
    }

    public interface IContentDeserializer
    {
        Task<dynamic> Deserialize(HttpContent content);
    }
}
