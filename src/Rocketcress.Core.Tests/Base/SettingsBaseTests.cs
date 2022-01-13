using Newtonsoft.Json.Linq;
using Rocketcress.Core.Base;
using System.Reflection;

namespace Rocketcress.Core.Tests.Base;

[TestClass]
public class SettingsBaseTests : TestClassBase
{
    private static readonly SettingsBase SettingsWith7Keys = new SettingsBase()
    {
        OtherSettings =
        {
            ["Test1"] = 1,
            ["Test2"] = 2,
            ["Test3"] = 3,
            ["Test4"] = 4,
            ["Test5"] = 5,
            ["Test6"] = 6,
            ["Test7"] = 7,
        },
    };

    [TestMethod]
    public void GetT_MissingKey()
    {
        var settings = new SettingsBase();

        Assert.IsNull(settings.Get<string>("Test"));
    }

    [TestMethod]
    public void GetT_Null()
    {
        var settings = new SettingsBase();
        settings.OtherSettings["Test"] = null;

        Assert.AreEqual(0, settings.Get<int>("Test"));
        Assert.AreEqual(null, settings.Get<object>("Test"));
    }

    [TestMethod]
    public void GetT_WithDefaultValue()
    {
        var settings = new SettingsBase();

        Assert.AreEqual(4711, settings.Get<int>("Test", 4711));

        var defaultObj = new object();
        Assert.AreSame(defaultObj, settings.Get<object>("Test", defaultObj));
    }

    [TestMethod]
    public void GetT_CorrectType()
    {
        var settings = new SettingsBase();
        var testObject = new TestObject1();
        settings.OtherSettings["Test"] = testObject;

        var result = settings.Get<TestObject1>("Test");
        Assert.AreSame(testObject, result);
    }

    [TestMethod]
    public void GetT_NoGetterFunction_NotJToken()
    {
        var settings = new SettingsBase();
        settings.OtherSettings["Test"] = new object();

        Assert.IsNull(settings.Get<TestObject1>("Test"));
    }

    [TestMethod]
    public void GetT_WithDefaultValue_NoGetterFunction_NotJToken()
    {
        var settings = new SettingsBase();
        settings.OtherSettings["Test"] = new object();

        var defaultValue = new TestObject1();
        Assert.AreSame(defaultValue, settings.Get<TestObject1>("Test", defaultValue));
    }

    [TestMethod]
    public void GetT_JToken()
    {
        var settings = new SettingsBase();
        settings.OtherSettings["Test"] = JToken.FromObject(new TestObject1 { StringValue = "Blubbi123" });

        var result = settings.Get<TestObject1>("Test");
        Assert.IsNotNull(result);
        Assert.AreEqual("Blubbi123", result.StringValue);
    }

    [TestMethod]
    [DataRow(null, null, DisplayName = "Null")]
    [DataRow(5, "5", DisplayName = "Integer")]
    [DataRow(5L, "5", DisplayName = "Long")]
    [DataRow(5.5D, "5.5", DisplayName = "Double")]
    [DataRow("Object", "System.Object", DisplayName = "Object")]
    public void GetT_String(object actualValue, string expectedOutput)
    {
        var settings = new SettingsBase();
        settings.OtherSettings["Test"] = Equals(actualValue, "Object") ? new object() : actualValue;

        Assert.AreEqual(expectedOutput, settings.Get<string>("Test"));
    }

    [TestMethod]
    [DataRow(null, (short)0, DisplayName = "Null")]
    [DataRow(5, (short)5, DisplayName = "Integer in range")]
    [DataRow(5L, (short)5, DisplayName = "Long in range")]
    [DataRow("5", (short)5, DisplayName = "String with correct format")]
    public void GetT_Short_Success(object actualValue, short expectedOutput)
    {
        var settings = new SettingsBase();
        settings.OtherSettings["Test"] = actualValue;

        Assert.AreEqual(expectedOutput, settings.Get<short>("Test"));
    }

