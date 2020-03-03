using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Communify.Communities.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Communify.Communities.ApiFunctions
{
    public class GetCommunityFunction
    {
        private readonly ICommunityRepository _communityRepository;
        public const string GetFunctionName = "GetCommunityFunction";

        public GetCommunityFunction(ICommunityRepository communityRepository)
        {
            _communityRepository = communityRepository;
        }

        [FunctionName(GetFunctionName)]
        public async Task<IActionResult> Get(
            [HttpTrigger(AuthorizationLevel.Function, Route = null)]
            HttpRequest req,
            ILogger log)
        {
            var globalId =
                req.Query.FirstOrDefault(qs => qs.Key.Equals("id", StringComparison.InvariantCultureIgnoreCase));
            if (!string.IsNullOrEmpty(globalId.Value))
            {
                var communityId = Guid.Parse(globalId.Value);
                var community = await _communityRepository.GetCommunityAsync(communityId);
                return new OkObjectResult(community);
            }

            return new InternalServerErrorResult();
        }
    }
}