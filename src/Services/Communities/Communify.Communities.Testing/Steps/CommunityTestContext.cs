using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Communify.Communities.ApiFunctions;
using Communify.Communities.Common.Models;
using Communify.Communities.Testing.Scaffolding;
using Communify.Testing.AzureFunctions;
using Communify.Testing.AzureFunctions.Adapters;
using Communify.Testing.AzureFunctions.Adapters.Development;

namespace Communify.Communities.Testing.Steps
{
    public class CommunityTestContext
    {
        private readonly IFunctionTestAdapter _functionTestAdapter;
       

        public CommunityTestContext(IFunctionTestAdapter functionTestAdapter)
        {
            _functionTestAdapter = functionTestAdapter;
            if (_functionTestAdapter is IFunctionTestInitialiser functionTestInitialiser)
            {
                functionTestInitialiser.Initialise<TestStartup>(typeof(GetCommunityFunction).Assembly);
            }
        }

        public async Task CreateCommunity(TestCommunity community)
        {
            Result =
                    await _functionTestAdapter.ExecuteHttpTrigger<TestCommunity, NullPayload>(CreateCommunityFunction.CreateFunctionName, "put", community);

            
        }

        public async Task<Community> GetCommunityById(string globalId)
        {
            var response = await _functionTestAdapter.ExecuteHttpTrigger<NullPayload, Community>(GetCommunityFunction.GetFunctionName, "get", new NullPayload(),
                new KeyValuePair<string, string>("id", globalId));
            if (response is HttpResponse<Community> communityResponse)
            {
                return communityResponse.Body;
            }

            return null;
        }

        public HttpResponse Result { get; private set; }
    }
}