    [TestMethod]
    [DataRow(5.5D, typeof(FormatException), DisplayName = "Double")]
    [DataRow(32768, typeof(OverflowException), DisplayName = "Integer bigger than short.MaxValue")]
    [DataRow("32768", typeof(OverflowException), DisplayName = "String representing number bigger than short.MaxValue")]
    [DataRow(-32769, typeof(OverflowException), DisplayName = "Integer smaller than short.MinValue")]
    [DataRow("-32769", typeof(OverflowException), DisplayName = "String representing number smaller than short.MinValue")]
    [DataRow("Object", typeof(FormatException), DisplayName = "Object")]
    [DataRow("Hello", typeof(FormatException), DisplayName = "String with incorrect format")]
    public void GetT_Short_Error(object actualValue, Type expectedErrorType)
    {
        var settings = new SettingsBase();
        settings.OtherSettings["Test"] = Equals(actualValue, "Object") ? new object() : actualValue;

        Assert.ThrowsException(expectedErrorType, () => settings.Get<short>("Test"));
    }

    [TestMethod]
    [DataRow(null, 0, DisplayName = "Null")]
    [DataRow(5L, 5, DisplayName = "Long in range")]
    [DataRow("5", 5, DisplayName = "String with correct format")]
    public void GetT_Int_Success(object actualValue, int expectedOutput)
    {
        var settings = new SettingsBase();
        settings.OtherSettings["Test"] = actualValue;

        Assert.AreEqual(expectedOutput, settings.Get<int>("Test"));
    }

    [TestMethod]
    [DataRow(5.5D, typeof(FormatException), DisplayName = "Double")]
    [DataRow(2147483648L, typeof(OverflowException), DisplayName = "Long bigger than int.MaxValue")]
    [DataRow("2147483648", typeof(OverflowException), DisplayName = "String representing number bigger than int.MaxValue")]
    [DataRow(-2147483649L, typeof(OverflowException), DisplayName = "Long smaller than int.MinValue")]
    [DataRow("-2147483649", typeof(OverflowException), DisplayName = "String representing number smaller than int.MinValue")]
    [DataRow("Object", typeof(FormatException), DisplayName = "Object")]
    [DataRow("Hello", typeof(FormatException), DisplayName = "String with incorrect format")]
    public void GetT_Int_Error(object actualValue, Type expectedErrorType)
    {
        var settings = new SettingsBase();
        settings.OtherSettings["Test"] = Equals(actualValue, "Object") ? new object() : actualValue;

        Assert.ThrowsException(expectedErrorType, () => settings.Get<int>("Test"));
    }

    [TestMethod]
    [DataRow(null, 0L, DisplayName = "Null")]
    [DataRow(5, 5L, DisplayName = "Integer")]
    [DataRow("5", 5L, DisplayName = "String with correct format")]
    public void GetT_Long_Success(object actualValue, long expectedOutput)
    {
        var settings = new SettingsBase();
        settings.OtherSettings["Test"] = actualValue;

        Assert.AreEqual(expectedOutput, settings.Get<long>("Test"));
    }

    [TestMethod]
    [DataRow(5.5D, typeof(FormatException), DisplayName = "Double")]
    [DataRow("9223372036854775808", typeof(OverflowException), DisplayName = "String representing number bigger than long.MaxValue")]
    [DataRow("-9223372036854775809", typeof(OverflowException), DisplayName = "String representing number smaller than long.MinValue")]
    [DataRow("Object", typeof(FormatException), DisplayName = "Object")]
    [DataRow("Hello", typeof(FormatException), DisplayName = "String with incorrect format")]
    public void GetT_Long_Error(object actualValue, Type expectedErrorType)
    {
        var settings = new SettingsBase();
        settings.OtherSettings["Test"] = Equals(actualValue, "Object") ? new object() : actualValue;

        Assert.ThrowsException(expectedErrorType, () => settings.Get<long>("Test"));
    }

    [TestMethod]
    [DataRow(null, 0F, DisplayName = "Null")]
    [DataRow(5L, 5F, DisplayName = "Long")]
    [DataRow(5.5D, 5.5F, DisplayName = "Double")]
    [DataRow("-5.5", -5.5F, DisplayName = "String with correct format")]
    public void GetT_Float_Success(object actualValue, float expectedOutput)
    {
        var settings = new SettingsBase();
        settings.OtherSettings["Test"] = actualValue;

        Assert.AreEqual(expectedOutput, settings.Get<float>("Test"));
    }

    [TestMethod]
    [DataRow("Object", typeof(FormatException), DisplayName = "Object")]
    [DataRow("Hello", typeof(FormatException), DisplayName = "String with incorrect format")]
    public void GetT_Float_Error(object actualValue, Type expectedErrorType)
    {
        var settings = new SettingsBase();
        settings.OtherSettings["Test"] = Equals(actualValue, "Object") ? new object() : actualValue;

        Assert.ThrowsException(expectedErrorType, () => settings.Get<float>("Test"));
    }

