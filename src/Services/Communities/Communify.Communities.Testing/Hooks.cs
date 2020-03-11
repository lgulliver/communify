using Communify.Testing.AzureFunctions.Adapters;
using Communify.Testing.AzureFunctions.Adapters.Development;
using TechTalk.SpecFlow;

namespace Communify.Communities.Testing
{
    [Binding]
    public class Hooks
    {
        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            featureContext.FeatureContainer.RegisterTypeAs<DevelopmentFunctionTestAdapter, IFunctionTestAdapter>();
        }

        //[BeforeScenario()]
        //public void BeforeScenario()
        //{

        //}
    }
}
