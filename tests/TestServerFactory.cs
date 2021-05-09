using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Serilog;

namespace BadMelon.Tests
{
    internal class TestServerFactory
    {
        private static TestServer testServer = null;

        public static TestServer GetTestServer()
        {
            if (testServer == null)
                testServer = new TestServer(WebHost.CreateDefaultBuilder().UseStartup<TestStartup>().UseSerilog().UseEnvironment("Testing"));

            return testServer;
        }
    }
}