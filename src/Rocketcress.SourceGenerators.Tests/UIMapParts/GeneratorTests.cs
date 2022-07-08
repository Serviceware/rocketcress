using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rocketcress.Selenium;
using Rocketcress.Selenium.Controls;
using Rocketcress.SourceGenerators.Tests.Runner;
using Rocketcress.SourceGenerators.Tests.Validators;
using Rocketcress.SourceGenerators.UIMapParts;
using Rocketcress.UIAutomation;
using Rocketcress.UIAutomation.Controls;
using System.Linq.Expressions;
using System.Windows.Automation;
using static Rocketcress.SourceGenerators.Tests.UIMapParts.Helper;

namespace Rocketcress.SourceGenerators.Tests.UIMapParts
{
    [TestClass]
    public class GeneratorTests
    {
        [TestMethod]
        [DataRow(typeof(WebElement), DisplayName = nameof(WebElement))]
        [DataRow(typeof(View), DisplayName = nameof(View))]
        [DataRow(typeof(UITestControl), DisplayName = nameof(UITestControl))]
        public void EmptyControl_GenerateConstructors(Type baseClass)
        {
            var source = GetNamespaceDeclaration(
                GetControlClassDelcaration("MyControl", baseClass));

            var validator = AnalyzerTestRunner.CompileAndGenerate(source, new Generator())
                .HasNoErrors()
                .HasType("Test.MyControl", out var controlTypeSymbol);
            ValidateDefaultConstructors(validator, controlTypeSymbol, baseClass);
        }

        [TestMethod]
        [DataRow(typeof(WebElement), DisplayName = nameof(WebElement))]
        [DataRow(typeof(View), DisplayName = nameof(View))]
        [DataRow(typeof(UITestControl), DisplayName = nameof(UITestControl))]
        public void EmptyControl_GenerateConstructors_Chain(Type baseClass)
        {
            var source = GetNamespaceDeclaration(
                GetControlClassDelcaration("MyControl", baseClass),
                GetControlClassDelcaration("MyControl2", "MyControl"));

            var validator = AnalyzerTestRunner.CompileAndGenerate(source, new Generator())
                .HasNoErrors()
                .HasType("Test.MyControl2", out var controlTypeSymbol);
            ValidateDefaultConstructors(validator, controlTypeSymbol, baseClass);
        }

        [TestMethod]
        [DataRow(typeof(WebElement), DisplayName = nameof(WebElement))]
        [DataRow(typeof(View), DisplayName = nameof(View))]
        [DataRow(typeof(UITestControl), DisplayName = nameof(UITestControl))]
        public void EmptyControl_GenerateInitUsingMethods(Type baseClass)
        {
            var source = GetNamespaceDeclaration(
                GetControlClassDelcaration("MyControl", baseClass));

            var locationKeyType = GetLocationKeyType(baseClass);
            var validator = AnalyzerTestRunner.CompileAndGenerate(source, new Generator());
            validator
                .HasNoErrors()
                .HasType("Test.MyControl", out var controlTypeSymbol)
                .ValidateType(controlTypeSymbol, controlType => controlType
                    .HasMethod(out var staticInitUsing, "InitUsing", typeof(Expression<>).MakeGenericType(typeof(Func<>).MakeGenericType(locationKeyType)))
                    .ValidateMethod(staticInitUsing, m => m.IsStatic().HasAccessibility(Accessibility.Private))
                    .HasMethod(out var instanceInitUsing, "InitUsing", GetInstanceInitUsingParameterType(validator.Compilation, controlTypeSymbol, locationKeyType))
                    .ValidateMethod(instanceInitUsing, m => m.IsStatic().HasAccessibility(Accessibility.Private)));
        }

        [TestMethod]
        [DataRow(typeof(WebElement), DisplayName = nameof(WebElement))]
        [DataRow(typeof(View), DisplayName = nameof(View))]
        [DataRow(typeof(UITestControl), DisplayName = nameof(UITestControl))]
        public void EmptyControl_GenerateInitMethods(Type baseClass)
        {
            var source = GetNamespaceDeclaration(
                GetControlClassDelcaration("MyControl", baseClass));

            AnalyzerTestRunner.CompileAndGenerate(source, new Generator())
                .HasNoErrors()
                .HasType("Test.MyControl", out var controlTypeSymbol)
                .ValidateType(controlTypeSymbol, controlType => controlType
                    .HasMethod(out var initControls, "Initialize", Array.Empty<INamedTypeSymbol>())
                    .ValidateMethod(initControls, m => m.IsNotStatic().HasAccessibility(Accessibility.Protected))
                    .HasMethod(out var onInitializing, "OnInitializing", Array.Empty<INamedTypeSymbol>())
                    .ValidateMethod(onInitializing, m => m.IsNotStatic().IsPartial().HasAccessibility(Accessibility.Private))
                    .HasMethod(out var onBaseInitialized, "OnBaseInitialized", Array.Empty<INamedTypeSymbol>())
                    .ValidateMethod(onBaseInitialized, m => m.IsNotStatic().IsPartial().HasAccessibility(Accessibility.Private))
                    .HasMethod(out var onInitialized, "OnInitialized", Array.Empty<INamedTypeSymbol>())
                    .ValidateMethod(onInitialized, m => m.IsNotStatic().IsPartial().HasAccessibility(Accessibility.Private)));
        }

