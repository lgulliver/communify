using System;
using System.Threading.Tasks;
using Communify.Communities.Common.Models;

namespace Communify.Communities.Common
{
    public interface ICommunityRepository
    {
        Task CreateCommunityAsync(Community community);
        Task<Community> GetCommunityAsync(Guid communityId);
        Task<Community> UpdateCommunityAsync(Community community);
        Task<bool> DeleteCommunityAsync(string id);
    }
}
