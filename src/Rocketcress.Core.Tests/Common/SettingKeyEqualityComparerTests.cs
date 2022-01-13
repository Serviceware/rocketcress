using Rocketcress.Core.Common;

namespace Rocketcress.Core.Tests.Common;

[TestClass]
public class SettingKeyEqualityComparerTests : TestClassBase
{
    [TestMethod]
    [DataRow("Test", "Test", DisplayName = "No tag")]
    [DataRow("[str]Test", "Test", DisplayName = "With tag no spaces")]
    [DataRow("[str] Test", "Test", DisplayName = "With tag one space")]
    [DataRow("[str]   Test", "Test", DisplayName = "With tag multiple spaces")]
    public void GetKey(string input, string expectedOutput)
    {
        var actualOutput = SettingKeyEqualityComparer.GetKey(input);
        Assert.AreEqual(expectedOutput, actualOutput);
    }

    [TestMethod]
    [DataRow("Test", "Test", true)]
    [DataRow("Test", "test", false)]
    [DataRow("[str]Test", "Test", true)]
    [DataRow("[str]   Test", "Test", true)]
    [DataRow("[str] Test", "[int] Test", true)]
    public void Equals_(string firstString, string secondString, bool expectedOutput)
    {
        var comparer = new SettingKeyEqualityComparer();
        var actualOutput = comparer.Equals(firstString, secondString);
        Assert.AreEqual(expectedOutput, actualOutput);
    }

    [TestMethod]
    [DataRow("Test", "Test")]
    [DataRow("[str]Test", "Test")]
    [DataRow("[str] Test", "Test")]
    [DataRow("[str]   Test", "Test")]
    public void GetHashCode_(string input, string expectedHashCodeBase)
    {
        var comparer = new SettingKeyEqualityComparer();
        var actualHashCode = comparer.GetHashCode(input);
        Assert.AreEqual(expectedHashCodeBase.GetHashCode(), actualHashCode);
    }
}
