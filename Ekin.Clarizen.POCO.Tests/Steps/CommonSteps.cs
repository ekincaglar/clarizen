using System.Diagnostics;
using Ekin.Clarizen.POCO.Tests.Context;
using TechTalk.SpecFlow;

namespace Ekin.Clarizen.POCO.Tests.Steps
{
    [Binding]
    public class CommonSteps : BaseApiSteps
    {
        public CommonSteps(BaseContext context) : base(context)
        {
        }

        [Given(@"I kill all excel instances")]
        public void KillAllExcel()
        {
            foreach (Process item in Process.GetProcesses())
            {
                if (item.ProcessName.ToLower().Contains("excel"))
                {
                    item.Kill();
                }
            }
        }
    }
}