        [TestMethod]
        [DataRow(typeof(WebElement), DisplayName = nameof(WebElement))]
        [DataRow(typeof(View), DisplayName = nameof(View))]
        [DataRow(typeof(UITestControl), DisplayName = nameof(UITestControl))]
        public void EmptyControl_NotPartial(Type baseClass)
        {
            var driverType = GetDriverType(baseClass);
            var source = GetNamespaceDeclaration(
                GetControlClassDelcaration(
                    "MyControl",
                    baseClass,
                    $"public MyControl({driverType.FullName} driver) : base(driver) {{}}",
                    isPartial: false));

            AnalyzerTestRunner.CompileAndGenerate(source, new Generator())
                .HasNoErrors()
                .HasType("Test.MyControl", out var controlTypeSymbol)
                .ValidateType(controlTypeSymbol, controlType => controlType
                    .DoesNotHaveMethod("Initialize")
                    .DoesNotHaveMethod("InitUsing")
                    .HasMembers(MethodKind.Constructor, 1));
        }

        [TestMethod]
        [DataRow(typeof(WebElement), DisplayName = nameof(WebElement))]
        [DataRow(typeof(View), DisplayName = nameof(View))]
        [DataRow(typeof(UITestControl), DisplayName = nameof(UITestControl))]
        public void WebElement_WithConstructors_GenerateMissingConstructors(Type baseClass)
        {
            var driverType = GetDriverType(baseClass);

            var constructors = $@"
public MyControl({driverType.FullName} driver) : base(driver) {{}}
public MyControl({driverType.FullName} driver, string test) : base(driver) {{}}";

            var source = GetNamespaceDeclaration(
                GetControlClassDelcaration("MyControl", baseClass, constructors));

            var validator = AnalyzerTestRunner.CompileAndGenerate(source, new Generator())
                .HasNoErrors()
                .HasType("Test.MyControl", out var controlTypeSymbol)
                .ValidateType(controlTypeSymbol, controlType => controlType
                    .HasConstructor(out var ctor1, driverType)
                    .ValidateMethod(ctor1, c => c.HasAccessibility(Accessibility.Public))
                    .HasConstructor(out var ctor2, driverType, typeof(string))
                    .ValidateMethod(ctor2, c => c.HasAccessibility(Accessibility.Public)));

            if (baseClass == typeof(WebElement))
            {
                validator.ValidateType(controlTypeSymbol, controlType => controlType
                    .HasMembers(MethodKind.Constructor, 5)
                    .HasConstructor(out var ctor1, typeof(WebDriver), typeof(OpenQA.Selenium.By))
                    .ValidateMethod(ctor1, c => c.HasAccessibility(Accessibility.Public))
                    .HasConstructor(out var ctor2, typeof(WebDriver), typeof(OpenQA.Selenium.By), typeof(OpenQA.Selenium.ISearchContext))
                    .ValidateMethod(ctor2, c => c.HasAccessibility(Accessibility.Public))
                    .HasConstructor(out var ctor3, typeof(WebDriver), typeof(OpenQA.Selenium.IWebElement))
                    .ValidateMethod(ctor3, c => c.HasAccessibility(Accessibility.Public)));
            }
            else if (baseClass == typeof(View))
            {
                validator.ValidateType(controlTypeSymbol, controlType => controlType
                    .HasMembers(MethodKind.Constructor, 2));
            }
            else if (baseClass == typeof(UITestControl))
            {
                validator.ValidateType(controlTypeSymbol, controlType => controlType
                    .HasMembers(MethodKind.Constructor, 7)
                    .HasConstructor(out var ctor1, typeof(Application), typeof(UIAutomation.By))
                    .ValidateMethod(ctor1, c => c.HasAccessibility(Accessibility.Public))
                    .HasConstructor(out var ctor2, typeof(Application), typeof(IUITestControl))
                    .ValidateMethod(ctor2, c => c.HasAccessibility(Accessibility.Public))
                    .HasConstructor(out var ctor3, typeof(Application), typeof(UIAutomation.By), typeof(AutomationElement))
                    .ValidateMethod(ctor3, c => c.HasAccessibility(Accessibility.Public))
                    .HasConstructor(out var ctor4, typeof(Application), typeof(UIAutomation.By), typeof(IUITestControl))
                    .ValidateMethod(ctor4, c => c.HasAccessibility(Accessibility.Public))
                    .HasConstructor(out var ctor5, typeof(Application), typeof(AutomationElement))
                    .ValidateMethod(ctor5, c => c.HasAccessibility(Accessibility.Public)));
            }
        }