    [TestMethod]
    [DataRow(null, 0D, DisplayName = "Null")]
    [DataRow(5L, 5D, DisplayName = "Long")]
    [DataRow(5.5F, 5.5D, DisplayName = "Float")]
    [DataRow("-5.5", -5.5D, DisplayName = "String with correct format")]
    public void GetT_Double_Success(object actualValue, double expectedOutput)
    {
        var settings = new SettingsBase();
        settings.OtherSettings["Test"] = actualValue;

        Assert.AreEqual(expectedOutput, settings.Get<double>("Test"));
    }

    [TestMethod]
    [DataRow("Object", typeof(FormatException), DisplayName = "Object")]
    [DataRow("Hello", typeof(FormatException), DisplayName = "String with incorrect format")]
    public void GetT_Double_Error(object actualValue, Type expectedErrorType)
    {
        var settings = new SettingsBase();
        settings.OtherSettings["Test"] = Equals(actualValue, "Object") ? new object() : actualValue;

        Assert.ThrowsException(expectedErrorType, () => settings.Get<double>("Test"));
    }

    [TestMethod]
    [DataRow(null, false, DisplayName = "Null")]
    [DataRow(true, true, DisplayName = "Boolean")]
    [DataRow(0, false, DisplayName = "Int 0")]
    [DataRow(1, true, DisplayName = "Int 1")]
    [DataRow(-5, true, DisplayName = "Int negative")]
    [DataRow(5, true, DisplayName = "Int 5")]
    [DataRow("true", true, DisplayName = "String \"true\"")]
    [DataRow("false", false, DisplayName = "String \"false\"")]
    [DataRow("0", false, DisplayName = "String \"0\"")]
    [DataRow("1", true, DisplayName = "String \"1\"")]
    [DataRow("-5", true, DisplayName = "String \"-5\"")]
    [DataRow("5", true, DisplayName = "String \"5\"")]
    public void GetT_Boolean_Success(object actualValue, bool expectedOutput)
    {
        var settings = new SettingsBase();
        settings.OtherSettings["Test"] = actualValue;

        Assert.AreEqual(expectedOutput, settings.Get<bool>("Test"));
    }

    [TestMethod]
    [DataRow(5.5D, typeof(FormatException), DisplayName = "Double")]
    [DataRow("Object", typeof(FormatException), DisplayName = "Object")]
    [DataRow("Hello", typeof(FormatException), DisplayName = "String with incorrect format")]
    public void GetT_Boolean_Error(object actualValue, Type expectedErrorType)
    {
        var settings = new SettingsBase();
        settings.OtherSettings["Test"] = Equals(actualValue, "Object") ? new object() : actualValue;

        Assert.ThrowsException(expectedErrorType, () => settings.Get<bool>("Test"));
    }

    [TestMethod]
    [DataRow("2022-01-13", "2022-01-13T00:00:00.0000000Z", DisplayName = "ISO Date")]
    [DataRow("2022-01-13T11:57:00.000", "2022-01-13T11:57:00.0000000Z", DisplayName = "ISO Date and time")]
    [DataRow("01/13/2022", "2022-01-13T00:00:00.0000000Z", DisplayName = "Invariant Date")]
    [DataRow("01/13/2022 11:57:00", "2022-01-13T11:57:00.0000000Z", DisplayName = "Invariant Date and time")]
    public void GetT_DateTime_Success(object actualValue, string expectedIsoDate)
    {
        var settings = new SettingsBase();
        settings.OtherSettings["Test"] = actualValue;

        Assert.AreEqual(expectedIsoDate, DateTime.SpecifyKind(settings.Get<DateTime>("Test"), DateTimeKind.Utc).ToString("O"));
    }

    [TestMethod]
    [DataRow("Object", typeof(FormatException), DisplayName = "Object")]
    [DataRow("Hello", typeof(FormatException), DisplayName = "String with incorrect format")]
    public void GetT_DateTime_Error(object actualValue, Type expectedErrorType)
    {
        var settings = new SettingsBase();
        settings.OtherSettings["Test"] = Equals(actualValue, "Object") ? new object() : actualValue;

        Assert.ThrowsException(expectedErrorType, () => settings.Get<DateTime>("Test"));
    }

