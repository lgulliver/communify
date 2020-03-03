using System;
using System.Net;
using System.Threading.Tasks;
using Communify.Communities.Common.Models;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Communify.Communities.Testing.Steps
{
    [Binding]
    public class Steps
    {
        private readonly CommunityTestContext _communityTestContext;
        private readonly Guid _communityId = Guid.NewGuid();
        private Community _community;

        public Steps(CommunityTestContext communityTestContext)
        {
            _communityTestContext = communityTestContext;
        }

        [Given(@"a community has formed")]
        public void GivenACommunityHasFormed()
        {
            _community = new Community
            {
                GlobalId = _communityId,
                Description = $"Description of {_communityId}",
                Name = $"{_communityId} Name"
            };
        }

        [When(@"the community is created")]
        public async Task WhenTheCommunityIsCreated()
        {
            await _communityTestContext.CreateCommunity(_community);
        }

        [Then(@"the community creation was successful")]
        public void ThenTheCommunityCreationWasSuccessful()
        {
            Assert.IsNotNull(_communityTestContext.Result);
            Assert.AreEqual((int)HttpStatusCode.Created, _communityTestContext.Result.StatusCode);
        }

        [Then(@"the community can be accessed")]
        public async Task ThenTheCommunityCanBeAccessed()
        {
            var community = await _communityTestContext.GetCommunityById(_communityId);
            Assert.IsNotNull(community);
            Assert.AreEqual(_community.Name, community.Name);
        }


    }
}
