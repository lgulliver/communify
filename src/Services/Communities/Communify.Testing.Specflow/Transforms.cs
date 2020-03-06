using System;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using TechTalk.SpecFlow.Assist.ValueRetrievers;

namespace Communify.Testing.Specflow
{
    [Binding]
    public class Transforms
    {
        public const string DefaultNullStringValue = "<null>";
        public const string DefaultGuidStringValue = "<guid>";

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
          Service.Instance.ValueRetrievers.Register(new NullValueRetriever(DefaultNullStringValue));
        }

        [StepArgumentTransformation]
        public string
            NullOrGuidAsString(
                string value) // TODO: see if this can be replaced by registering a NullableDecimalValueRetriever with the right function
        {
            if (value.Equals(DefaultGuidStringValue, StringComparison.InvariantCultureIgnoreCase))
            {
                return Guid.NewGuid().ToString();
            }

            if (value.Equals(Transforms.DefaultNullStringValue, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            return value;
        }

        [StepArgumentTransformation]
        public HttpStatusCode
            ToHttpStatusCode(
                string value) // TODO: see if this can be replaced by registering a NullableDecimalValueRetriever with the right function
        {
            return value.ToLowerInvariant().Replace(" ", string.Empty) switch
            {
                "ok" => HttpStatusCode.OK,
                "badrequest" => HttpStatusCode.BadRequest,
                "servererror" => HttpStatusCode.InternalServerError,
                "created" => HttpStatusCode.Created,
                "oknocontent" => HttpStatusCode.NoContent,
                _ => throw new InvalidOperationException($"Unknown result value in feature file: {value}")
            };
        }
    }


}
