using System.Threading.Tasks;
using Communify.Communities.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Communify.Communities.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CommunitiesController : ControllerBase
    {
        [HttpPut]
        public async Task<ActionResult> Create([FromServices]ICommunityRepository communityRepository,Community community)
        {
            await communityRepository.CreateCommunityAsync(community);
            return Ok();
        }
    }
}