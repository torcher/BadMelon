using Xunit;

namespace BadMelon.Tests.Fixtures
{
    [CollectionDefinition("Controller collection")]
    public class ControllerCollection : ICollectionFixture<ControllerTestsFixture>
    {
    }
}