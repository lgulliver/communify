using System;
using System.Collections.Generic;

namespace Communify.Testing.AzureFunctions
{
    public class HttpResponse<TResponsePayload> : HttpResponse
    {
        public TResponsePayload Body { get; }

        public HttpResponse(int statusCode, IDictionary<string, string> headers, TResponsePayload body) : base(statusCode, headers)
        {
            Body = body;
        }
    }

    public class HttpResponse
    {
        public HttpResponse(int statusCode, IDictionary<string, string> headers)
        {
            StatusCode = statusCode;
            Headers = headers;
        }

        public int StatusCode { get; }
        
        public IDictionary<string, string> Headers { get; }
    }

    public class HttpException : Exception
    {
        public int StatusCode { get; }
        public dynamic Content { get; }

        public HttpException(int statusCode, string message, dynamic content = null) : base(message)
        {
            StatusCode = statusCode;
            Content = content;
        }
    }
}