    [TestMethod]
    [DataRow(null, null, DisplayName = "Null")]
    [DataRow("https://www.google.de", "https://www.google.de/", DisplayName = "String Url")]
    public void GetT_Uri_Success(object actualValue, string expectedUri)
    {
        var settings = new SettingsBase();
        settings.OtherSettings["Test"] = actualValue;

        Assert.AreEqual(expectedUri, settings.Get<Uri>("Test")?.ToString());
    }

    [TestMethod]
    [DataRow("Object", typeof(UriFormatException), DisplayName = "Object")]
    [DataRow("Hello", typeof(UriFormatException), DisplayName = "String with incorrect format")]
    public void GetT_Uri_Error(object actualValue, Type expectedErrorType)
    {
        var settings = new SettingsBase();
        settings.OtherSettings["Test"] = Equals(actualValue, "Object") ? new object() : actualValue;

        Assert.ThrowsException(expectedErrorType, () => settings.Get<Uri>("Test"));
    }

    [TestMethod]
    public void GetT2()
    {
        var actual = SettingsWith7Keys.Get<int, int>("Test1", "Test2");
        Assert.AreEqual((1, 2), actual);
    }

    [TestMethod]
    public void GetT3()
    {
        var actual = SettingsWith7Keys.Get<int, int, int>("Test1", "Test2", "Test3");
        Assert.AreEqual((1, 2, 3), actual);
    }

    [TestMethod]
    public void GetT4()
    {
        var actual = SettingsWith7Keys.Get<int, int, int, int>("Test1", "Test2", "Test3", "Test4");
        Assert.AreEqual((1, 2, 3, 4), actual);
    }

    [TestMethod]
    public void GetT5()
    {
        var actual = SettingsWith7Keys.Get<int, int, int, int, int>("Test1", "Test2", "Test3", "Test4", "Test5");
        Assert.AreEqual((1, 2, 3, 4, 5), actual);
    }

    [TestMethod]
    public void GetT6()
    {
        var actual = SettingsWith7Keys.Get<int, int, int, int, int, int>("Test1", "Test2", "Test3", "Test4", "Test5", "Test6");
        Assert.AreEqual((1, 2, 3, 4, 5, 6), actual);
    }

    [TestMethod]
    public void GetT7()
    {
        var actual = SettingsWith7Keys.Get<int, int, int, int, int, int, int>("Test1", "Test2", "Test3", "Test4", "Test5", "Test6", "Test7");
        Assert.AreEqual((1, 2, 3, 4, 5, 6, 7), actual);
    }

    [TestMethod]
    public void TryGetT_KeyDoesNotExist()
    {
        bool success = SettingsWith7Keys.TryGet<int>("Blubbi", out int value);
        Assert.IsFalse(success);
        Assert.AreEqual(0, value);
    }

    [TestMethod]
    public void TryGetT2()
    {
        bool success = SettingsWith7Keys.TryGet<int, int>("Test1", "Test2", out var actual);
        Assert.IsTrue(success);
        Assert.AreEqual((1, 2), actual);
    }

    [TestMethod]
    public void TryGetT3()
    {
        bool success = SettingsWith7Keys.TryGet<int, int, int>("Test1", "Test2", "Test3", out var actual);
        Assert.IsTrue(success);
        Assert.AreEqual((1, 2, 3), actual);
    }

    [TestMethod]
    public void TryGetT4()
    {
        bool success = SettingsWith7Keys.TryGet<int, int, int, int>("Test1", "Test2", "Test3", "Test4", out var actual);
        Assert.IsTrue(success);
        Assert.AreEqual((1, 2, 3, 4), actual);
    }

    [TestMethod]
    public void TryGetT5()
    {
        bool success = SettingsWith7Keys.TryGet<int, int, int, int, int>("Test1", "Test2", "Test3", "Test4", "Test5", out var actual);
        Assert.IsTrue(success);
        Assert.AreEqual((1, 2, 3, 4, 5), actual);
    }

    [TestMethod]
    public void TryGetT6()
    {
        bool success = SettingsWith7Keys.TryGet<int, int, int, int, int, int>("Test1", "Test2", "Test3", "Test4", "Test5", "Test6", out var actual);
        Assert.IsTrue(success);
        Assert.AreEqual((1, 2, 3, 4, 5, 6), actual);
    }

    [TestMethod]
    public void TryGetT7()
    {
        bool success = SettingsWith7Keys.TryGet<int, int, int, int, int, int, int>("Test1", "Test2", "Test3", "Test4", "Test5", "Test6", "Test7", out var actual);
        Assert.IsTrue(success);
        Assert.AreEqual((1, 2, 3, 4, 5, 6, 7), actual);
    }

