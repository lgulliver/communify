using Communify.Communities.Api.Models;
using TechTalk.SpecFlow;

namespace Communify.Communities.Tests
{
    public class Steps
    {
        private readonly CommunifyTestClient _communifyTestClient;

        public Steps(CommunifyTestClient communifyTestClient)
        {
            _communifyTestClient = communifyTestClient;
        }

        [Given(@"a communifier")]
        public void GivenACommunifier()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"a non-communifier")]
        public void GivenANon_Communifier()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"a valid new community")]
        public void GivenAValidNewCommunity()
        {
            _communifyTestClient.Community = new Community
                {Description = "Test Community Description", Name = "Test Community"};
        }

        [When(@"the community is posted")]
        public void WhenTheCommunityIsPosted()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the community should be created")]
        public void ThenTheCommunityShouldBeCreated()
        {
            ScenarioContext.Current.Pending();
        }

    }

    [Binding]
    public class CommunifyTestClient
    {
        public Community Community { get; set; }
        

    }
}
