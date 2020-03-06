using System.Threading.Tasks;
using Communify.Communities.Common;
using Communify.Communities.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;


namespace Communify.Communities.ApiFunctions
{
    public class CreateCommunityFunction
    {
        private readonly ICommunityRepository _communityRepository;
        public const string CreateFunctionName = "CreateCommunityFunction";

        public CreateCommunityFunction(ICommunityRepository communityRepository)
        {
            _communityRepository = communityRepository;
        }

        [FunctionName(CreateFunctionName)]
        public async Task<IActionResult> Create(
            [HttpTrigger(AuthorizationLevel.Function, "put")]
            HttpRequest req,
            ILogger log)
        {
            var communityValidatedModel = await req.GetJsonBody<Community, CommunityValidator>();
            if (!communityValidatedModel.IsValid)
            {
                return communityValidatedModel.ToBadRequestResult();
            }

            await _communityRepository.CreateCommunityAsync(communityValidatedModel.Value);

            return new CreatedResult($"{communityValidatedModel.Value.GlobalId}", communityValidatedModel.Value);
        }
    }
}
