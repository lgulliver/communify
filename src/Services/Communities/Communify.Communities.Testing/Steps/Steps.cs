using System;
using System.Net;
using System.Threading.Tasks;
using Communify.Communities.Common.Models;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Communify.Communities.Testing.Steps
{
    public class TestCommunity
    {
        public string GlobalId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    [Binding]
    public class Steps
    {
        private readonly CommunityTestContext _communityTestContext;
        private readonly string _communityId = Guid.NewGuid().ToString();
        private TestCommunity _community;

        public Steps(CommunityTestContext communityTestContext)
        {
            _communityTestContext = communityTestContext;
        }

        [Given(@"a community has formed")]
        public void GivenACommunityHasFormed()
        {
            _community = new TestCommunity
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

        [Given(@"the community is called (.*)")]
        public void GivenTheCommunityIsCalled(string name)
        {
            _community.Name = name;
        }

        [Given(@"the community is described as (.*)")]
        public void GivenTheCommunityIsDescribedAs(string description)
        {
            _community.Description = description;
        }

        [Given(@"the community has been given an id of (.*)")]
        public void GivenTheCommunityHasBeenGivenAnIdOf(string id)
        {
            _community.GlobalId = id;
        }

        [Then(@"the community creation has a result of (.*)")]
        public void ThenTheCommunityCreationHasAResultOf(HttpStatusCode statusCode)
        {
             Assert.AreEqual(statusCode, (HttpStatusCode)_communityTestContext.Result.StatusCode);
        }
    }
}
