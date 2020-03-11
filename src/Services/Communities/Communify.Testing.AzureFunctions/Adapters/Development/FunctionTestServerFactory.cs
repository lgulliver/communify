using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Communify.Testing.AzureFunctions.Metadata;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Communify.Testing.AzureFunctions.Adapters.Development
{
    public static class FunctionTestServerFactory
    {
        private static List<FunctionDescriptor> _descriptors = new List<FunctionDescriptor>();
   
        public static TestServer Run<TStartup>(Assembly functionAssembly = null) where TStartup : FunctionsStartup, new()
        {
            var (host, functions) = Initialize<TStartup>(functionAssembly);
            
            _descriptors.AddRange(functions);
       
            var builder = new WebHostBuilder()
                .UseStartup<FunctionTestHostStartup>()
                .ConfigureServices((context, collection) =>
                {
                    collection.AddTransient(provider => new FunctionTypeFactory(host));
                    collection.AddSingleton(provider => functions);
                });
            
            var testServer = new TestServer(builder);
         //   await testServer.Host.StartAsync();
            return testServer;
        }


        private static (IHost, List<FunctionDescriptor>) Initialize<TStartup>(Assembly functionAssembly = null) where TStartup : FunctionsStartup, new()
        {
            if (functionAssembly == null)
            {
                functionAssembly = typeof(TStartup).Assembly;
            }

            var functions = functionAssembly.ToFunctionMethodDictionary<HttpTriggerAttribute>();
            

            if (!functions.Any())
            {
                throw new InvalidOperationException($"DevelopmentFunctionTestRunner found no HttpTrigger functions in {functionAssembly.FullName}");
            }

            
            return (
                new HostBuilder()
                .ConfigureFunctionsHost<TStartup>(functions.Values)
                .Build(), 
                functions.Select
                    (f => new HttpFunctionDescriptor { FunctionMethod = f.Value, Name = f.Key.ToUpper()})
                    .Cast<FunctionDescriptor>()
                    .ToList());
        }


    }

}
