using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace BadMelon.Tests
{
    internal class TestServerFactory
    {
        public static TestServer TestServer = new TestServer(WebHost.CreateDefaultBuilder().UseStartup<TestStartup>().UseEnvironment("Testing"));
    }
}