using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Serilog;

namespace BadMelon.Tests
{
    internal class TestServerFactory
    {
        public static TestServer TestServer = new TestServer(WebHost.CreateDefaultBuilder().UseStartup<TestStartup>().UseSerilog().UseEnvironment("Testing"));
    }
}