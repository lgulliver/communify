using System;
using System.Reflection;

namespace Communify.Testing.AzureFunctions.Metadata
{
    public abstract class FunctionDescriptor
    {
        public string Name { get; set; }
        public MethodInfo FunctionMethod { get; set; }
        public Type DeclaringType => FunctionMethod?.DeclaringType;
    }

    public class HttpFunctionDescriptor : FunctionDescriptor
    {

    }
}