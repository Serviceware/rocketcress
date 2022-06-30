using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rocketcress.Selenium;
using Rocketcress.SourceGenerators.Tests.Runner;
using System.Linq.Expressions;

namespace Rocketcress.SourceGenerators.Tests
{
    [TestClass]
    public class UIMapPartsGeneratorSeleniumTests
    {
        [TestMethod]
        public void EmptyWebElement_GenerateConstructors()
        {
            var source = @"
using Rocketcress.Selenium.Controls;
using Rocketcress.Core.Attributes;
namespace Test
{
    [GenerateUIMapParts]
    public partial class MyControl : WebElement {}
}";

            SourceGeneratorTestRunner.CompileAndGenerate(source, new UIMapPartsGenerator())
                .HasNoErrors()
                .HasType("Test.MyControl", out var controlTypeSymbol)
                .ValidateType(controlTypeSymbol, controlType => controlType
                    .HasMembers(MethodKind.Constructor, 4)
                    .HasConstructor(out var ctor1, typeof(WebDriver), typeof(OpenQA.Selenium.By))
                    .ValidateMethod(ctor1, c => c.HasAccessibility(Accessibility.Public))
                    .HasConstructor(out var ctor2, typeof(WebDriver), typeof(OpenQA.Selenium.By), typeof(OpenQA.Selenium.ISearchContext))
                    .ValidateMethod(ctor2, c => c.HasAccessibility(Accessibility.Public))
                    .HasConstructor(out var ctor3, typeof(WebDriver), typeof(OpenQA.Selenium.IWebElement))
                    .ValidateMethod(ctor3, c => c.HasAccessibility(Accessibility.Public))
                    .HasConstructor(out var ctor4, typeof(WebDriver))
                    .ValidateMethod(ctor4, c => c.HasAccessibility(Accessibility.Protected)));
        }

        [TestMethod]
        public void EmptyWebElement_GenerateInitUsingMethods()
        {
            var source = @"
using Rocketcress.Selenium.Controls;
using Rocketcress.Core.Attributes;
namespace Test
{
    [GenerateUIMapParts]
    public partial class MyControl : WebElement {}
}";

            var compilation = SourceGeneratorTestRunner.CompileAndGenerate(source, new UIMapPartsGenerator());
            compilation
                .HasNoErrors()
                .HasType("Test.MyControl", out var controlTypeSymbol)
                .ValidateType(controlTypeSymbol, controlType => controlType
                    .HasMethod(out var staticInitUsing, "InitUsing", typeof(Expression<Func<By>>))
                    .ValidateMethod(staticInitUsing, m => m.IsStatic().HasAccessibility(Accessibility.Private))
                    .HasMethod(out var instanceInitUsing, "InitUsing", GetInstanceInitUsingParameterType(compilation.Compilation, controlTypeSymbol))
                    .ValidateMethod(instanceInitUsing, m => m.IsStatic().HasAccessibility(Accessibility.Private)));
        }

        [TestMethod]
        public void EmptyWebElement_GenerateInitMethods()
        {
            var source = @"
using Rocketcress.Selenium.Controls;
using Rocketcress.Core.Attributes;
namespace Test
{
    [GenerateUIMapParts]
    public partial class MyControl : WebElement {}
}";

            SourceGeneratorTestRunner.CompileAndGenerate(source, new UIMapPartsGenerator())
                .HasNoErrors()
                .HasType("Test.MyControl", out var controlTypeSymbol)
                .ValidateType(controlTypeSymbol, controlType => controlType
                    .HasMethod(out var initControls, "InitializeControls", Array.Empty<INamedTypeSymbol>())
                    .ValidateMethod(initControls, m => m.IsNotStatic().HasAccessibility(Accessibility.Protected))
                    .HasMethod(out var onInitializing, "OnInitializing", Array.Empty<INamedTypeSymbol>())
                    .ValidateMethod(onInitializing, m => m.IsNotStatic().IsPartial().HasAccessibility(Accessibility.Private))
                    .HasMethod(out var onBaseInitialized, "OnBaseInitialized", Array.Empty<INamedTypeSymbol>())
                    .ValidateMethod(onBaseInitialized, m => m.IsNotStatic().IsPartial().HasAccessibility(Accessibility.Private))
                    .HasMethod(out var onInitialized, "OnInitialized", Array.Empty<INamedTypeSymbol>())
                    .ValidateMethod(onInitialized, m => m.IsNotStatic().IsPartial().HasAccessibility(Accessibility.Private)));
        }