    [TestMethod]
    [DataRow(true, false, DisplayName = "Debug")]
    [DataRow(false, false, DisplayName = "Release")]
    [DataRow(true, true, DisplayName = "Force Default")]
    public void GetSettingFile(bool debug, bool forceDefault)
    {
        var testFolder = Path.Combine(TestContext.DeploymentDirectory, Guid.NewGuid().ToString());
        var settingsFolder = Path.Combine(testFolder, "TestSettings");
        Directory.CreateDirectory(settingsFolder);
        var filePaths = new[]
        {
            Path.Combine(settingsFolder, $"Blubbi.settings_{Environment.MachineName}.json"),
            Path.Combine(settingsFolder, "Blubbi.settings_debug.json"),
            Path.Combine(settingsFolder, "Blubbi.settings.json"),
            Path.Combine(settingsFolder, $"settings_{Environment.MachineName}.json"),
            Path.Combine(settingsFolder, "settings_debug.json"),
            Path.Combine(settingsFolder, "settings.json"),
            Path.Combine(testFolder, $"Blubbi.settings_{Environment.MachineName}.json"),
            Path.Combine(testFolder, "Blubbi.settings_debug.json"),
            Path.Combine(testFolder, "Blubbi.settings.json"),
            Path.Combine(testFolder, $"settings_{Environment.MachineName}.json"),
            Path.Combine(testFolder, "settings_debug.json"),
            Path.Combine(testFolder, "settings.json"),
        };
        foreach (var file in filePaths)
            File.WriteAllBytes(file, Array.Empty<byte>());
        var assembly = CreateFakeAssembly("Blubbi", Path.Combine(testFolder, "Blubbi.dll"));
        TestHelper.IsDebugConfiguration = debug;

        for (int i = 0; i < filePaths.Length; i++)
        {
            if ((!debug && (i % 3) == 1) || (forceDefault && (i % 3) != 2))
                continue;

            var result = SettingsBase.GetSettingFile(assembly, null, forceDefault);
            Assert.AreEqual(filePaths[i], result);
            File.Delete(filePaths[i]);
        }
    }

    [TestMethod]
    public void GetFromFile_InvalidFilePath()
    {
        Assert.ThrowsException<ArgumentException>(() => SettingsBase.GetFromFile<SettingsBase>(null));
        Assert.ThrowsException<ArgumentException>(() => SettingsBase.GetFromFile<SettingsBase>(string.Empty));
        Assert.ThrowsException<ArgumentException>(() => SettingsBase.GetFromFile<SettingsBase>("    "));
    }

    [TestMethod]
    public void GetFromFile_MissingFile()
    {
        var filePath = Path.Combine(TestContext.DeploymentDirectory, Guid.NewGuid().ToString() + ".json");
        Assert.ThrowsException<FileNotFoundException>(() => SettingsBase.GetFromFile<SettingsBase>(filePath));
    }

    [TestMethod]
    public void GetFromFile_Success()
    {
        var filePath = Path.Combine(TestContext.DeploymentDirectory, Guid.NewGuid().ToString() + ".json");
        File.WriteAllText(filePath, $"{{ \"Timeout\":\"00:00:15\", \"Username\":\"MyUser\", \"Password\":\"MyPassword\", \"Language\":\"German\", \"OtherSettings\": {{ \"MySetting1\":123 }}, \"SettingsTypes\":[{{ \"TypeName\":\"System.String\", \"TagName\":\"str\" }}] }}");
        var settings = SettingsBase.GetFromFile<SettingsBase>(filePath);

        Assert.AreEqual(15, settings.Timeout.TotalSeconds);
        Assert.AreEqual("MyUser", settings.Username);
        Assert.AreEqual("MyPassword", settings.Password);
        Assert.AreEqual(LanguageOptions.German, settings.Language);
        Assert.AreEqual(123L, settings.OtherSettings["MySetting1"]);
        Assert.AreEqual("System.String", settings.SettingsTypes[0].TypeName);
        Assert.AreEqual("str", settings.SettingsTypes[0].TagName);
    }

