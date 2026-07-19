namespace Blogging.Api.Tests;

[TestClass]
public sealed class BootstrapTests
{
    [TestMethod]
    public void TestRuntimeTargetsDotnetTen()
    {
        Assert.AreEqual(10, Environment.Version.Major);
    }
}