        [TestMethod]
        public void WebElement_WithConstructors_GenerateMissingConstructors()
        {
            var source = @"
using Rocketcress.Selenium.Controls;
using Rocketcress.Core.Attributes;
namespace Test
{
    [GenerateUIMapParts]
    public partial class MyControl : WebElement
    {
        public MyControl(Rocketcress.Selenium.WebDriver driver) : base(driver) {}
        public MyControl(Rocketcress.Selenium.WebDriver driver, string test) : base(driver) {}
    }
}";

            SourceGeneratorTestRunner.CompileAndGenerate(source, new UIMapPartsGenerator())
                .HasNoErrors()
                .HasType("Test.MyControl", out var controlTypeSymbol)
                .ValidateType(controlTypeSymbol, controlType => controlType
                    .HasMembers(MethodKind.Constructor, 5)
                    .HasConstructor(out var ctor1, typeof(WebDriver), typeof(OpenQA.Selenium.By))
                    .ValidateMethod(ctor1, c => c.HasAccessibility(Accessibility.Public))
                    .HasConstructor(out var ctor2, typeof(WebDriver), typeof(OpenQA.Selenium.By), typeof(OpenQA.Selenium.ISearchContext))
                    .ValidateMethod(ctor2, c => c.HasAccessibility(Accessibility.Public))
                    .HasConstructor(out var ctor3, typeof(WebDriver), typeof(OpenQA.Selenium.IWebElement))
                    .ValidateMethod(ctor3, c => c.HasAccessibility(Accessibility.Public))
                    .HasConstructor(out var ctor4, typeof(WebDriver))
                    .ValidateMethod(ctor4, c => c.HasAccessibility(Accessibility.Public))
                    .HasConstructor(out var ctor5, typeof(WebDriver), typeof(string))
                    .ValidateMethod(ctor5, c => c.HasAccessibility(Accessibility.Public)));
        }

        [TestMethod]
        public void WebElement_WithConstructors_DisableConstructorGeneration_NoConstructorsGenerated()
        {
            var source = @"
using Rocketcress.Selenium.Controls;
using Rocketcress.Core.Attributes;
namespace Test
{
    [GenerateUIMapParts(GenerateDefaultConstructors = false)]
    public partial class MyControl : WebElement
    {
        public MyControl(Rocketcress.Selenium.WebDriver driver) : base(driver) {}
        public MyControl(Rocketcress.Selenium.WebDriver driver, string test) : base(driver) {}
    }
}";

            SourceGeneratorTestRunner.CompileAndGenerate(source, new UIMapPartsGenerator())
                .HasNoErrors()
                .HasType("Test.MyControl", out var controlTypeSymbol)
                .ValidateType(controlTypeSymbol, controlType => controlType
                    .HasMembers(MethodKind.Constructor, 2)
                    .HasConstructor(out var ctor1, typeof(WebDriver))
                    .ValidateMethod(ctor1, c => c.HasAccessibility(Accessibility.Public))
                    .HasConstructor(out var ctor2, typeof(WebDriver), typeof(string))
                    .ValidateMethod(ctor2, c => c.HasAccessibility(Accessibility.Public)));
        }

        private INamedTypeSymbol GetInstanceInitUsingParameterType(Compilation compilation, INamedTypeSymbol controlTypeSymbol)
        {
            var expressionTypeSymbol = compilation.GetTypeByMetadataName("System.Linq.Expressions.Expression`1")!;
            var funcTypeSymbol = compilation.GetTypeByMetadataName("System.Func`2")!;
            var byTypeSymbol = compilation.GetTypeByMetadataName("Rocketcress.Selenium.By")!;

            var construcedFuncSymbol = funcTypeSymbol.Construct(controlTypeSymbol, byTypeSymbol);
            return expressionTypeSymbol.Construct(construcedFuncSymbol);
        }
    }
}