    [TestMethod]
    public void GetFromFiles_InvalidFilePath()
    {
        var defaultFilePath = Path.Combine(TestContext.DeploymentDirectory, Guid.NewGuid().ToString() + ".json");
        Assert.ThrowsException<ArgumentException>(() => SettingsBase.GetFromFiles<SettingsBase>(null, defaultFilePath));
        Assert.ThrowsException<ArgumentException>(() => SettingsBase.GetFromFiles<SettingsBase>(string.Empty, defaultFilePath));
        Assert.ThrowsException<ArgumentException>(() => SettingsBase.GetFromFiles<SettingsBase>("    ", defaultFilePath));
    }

    [TestMethod]
    public void GetFromFiles_InvalidDefaultFilePath()
    {
        var filePath = Path.Combine(TestContext.DeploymentDirectory, Guid.NewGuid().ToString() + ".json");
        Assert.ThrowsException<ArgumentException>(() => SettingsBase.GetFromFiles<SettingsBase>(filePath, null));
        Assert.ThrowsException<ArgumentException>(() => SettingsBase.GetFromFiles<SettingsBase>(filePath, string.Empty));
        Assert.ThrowsException<ArgumentException>(() => SettingsBase.GetFromFiles<SettingsBase>(filePath, "    "));
    }

    [TestMethod]
    public void GetFromFiles_MissingFile()
    {
        var filePath = Path.Combine(TestContext.DeploymentDirectory, Guid.NewGuid().ToString() + ".json");
        var defaultFilePath = Path.Combine(TestContext.DeploymentDirectory, Guid.NewGuid().ToString() + ".json");
        File.WriteAllText(defaultFilePath, string.Empty);
        Assert.ThrowsException<FileNotFoundException>(() => SettingsBase.GetFromFiles<SettingsBase>(filePath, defaultFilePath));
    }

    [TestMethod]
    public void GetFromFiles_MissingDefaultFile()
    {
        var filePath = Path.Combine(TestContext.DeploymentDirectory, Guid.NewGuid().ToString() + ".json");
        var defaultFilePath = Path.Combine(TestContext.DeploymentDirectory, Guid.NewGuid().ToString() + ".json");
        File.WriteAllText(filePath, string.Empty);
        Assert.ThrowsException<FileNotFoundException>(() => SettingsBase.GetFromFiles<SettingsBase>(filePath, defaultFilePath));
    }

    [TestMethod]
    public void GetFromFiles_Success()
    {
        var filePath = Path.Combine(TestContext.DeploymentDirectory, Guid.NewGuid().ToString() + ".json");
        var defaultFilePath = Path.Combine(TestContext.DeploymentDirectory, Guid.NewGuid().ToString() + ".json");
        File.WriteAllText(filePath, $"{{ \"Timeout\":\"00:00:20\", \"Username\":\"MyUser2\", \"Password\":\"MyPassword2\", \"Language\":\"French\", \"OtherSettings\": {{ \"MySetting1\":789, \"MySetting3\":753 }} }}");
        File.WriteAllText(defaultFilePath, $"{{ \"Timeout\":\"00:00:15\", \"Username\":\"MyUser\", \"Password\":\"MyPassword\", \"Language\":\"German\", \"OtherSettings\": {{ \"MySetting1\":123, \"MySetting2\":456 }}, \"SettingsTypes\":[{{ \"TypeName\":\"System.String\", \"TagName\":\"str\" }}] }}");
        var settings = SettingsBase.GetFromFiles<SettingsBase>(filePath, defaultFilePath);

        Assert.AreEqual(20, settings.Timeout.TotalSeconds);
        Assert.AreEqual("MyUser2", settings.Username);
        Assert.AreEqual("MyPassword2", settings.Password);
        Assert.AreEqual(LanguageOptions.French, settings.Language);
        Assert.AreEqual(789L, settings.OtherSettings["MySetting1"]);
        Assert.AreEqual(456L, settings.OtherSettings["MySetting2"]);
        Assert.AreEqual(753L, settings.OtherSettings["MySetting3"]);
        Assert.AreEqual("System.String", settings.SettingsTypes[0].TypeName);
        Assert.AreEqual("str", settings.SettingsTypes[0].TagName);
    }

    private Assembly CreateFakeAssembly(string name, string location)
    {
        var assembly = Mocks.Create<Assembly>();
        assembly.Setup(x => x.GetName()).Returns(new AssemblyName(name));
        assembly.Setup(x => x.Location).Returns(location);
        return assembly.Object;
    }

    public class TestObject1
    {
        public string StringValue { get; set; }
    }
}