        [TestMethod]
        [DataRow(typeof(WebElement), DisplayName = nameof(WebElement))]
        [DataRow(typeof(View), DisplayName = nameof(View))]
        [DataRow(typeof(UITestControl), DisplayName = nameof(UITestControl))]
        public void WebElement_WithConstructors_DisableConstructorGeneration_NoConstructorsGenerated(Type baseClass)
        {
            var driverType = GetDriverType(baseClass);

            var constructors = $@"
public MyControl({driverType.FullName} driver) : base(driver) {{}}
public MyControl({driverType.FullName} driver, string test) : base(driver) {{}}";

            var source = GetNamespaceDeclaration(
                GetControlClassDelcaration("MyControl", baseClass, constructors, "GenerateDefaultConstructors = false"));

            AnalyzerTestRunner.CompileAndGenerate(source, new Generator())
                .HasNoErrors()
                .HasType("Test.MyControl", out var controlTypeSymbol)
                .ValidateType(controlTypeSymbol, controlType => controlType
                    .HasMembers(MethodKind.Constructor, 2)
                    .HasConstructor(out var ctor1, driverType)
                    .ValidateMethod(ctor1, c => c.HasAccessibility(Accessibility.Public))
                    .HasConstructor(out var ctor2, driverType, typeof(string))
                    .ValidateMethod(ctor2, c => c.HasAccessibility(Accessibility.Public)));
        }

        private static INamedTypeSymbol GetInstanceInitUsingParameterType(Compilation compilation, INamedTypeSymbol controlTypeSymbol, Type locationKeyType)
        {
            var expressionTypeSymbol = compilation.GetTypeByMetadataName("System.Linq.Expressions.Expression`1")!;
            var funcTypeSymbol = compilation.GetTypeByMetadataName("System.Func`2")!;
            var byTypeSymbol = compilation.GetTypeByMetadataName(locationKeyType.FullName!)!;

            var construcedFuncSymbol = funcTypeSymbol.Construct(controlTypeSymbol, byTypeSymbol);
            return expressionTypeSymbol.Construct(construcedFuncSymbol);
        }

        private static void ValidateDefaultConstructors(CompilationValidator validator, INamedTypeSymbol controlTypeSymbol, Type baseClass)
        {
            if (baseClass == typeof(WebElement))
            {
                validator.ValidateType(controlTypeSymbol, controlType => controlType
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
            else if (baseClass == typeof(View))
            {
                validator.ValidateType(controlTypeSymbol, controlType => controlType
                    .HasMembers(MethodKind.Constructor, 1)
                    .HasConstructor(out var ctor4, typeof(WebDriver))
                    .ValidateMethod(ctor4, c => c.HasAccessibility(Accessibility.Public)));
            }
            else if (baseClass == typeof(UITestControl))
            {
                validator.ValidateType(controlTypeSymbol, controlType => controlType
                    .HasMembers(MethodKind.Constructor, 6)
                    .HasConstructor(out var ctor1, typeof(Application), typeof(UIAutomation.By))
                    .ValidateMethod(ctor1, c => c.HasAccessibility(Accessibility.Public))
                    .HasConstructor(out var ctor2, typeof(Application), typeof(IUITestControl))
                    .ValidateMethod(ctor2, c => c.HasAccessibility(Accessibility.Public))
                    .HasConstructor(out var ctor3, typeof(Application), typeof(UIAutomation.By), typeof(AutomationElement))
                    .ValidateMethod(ctor3, c => c.HasAccessibility(Accessibility.Public))
                    .HasConstructor(out var ctor4, typeof(Application), typeof(UIAutomation.By), typeof(IUITestControl))
                    .ValidateMethod(ctor4, c => c.HasAccessibility(Accessibility.Public))
                    .HasConstructor(out var ctor5, typeof(Application), typeof(AutomationElement))
                    .ValidateMethod(ctor5, c => c.HasAccessibility(Accessibility.Public))
                    .HasConstructor(out var ctor6, typeof(Application))
                    .ValidateMethod(ctor6, c => c.HasAccessibility(Accessibility.Protected)));
            }
        }
    }
}