using TechTalk.SpecFlow;

namespace Ekin.Clarizen.Tests.Hooks
{
    public static class FeatureHooks
    {
        [BeforeFeature]
        public static void CleanUpFeature()
        {
            TimeProvider.ResetToDefault();
        }
    }
}