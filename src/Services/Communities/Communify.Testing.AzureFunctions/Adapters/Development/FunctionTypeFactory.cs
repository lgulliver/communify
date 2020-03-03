using Communify.Testing.AzureFunctions.Metadata;
using Microsoft.Extensions.Hosting;

namespace Communify.Testing.AzureFunctions.Adapters.Development
{
    public class FunctionTypeFactory
    {
        private readonly IHost _host;

        public FunctionTypeFactory(IHost host)
        {
            _host = host;

        }
        public object GetFunction(FunctionDescriptor descriptor)
        {
            return _host.Services.GetService(descriptor.DeclaringType);
        }

    }
}