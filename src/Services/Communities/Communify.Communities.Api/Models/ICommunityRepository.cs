using System;
using System.Threading.Tasks;

namespace Communify.Communities.Api.Models
{
    public interface ICommunityRepository
    {
        Task CreateCommunityAsync(Community community);
        Task<Community> GetCommunityAsync(Guid communityId);
        Task<Community> UpdateCommunityAsync(Community community);
        Task<bool> DeleteCommunityAsync(string id);
    }
}
