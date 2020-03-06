using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Communify.Testing.AzureFunctions.Metadata;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging.Abstractions;

namespace Communify.Testing.AzureFunctions.Adapters.Development
{
    public class FunctionHandlerMiddleware
    {
        // ReSharper disable once UnusedParameter.Local
        public FunctionHandlerMiddleware(RequestDelegate next)
        {
        }

        // ReSharper disable once UnusedMember.Global
        public async Task InvokeAsync(HttpContext context, List<FunctionDescriptor> functionDescriptors, FunctionTypeFactory functionTypeFactory)
        {
            var path = context.Request.Path;
            var functionName = path.Value.Split('/').Last();
            if (functionName.Contains('?'))
            {
                functionName = functionName.Split('?').First();
            }

            if (functionName.Contains("/"))
            {
                functionName = functionName.Replace("/", string.Empty);
            }

           
            var functionDescriptor = functionDescriptors.FirstOrDefault(f =>
                f.Name.Equals(functionName, StringComparison.InvariantCultureIgnoreCase));

            if (functionDescriptor == null)
            {
                string fd = "functions: ";
                foreach (var descriptor in functionDescriptors)
                {
                    fd = $"{fd} {descriptor.Name} +";
                }
                throw new InvalidOperationException($"Could not resolve functions descriptor for function {functionName}. Functions found: {fd}");
            }

            if (functionDescriptor.FunctionMethod.IsStatic)
            {
                if (functionDescriptor.FunctionMethod.IsAsync())
                {
                    await functionDescriptor.FunctionMethod.InvokeAsync(null, context.Request);
                }
                else
                {
                    functionDescriptor.FunctionMethod.Invoke(null, new object[] {context.Request});
                }
            }
            else
            {
                // TODO: parameter resolution
                ActionResult actionResult;
                var function = functionTypeFactory.GetFunction(functionDescriptor);
                try
                {
                    if (functionDescriptor.FunctionMethod.IsAsync())
                    {
                        actionResult =
                            (ActionResult)await functionDescriptor.FunctionMethod.InvokeAsync(function, context.Request, NullLogger.Instance);
                    }
                    else
                    {
                        actionResult =
                            functionDescriptor.FunctionMethod.Invoke(function, new object[] { context.Request, NullLogger.Instance }) as
                                ActionResult;
                        if (actionResult == null)
                        {
                            throw new InvalidOperationException("Expected ActionResult from Function Method Invocation");
                        }
                    }

                    
                }
                catch (Exception e)
                {
                    var errorResult = new ObjectResult(e.ToString()) {StatusCode = 500};
                    actionResult = errorResult;
                }

                var actionContext = new ActionContext(context, new RouteData(), new ActionDescriptor());
                // ReSharper disable once PossibleNullReferenceException
                await actionResult?.ExecuteResultAsync(actionContext);
            }
        }
    }
}