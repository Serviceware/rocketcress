# Rocketcress
[![Build Status](https://dev.azure.com/serviceware/Rocketcress/_apis/build/status/Serviceware.rocketcress?branchName=main)](https://dev.azure.com/serviceware/Rocketcress/_build/latest?definitionId=1&branchName=main)

Rocketcress is a colection of libraries that help you to easily write Integration- and UI-Tests in C# and MSTest.

#### Contents
- [📚 Library List](#library-list)
- [📖 Explanation of terms](#explanation-of-terms)
- [🐱‍🏍 Getting Started](#getting-started)
  - [➕ Create a new Test Project](#create-test-project)
  - [📌 Write a first test](#write-first-test)
- [🌈 Explanation of Functions](#explanation-of-functions)
  - [🔧 Settings](#settings)
  - [👆 Interacting with the UI](#interact-with-ui)
  - [⏳ Waiter](#waiter)
  - [🔑 TestHelper](#testhelper)
- [🔨 Create UIMaps](#create-uimaps)
  - [🔎 Define location keys](#location-keys)
- [🏅 Dos and Don'ts](#dos-and-donts)
- [🔮 FAQ](#faq)
  - [💡 General](#faq-general)
  - [🌐 Selenium](#faq-selenium)
  - [💻 UIAutomation](#faq-uiautomation)

<br>

# 📚 Library List <a id="library-list"></a>
| Name | Description | Supported frameworks |
|------|-------------|----------------------|
| Rocketcress.Core | Contains core functionality of for all Rocketcress libraries and tests | <span style="white-space:nowrap;">`.NET 5.0` <br> `.NET Framework 4.8`</span> |
| Rocketcress.Core.Windows | Contains core functionality specifically for Windows of for all Rocketcress libraries and tests | <span style="white-space:nowrap;">`.NET Framework 4.8`</span> |
| Rocketcress.Selenium | You can reference this library if you want to write tests using the [Selenium](https://github.com/SeleniumHQ/selenium) framework | <span style="white-space:nowrap;">`.NET 5.0` <br> `.NET Framework 4.8`</span> |
| Rocketcress.SourceGenerators | Contains C# 9.0 Source Generators that are useful to test projects. | <span style="white-space:nowrap;">`.NET 5.0` <br> `.NET Framework 4.8`</span> |
| Rocketcress.UIAutomation | You can reference this library if you want to write tests using the [UIAutomation](https://docs.microsoft.com/en-us/dotnet/framework/ui-automation/ui-automation-overview) framework | <span style="white-space:nowrap;">`.NET 5.0` <br> `.NET Framework 4.8`</span> |

<br>

# 📖 Explanation of terms <a id="explanation-of-terms"></a>
| Term         | Description |
|--------------|-------------|
| UIMap        | A UIMap is a class that wraps the interaction with a specific control or view of an application. It contains sub-controls, methods and properties which are used to easily interact with a specific control or view without knowing how to find it in the UI. |
| Location Key | An object that specifies how a control/element is searched in the UI. The location key is specified by an object of type `By` which is contained in both Selenium and UIAutomation libraries.

<br>

# 🐱‍🏍 Getting Started <a id="getting-started"></a>

## ➕ Create a new Test Project <a id="create-test-project"></a>

Creating a project that uses Rocketcress is fairly simple and can be done in the following few steps:
1. Create a new project using the Project Template `MSTest Test Project (.NET Core)`
   1. If you want to target .NET Framework, use the same template and change the `TargetFramework` of that created csproj to `net48`
2. Add the Rocketcress NuGet packages that you need
   1. It is recommended to always add `Rocketcress.SourceGenerators`
   2. Depending on what test you are writing use the following NuGet packages:
      1. IntegrationTest: `Rocketcress.Core`
      2. Selenium UITest: `Rocketcress.Selenium`
         1. If you need to run tests in Firefox or IE, add the NuGet package(s) for the specific driver(s)
         2. Chrome and Edge are supported out of the box
      3. UIAutomation UITest: `Rocketcress.UIAutomation`
3. Add the following property to the csproj file: `<CopySettings>true</CopySettings>`
4. Create a `settings.json` file somewhere in the project and set the "Build Action" in properties to "C# analyzer additional file" so that the source generator can automatically generate a C# class (`SettingValues`) that you can use to load settings.

## 📌 Write a first test <a id="write-first-test"></a>

Most of the things that needs to be done to initialize a test is already done in base classes that can be used in test classes. The following base classes exist:
| Class Name                                                   | Description |
|--------------------------------------------------------------|-------------|
| Rocketcress.Selenium.SeleniumTestBase                        | Base class for all Selenium tests. |
| Rocketcress.UIAutomation.UIAutomationTestBase                | Base class for all UIAutomation tests. |

The general procedure to add a new test class is the following:
1. Create a new file with the template `Class` to the test project
2. Add the `TestClass` code attribute to the class
3. Inherit from one of the base classes above
4. Add a test method by using the `testm` snippet or adding the `TestMethod` code attribute to a public method

At the start of each test method, the browser/application is already started. The UI can be interacted with by using one of the UIMaps directly.

These are two examples for tests in test classes for Selenium and UIAutomation:
- Selenium:
  ```C#
  // [...]

  [TestClass]
  public class LoginTests : SeleniumTestBase
  {
      [TestMethod]
      public void Selenium_Login_Success()
      {
         var mainView = MainView.Login();
         mainView.Logoff(true);
      }
  }
  ```
- UIAutomation
  ```C#
  // [...]

  [TestClass]
  public class LoginViewTests : UIAutomationTestBase
  {
      [TestMethod]
      public void UIA_Login_Success()
      {
          var mainView = MainView.Login();
          mainView.Logoff();
      }
  }
  ```

<br>

# 🌈 Explanation of Functions <a id="explanation-of-functions"></a>

## Settings 🔧 <a id="settings"></a>
The setting classes in the libraries already contain a lot of properties. These can be read about in the code itself. But there are also properties called `OtherSettings`, `KeyClasses` and `SettingTypes` that are special.

The `OtherSettings` property contains a list of custom settings. These settings can be of any type and the type can be specified by prepending a tag to the property name (e.g. `"[str] MyString": "Hello world!"`).

The tags are defined by the `SettingTypes` property. This property is an array of type definitions that contain two properties:
- `TagName`: The name of the tag that can be used by custom settings
- `TypeName`: The type name that is used when generating the settings class (use the full qualified name of the type)

The `KeyClasses` property is used to structure the settings. A prefix with a name can be specified to group custom settings which names start with the given prefix (e.g. `"TL_": "Translation"` will group all custom settings which names are starting with "TL_").

### Example
```JSON
{
  /* [...] */
  "OtherSettings": {
    "[int] MyId": 710,
    "[str] MyString": "SERVICEDESK",
    "[str] TL_MyTranslation": "Reference No."
  },
  "KeyClasses": {
    "TL_": "Translation"
  },
  "SettingsTypes": [
    {
      "TagName": "str",
      "TypeName": "string"
    },
    {
      "TagName": "int",
      "TypeName": "int"
    }
  ]
}
```
This settings.json will generate the following C# file:
```C#
// [...]
#region Setting Key Classes
[AddKeysClass(typeof(TranslationKeys))]
public static class SettingKeys
{
   public static readonly string MyId = "[int] MyId";
   public static readonly string MyString = "[str] MyString";
}

public static class TranslationKeys
{
   public static readonly string MyTranslation = "[str] TL_MyTranslation";
}
#endregion

#region Setting Classes
public static class SettingValues
{
   // [...]
   public static int MyId 
       => _properties.GetProperty(() => _settings.Get<int>(SettingKeys.MyId));
   public static string MyString 
       => _properties.GetProperty(() => _settings.Get<string>(SettingKeys.MyString));
}

public static class TranslationValues
{
   // [...]
   public static string MyTranslation 
       => _properties.GetProperty(() => _settings.Get<string>(TranslationKeys.MyTranslation));
}
#endregion
```

### Create settings files for different environments
It is possible to add more settings files for different environment. For once the "settings_debug.json" is used when the test is executed with the `DEBUG` configuration. It is also possible to create a settings file for a specific environment by naming the settings file "settings_[MachineName].json" (replacing `[MachineName]` by the Name of the Computer; e.g. "settings_LAP-MASC1.json").

The settings files can be placed anywhere in the project (even in subfolders).

Please remember that you need a `settings.json` with the Build Action "C# analyzer additional file" and a reference to the `Rocketcress.SourceGenerators` NuGet package so that the settings class is generated.

Also the property `<CopySettings>true</CopySettings>` needs to be added to the project, so that the settings files are all copied to the output directory while building the project.

### Overwriting settings in different setting files
It is not needed to copy the whole "settings.json" file for the environment specific settings files (or the debug file). It is possible to just create an empty JSON file and specify only the properties that should be overwritten. Also the tag of custom settings can be emitted.

For example this file will use all settings from the settings file above but overwrites the timeout and `AdminUserId`:
```JSON
{
   "Timeout": "00:05:00",
   "OtherSettings": {
      "AdminUserId": 4711,
   }
}
```
As you can see, also the `KeyClasses` and `SettingsTypes` can be emitted. These properties are only used from the main settings.json.

## 👆 Interacting with the UI <a id="interact-with-ui"></a>
The interaction with the UI is mostly done by the control classes provided in the libraries (Selenium: `WebElement`, UIAutomation: `UITestControl`). For these classes there are different derived classes for specific controls (e.g. WpfTextBox for a TextBox control in WPF) which contains more specialized actions.

All actions are done by calling methods or setting/getting property values. For example, the `Click()` method will click on the control and setting the `Text` property of the `WpfTextBox` will set the text of the TextBox.

There are a lot of actions, which would be to much to explain here. All actions can be seen in the code.

### Views in Selenium library
In Selenium there is another class that can be used to create UIMaps - the `View` class. Classes that derive from `View` should represent a browser page (basically the `body` element of a web page). So it cannot be interacted with directly, but contains all elements that are on that page. A `View` needs to specify a location key which is used to identify if the view is loaded completely. This should be any element that loads last on the page.

Also an important note is, that with the help of the `SetFocus` method, the driver focus can be switched to a specific view. In Selenium this is normally done by calling the driver with a specific window handle. This is done automatically by the `View` base class.


## ⏳ Waiter <a id="waiter"></a>

The `Waiter` is one of the most impotant classes in the libraries. It handles wait actions which are exceptionally important for UI Tests.

The `Waiter` class has one method `WaitUntil` which as a lot of overloads. In general it accepts a `Func<T>` which is the wait condition. The `WaitUntil` method will wait until that function returns a value that does not equal to the `default` value of the given type `T`. That means if `T` is `bool` the method will wait until the function returns `true`. The result of that function is returned by the `WaitUntil` method. If a timeout occurres the method will return the `default` value of type `T` by default.

There are overloads to specific the timeout (default is the one from the provided settings) and the wait between checks (default: 100ms). Also there are overloads that return the duration of the waiting action in an `out` parameter. But one of the most important parameters are the `assert` and `assertMsg` parameters which determine wether a `Assert.Fail` should be executed if the method runs into a timeout. If so, the `assertMsg` is used as the message. If `assertMsg` is not provided a default message is displayed.

For example this call will wait until the element `myElement` does have the text `success` in it. It checks every second and throws an `AssertException` with the message "Text is wrong." if the timeout of 5 minutes is exceeded:
```C#
var myElement = new WebElement(/* [...] */);
Waiter.WaitUntil(() => myElement.Text == "success", TimeSpan.FromMinutes(5), 1000, true, "Text is wrong.");
```

## 🔑 TestHelper <a id="testhelper"></a>

The `TestHelper` class contains a lot of useful methods/properties. Like the following:
- `IsDebugConfiguration`: Returns a value indicating wether the test is executed in debug configuration
- `RetryAction`: Retries a specific action until it returns an expected value; other overloads will retry an action until it will not throw an exception
- `Try`: A shortened version of `try { } catch { }`
- `RunPowerShell`: Runs a PowerShell script and returns the Exit Code, Standard Output and Standard Error
- `LoopUntilAllFinished`: Runs a list of functions in parallel until all have returned a value; will rerun functions that already completed so the result will be the most up to date values of these functions
- `RunWithTimeout`: Runs an action and stops it if the specified timeout is exceeded.

<br>

# 🔨 Create UIMaps <a id="create-uimaps"></a>
Before a UIMap for a control/element is created, the base class which to be use needs to be determined.
- Selemium: Always use the `View` class for web pages and `WebElement` class for elements
- UIAutomation: Determine the properties "FrameworkId" and "ControlType" from the control for which to create the UIMap (use [Inspect](https://docs.microsoft.com/en-us/windows/win32/winauto/inspect-objects) for this) - that will lead to the name of the base class
  - Example: A control with FrameworkId = "Wpf" and ControlType = "Button" should use the class `WpfButton`

### Specify a control on a view
Let's say you have a view and want to add a control to the views UIMap. Start by overriding the method `Initialize` (UIAutomation) or `InitializeControls` (Selenium) from the base class.

After that create a field and a property for the control under the `Initialize`/`InitializeControls` method and initialize the property in that method.

Example (Selenium):
```C#
public MyView() : base() { }
public MyView(WebDriver driver) : base(driver) { }

protected override void InitializeControls()
{
   base.InitializeControls();
   MyControl = new WebElement(ByMyControl);
}

private static readonly By ByMyControl = By.Id("my-fancy-control");
public WebElement MyControl { get; private set; }
```

### Specify a child control on a control
In this case there is a control that has other controls as child controls. In this case the procedure is mostly the same as for views, but `this` should be passed into the child control, to only search for sub controls.

**Important**: If a XPath is specified for child controls, the XPath needs to start with a dot (e.g. `./div`), so the search only happens in the context of the parent control.

Example (Selenium):
```C#
public MyControl(By locationKey) : base(locationKey) { }
public MyControl(IWebElement element) : base(element) { }
public MyControl(By locationKey, ISearchContext searchContext) : base(locationKey, searchContext) { }
protected MyControl() : base() { }

protected override void InitializeControls()
{
   base.InitializeControls();
   MyChildControl = new WebElement(ByMyControl, this);
}

private static readonly By ByMyChildControl = By.XPath("./input[@type='button']");
public WebElement MyChildControl { get; private set; }
```

## 🔎 Define location keys <a id="location-keys"></a>
As described in the [Explanation of terms](#explanation-of-terms), a location key is basically an object that describes where a control/element can be found in the UI.
There are major differences of defining such a location key in Selenium and UIAutomation. But in both libraries a class called `By` is used.

### Selenium
For the Selenium library the native Selenium class `OpenQA.Selenium.By` is used. With this class it is possible to define a location key with the following search criteria:
- **`ClassName`**: Find an element by its CSS class
- **`Id`**: Find an element by its ID
- **`XPath`**: Find an element by a [XPath](https://www.w3schools.com/xml/xpath_syntax.asp)
- `CssSelector`: Find an element using a CSS selector
- `LinkText` / `PartialLinkText`: Find an element by its link text
- `Name`: Find an element by its name
- `TagName`: Find an element by its tag name

The full API documentation for that class can be found [here](https://www.selenium.dev/selenium/docs/api/dotnet/html/T_OpenQA_Selenium_By.htm).

To find the correct properties or XPaths use the browsers buildin developer tools.

### UIAutomation
The native UIAutomation framework uses a very strange and complicated search mechanism. Searching for controls can also be done manually, so a better search engine was implemented in the Rocketcress.UIAutomation library. The search engine uses a location key, like Selenium, in which a variety of search criteria can be added. The class to use here is the `Rocketcress.UIAutomation.By` and the following criteria can be specified:
- **`CPath`**: Find an element by a "[CPath](#cpath)" (a modified version of [XPath](https://www.w3schools.com/xml/xpath_syntax.asp)) - **Highly recommended!**
+ **`AutomationId`**: Find an element by its "AutomationId" property
+ **`ControlType`**: Find an element by its "ControlType" property
+ **`ClassName`**: Find an element by its "ClassName" property
+ **`Name`**: Find an element by its "Name" property
+ `Framework`: Find an element by its "Framework" property
+ `HelpText`: Find an element by its "HelpText" property
+ `ProcessId`: FInd an element by its "ProcessId" property
+ `ItemStatus`: Find an element by its "ItemStatus" property
+ `AccessKey`: Find an element by its "AccessKey" property
+ `AcceleratorKey`: Find an element by its "AcceleratorKey" property
- `ChildOf`: Find an element that is child of an element found by another location key
- `HasChild`: Find an element that has child elements that match a specified location key
- `RelativeTo`: Find an element that is relative (so neighbor) to an element found by another location key
+ `Property`: Find an element with a specific property value
+ `PatternAvailable` / `PatternNotAvailable`: Find an element that has or has not a specific UIAutomation pattern
+ `Condition`: Find an element by a custom condition
- `Scope` / `Descendants` / `Skip` / `Take` / `MaxDepth`: Set the scope of the location key

Unlike Selenium, multiple search criteria can be added to one `By` object by executing the `And[...]` methods like this: `By.ControlType(ControlType.Button).AndAutomationId("MyButton")`<br>
With the methods `FindFirst` and `FindAll` a control can be searched. Though it is recommended to use the class `UITestControl` or any of its derived classes to search for controls.

To find the correct properties use the [Inspect](https://docs.microsoft.com/en-us/windows/win32/winauto/inspect-objects) tool.

### CPath <a id="cpath"></a>
CPath is a syntax for describing a path to a UIAutomation control in the control tree. It is a smaller and modified version of the XPath syntax.
CPath can be easily combined with the normal By-Method-Syntax.

#### General Structure
```
   |[MaxDepth]                [Condition]                           [Child]
  ┌┴┐      ┌───────────────────────┴────────────────────────┐┌─────────┴─────────┐
//{3}Window[@name='ClassicDesk' and ./Edit[@id='ServerName']]/Button[@id='submit']
├┘   └──┬─┘ └┬────────────────┘ └┬┘ └┬─────────────────────┘
|       |    |PropertyCondition  |   |HashChildCondition
|Path   |[ControlType]           |Operator
```

#### Available Path Elements
- Direct child (`/` | `./`) - MaxDepth will be ignored
- Descendant (`//` | `.//`) - MaxDepth determines the maximal descendant depth to search for (Default is 5)
- Parent (`..` | `./..`) - MaxDepth will be ignored
- Ascendant (`...` | `./...`) - MaxDepth determines the maximal ascendant depth to search for (Default is 5)
- Relative (`/<` | `/>` | `/<>` | `/.<` | `/.>` | `/.<>`) - MaxDepth will be ignored<br>
  Searches for a control that is on the same level.
  - `.` include the element in the search
  - `<` include preceding elements in the search
  - `>` include subsequent elements in the search
- Composite Path (`<path1>|<path2>|[...]`) - Combines multiple paths like an or-statement<br>
  E.g.: `..|/` matches either the parent or the direct child;
- Combine Paths without conditions with `*` as control type<br>
  E.g.: `./..*..*//button`: \<Parent> -> \<Parent> -> \<Descendant buttons>
- Max search depth (`{<MaxDepth>}`) - Determines the maximum search depth; can be used only after Descendant or Ascendant

#### Available Control Types
All control types from the class [System.Windows.Automation.ControlType](https://docs.microsoft.com/en-us/dotnet/api/system.windows.automation.controltype?view=netcore-3.1) can be used. Control types are case insensitive and `-` characters are ignored. There are though custom aliases for the current control types:
| Type Name | Aliases           | Description                   |
|-----------|-------------------|-------------------------------|
| Text      | `text`, `label`   | A TextBlock/Label control     |
| Edit      | `edit`, `textbox` | A TextBox/PasswordBox control |
| Tab       | `tab`, `tablist`  | A Tab control                 |

#### Available Condition types
- PropertyCondition (`@property[~=]<value>`)<br>
  Matches an element by one of its properties. All properties defined in [System.Windows.Automation.AutomationElement](https://docs.microsoft.com/en-us/dotnet/api/system.windows.automation.automationelement?view=netcore-3.1) are supported (properties are case insensitive and `-` characters are ignored).<br>
  The following string matching options are available:
  - `=` or `==` : Case sensitive equality
  - `=~` : Case insensitive equality
  - `~=` : Case sensitive contains
  - `~` or `~~` : Case insensitive contains
- HasChildCondition (`<cpath>`)<br>
  Matches an element that has the sub element defined by the given cpath
- Condition Operators: `and` / `or`<br>
  You can also use parentheses to group conditions like `[...] and ([...] or [...])`.<br>
  `and` will bind stronger than `or`, so `[...] or [...] and [...]` is the same as `[...] or ([...] and [...])`.

<br>

# 🏅 Dos and Don'ts <a id="dos-and-donts"></a>

+ Always think about wait actions. Is a wait needed, on what to wait and how long should the timeout be.<br>
  **This is the most common cause of unstable tests if not done correctly!**
+ Use the settings as often as possible, instead of hardcoding information into the test.
+ When creating UIMaps always add the same constructors as their base class.
+ When initializing a control in a UIMap of another control, pass in `this` as the "searchContext" (Selenium) or "parent" (UIAutomation) parameters to the constructor of the child control.
- Try to avoid the usage of `Thread.Sleep` or `Task.Delay`; use the [`Waiter`](#waiter) class instead.
- Never use custom settings in a UIMap project.
- Try to avoid using translated strings in a location key.
- Try to avoid searching for elements in a Test directly; add a UIMap to one of the UIMap libraries instead.
- (Selenium) Never search for elements directly on the driver; create an instance of the `WebElement` class instead.
- (Selenium) Never use Windows specific actions (like sending keys with Windows.Forms), only interact with controls and/or the driver.

<br>

# 🔮 FAQ <a id="faq"></a>

## 💡 General <a id="faq-general"></a>

### Who can I ask if I have any problems with the libraries?
If you are an employee of the Serviceware SE, contact the PANDA Team from PD Processes which already has a lot of experience with these libraries; otherwise create an Issue on Github.

## 🌐 Selenium <a id="faq-selenium"></a>

### How can I access the Selenium driver?
The Driver is available in the following locations:
1. **Inside a test class**: Use the `CurrentDriver` property
2. **On a View of WebElement**: Use the `Driver` property
3. **Anywhere else**: Use the `Driver` property of `SeleniumTestContext.CurrentContext`

### How is determined which browser to use in a Selenium test?
There are multiple ways of telling the Selenium library what browser to use. The library checks the locations in the following order (first wins):
1. When the test is executed via Azure DevOps Pipelines using a Test Plan in which the test is associated to a configuration (checked by getting property "TestConfiguration" of the MSTest TestContext) with the following criteria (first wins):
   1. Contains "chrome" (case insensitive): Google Chrome is used
   2. Contains "firefox" (case insensitive): Mozilla Firefox is used
   3. Contains "ie", "internet explorer" or "internetexplorer" (case insensitive): Microsoft Internet Explorer is used
   4. Contains "edge" (case insensitive): Microsoft Edge (Chromium) is used
2. The test has a "Rocketcress.Selenium.BrowserDataSourceAttribute" code attribute associated.
3. The fallback location is always the property "DefaultBrowser" in the settings.json. If the property if not provided, Chrome is used.

### How can a Selenium test be executed in multiple browsers?
Selenium tests can be executed in multiple browsers with the following options:
1. Add the test to a Test Plan in Azure DevOps Server and set multiple configurations on the test (containing the name of the browser to test). <br>
   This option only works in Azure DevOps Pipelines.
2. Using the `Rocketcress.Selenium.BrowserDataSourceAttribute` code attribute on the test method. <br>
   This option works locally and in Azure DevOps Pipelines. When using Azure DevOps Pipelines, the first option is recommended, because you can then differentiate test results between browsers.

### How can I start another Browser?
A new web driver can be created by executing the `CreateAndSwitchToNewDriver` method from the `SeleniumTestContext`. The current driver can be switched by using the `SwitchCurrentDriver` method or by executing the `SetFocus` of an existing instance of a `View`.

### My tests are failing a lot in Internet Explorer, why?
The IEDriver for Selenium is not the best, so it is really slow. If you run into issues with expiring timeouts in Internet Explorer, try adjusting the timeout. You can also increase the timeout in the tests by either settings the `Timeout` property on the `Settings` or passing in a different timeout value in the `Waiter.WaitUntil`.

You can also check with the `GetBrowser()` method of the driver if the IE is currently used and just increase the timeout then.

Generally it is recommended to not use IE at all.

## 💻 UIAutomation <a id="faq-uiautomation"></a>

None right now. Questions and Answers will be added here if any occur.