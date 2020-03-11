using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communify.Communities.ApiFunctions;
using Communify.Communities.Common;
using Communify.Communities.Common.Models;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Communify.Communities.Testing.Scaffolding
{
    public class TestStartup : Startup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddScoped<ICommunityRepository, TestCommunityRepository>();
        }
    }

    public class TestCommunityRepository : ICommunityRepository
    {
        private readonly List<Community> _communities;
        public TestCommunityRepository()
        {
            _communities = new List<Community>();
        }
        public Task CreateCommunityAsync(Community community)
        {
            return Task.Run(() => _communities.Add(community));
        }

        public Task<Community> GetCommunityAsync(Guid communityId)
        {
            return Task.Run(() => _communities.FirstOrDefault(c => c.GlobalId == communityId)); 
        }

        public Task<Community> UpdateCommunityAsync(Community community)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteCommunityAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
