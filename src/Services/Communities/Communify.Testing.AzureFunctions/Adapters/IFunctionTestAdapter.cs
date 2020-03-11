using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;

namespace Communify.Testing.AzureFunctions.Adapters
{
    public interface IFunctionTestAdapter : IDisposable
    {
        Task<HttpResponse> ExecuteHttpTrigger<TRequestPayload, TResponsePayload>(string functionName, string method, TRequestPayload payload,
            params KeyValuePair<string, string>[] queryString);
    }

    public interface IFunctionTestInitialiser
    {
        void Initialise<TStartup>(Assembly functionAssembly = null) where TStartup : FunctionsStartup, new();
    }
}