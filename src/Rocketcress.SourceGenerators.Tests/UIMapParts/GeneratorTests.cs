extern alias rccore;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
using IdStyle = rccore::Rocketcress.Core.Attributes.IdStyle;

namespace Rocketcress.SourceGenerators.Tests.UIMapParts
{
    public class GeneratorTests<T>
    {
        protected Type BaseType => typeof(T);
        protected Type DriverType => GetDriverType(BaseType);
        protected Type InitLocationKeyType => GetInitLocationKeyType(BaseType);
        protected Type CtorLocationKeyType => GetCtorLocationKeyType(BaseType);
        protected Type ParentControlType => GetParentControlType(BaseType);

        [TestMethod]
        public void UIMapPartsClass_Empty_GenerateConstructors()
        {
            var source = GetNamespaceDeclaration(
                GetControlClassDelcaration("MyControl", BaseType));

            var validator = AnalyzerTestRunner.CompileAndGenerate(source, new Generator())
                .HasNoErrors()
                .HasType("Test.MyControl", out var controlTypeSymbol);
            ValidateDefaultConstructors(validator, controlTypeSymbol, BaseType);
        }

        [TestMethod]
        public void UIMapPartsClass_Empty_GenerateConstructors_Chain()
        {
            var source = GetNamespaceDeclaration(
                GetControlClassDelcaration("MyControl", BaseType),
                GetControlClassDelcaration("MyControl2", "MyControl"));

            var validator = AnalyzerTestRunner.CompileAndGenerate(source, new Generator())
                .HasNoErrors()
                .HasType("Test.MyControl2", out var controlTypeSymbol);
            ValidateDefaultConstructors(validator, controlTypeSymbol, BaseType);
        }

        [TestMethod]
        public void UIMapPartsClass_Empty_GenerateInitUsingMethods()
        {
            var source = GetNamespaceDeclaration(
                GetControlClassDelcaration("MyControl", BaseType));

            var validator = AnalyzerTestRunner.CompileAndGenerate(source, new Generator());
            validator
                .HasNoErrors()
                .HasType("Test.MyControl", out var controlTypeSymbol)
                .ValidateType(controlTypeSymbol, controlType => controlType
                    .HasMethod(out var staticInitUsing, "InitUsing", typeof(Expression<>).MakeGenericType(typeof(Func<>).MakeGenericType(InitLocationKeyType)))
                    .ValidateMethod(staticInitUsing, m => m.IsStatic().HasAccessibility(Access.Private))
                    .HasMethod(out var instanceInitUsing, "InitUsing", GetInstanceInitUsingParameterType(validator.Compilation, controlTypeSymbol, InitLocationKeyType, true))
                    .ValidateMethod(instanceInitUsing, m => m.IsStatic().HasAccessibility(Access.Private)));
        }

        [TestMethod]
        public void UIMapPartsClass_Empty_GenerateInitMethods()
        {
            var source = GetNamespaceDeclaration(
                GetControlClassDelcaration("MyControl", BaseType));

            AnalyzerTestRunner.CompileAndGenerate(source, new Generator())
                .HasNoErrors()
                .HasType("Test.MyControl", out var controlTypeSymbol)
                .ValidateType(controlTypeSymbol, controlType => controlType
                    .HasMethod(out var initControls, "Initialize", Array.Empty<INamedTypeSymbol>())
                    .ValidateMethod(initControls, m => m.IsNotStatic().HasAccessibility(Access.Protected))
                    .HasMethod(out var onInitializing, "OnInitializing", Array.Empty<INamedTypeSymbol>())
                    .ValidateMethod(onInitializing, m => m.IsNotStatic().IsPartial().HasAccessibility(Access.Private))
                    .HasMethod(out var onBaseInitialized, "OnBaseInitialized", Array.Empty<INamedTypeSymbol>())
                    .ValidateMethod(onBaseInitialized, m => m.IsNotStatic().IsPartial().HasAccessibility(Access.Private))
                    .HasMethod(out var onInitialized, "OnInitialized", Array.Empty<INamedTypeSymbol>())
                    .ValidateMethod(onInitialized, m => m.IsNotStatic().IsPartial().HasAccessibility(Access.Private)));
        }

        [TestMethod]
        public void UIMapPartsClass_Empty_NotPartial()
        {
            var driverType = GetDriverType(BaseType);
            var source = GetNamespaceDeclaration(
                GetControlClassDelcaration(
                    "MyControl",
                    BaseType,
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
        public void UIMapPartsClass_WithConstructors_GenerateMissingConstructors()
        {
            var constructors = $@"
public MyControl({DriverType.FullName} driver) : base(driver) {{}}
public MyControl({DriverType.FullName} driver, string test) : base(driver) {{}}";

            var source = GetNamespaceDeclaration(
                GetControlClassDelcaration("MyControl", BaseType, constructors));

            var validator = AnalyzerTestRunner.CompileAndGenerate(source, new Generator())
                .HasNoErrors()
                .HasType("Test.MyControl", out var controlTypeSymbol)
                .ValidateType(controlTypeSymbol, controlType => controlType
                    .HasConstructor(out var ctor1, DriverType)
                    .ValidateMethod(ctor1, c => c.HasAccessibility(Access.Public))
                    .HasConstructor(out var ctor2, DriverType, typeof(string))
                    .ValidateMethod(ctor2, c => c.HasAccessibility(Access.Public)));

            if (BaseType == typeof(WebElement))
            {
                validator.ValidateType(controlTypeSymbol, controlType => controlType
                    .HasMembers(MethodKind.Constructor, 5)
                    .HasConstructor(out var ctor1, typeof(WebDriver), typeof(OpenQA.Selenium.By))
                    .ValidateMethod(ctor1, c => c.HasAccessibility(Access.Public))
                    .HasConstructor(out var ctor2, typeof(WebDriver), typeof(OpenQA.Selenium.By), typeof(OpenQA.Selenium.ISearchContext))
                    .ValidateMethod(ctor2, c => c.HasAccessibility(Access.Public))
                    .HasConstructor(out var ctor3, typeof(WebDriver), typeof(OpenQA.Selenium.IWebElement))
                    .ValidateMethod(ctor3, c => c.HasAccessibility(Access.Public)));
            }
            else if (BaseType == typeof(View))
            {
                validator.ValidateType(controlTypeSymbol, controlType => controlType
                    .HasMembers(MethodKind.Constructor, 2));
            }
            else if (BaseType == typeof(UITestControl))
            {
                validator.ValidateType(controlTypeSymbol, controlType => controlType
                    .HasMembers(MethodKind.Constructor, 7)
                    .HasConstructor(out var ctor1, typeof(Application), typeof(UIAutomation.By))
                    .ValidateMethod(ctor1, c => c.HasAccessibility(Access.Public))
                    .HasConstructor(out var ctor2, typeof(Application), typeof(IUITestControl))
                    .ValidateMethod(ctor2, c => c.HasAccessibility(Access.Public))
                    .HasConstructor(out var ctor3, typeof(Application), typeof(UIAutomation.By), typeof(AutomationElement))
                    .ValidateMethod(ctor3, c => c.HasAccessibility(Access.Public))
                    .HasConstructor(out var ctor4, typeof(Application), typeof(UIAutomation.By), typeof(IUITestControl))
                    .ValidateMethod(ctor4, c => c.HasAccessibility(Access.Public))
                    .HasConstructor(out var ctor5, typeof(Application), typeof(AutomationElement))
                    .ValidateMethod(ctor5, c => c.HasAccessibility(Access.Public)));
            }
        }

        [TestMethod]
        public void UIMapPartsClass_WithConstructors_DisableConstructorGeneration_NoConstructorsGenerated()
        {
            var constructors = $@"
public MyControl({DriverType.FullName} driver) : base(driver) {{}}
public MyControl({DriverType.FullName} driver, string test) : base(driver) {{}}";

            var source = GetNamespaceDeclaration(
                GetControlClassDelcaration("MyControl", BaseType, constructors, "GenerateDefaultConstructors = false"));

            AnalyzerTestRunner.CompileAndGenerate(source, new Generator())
                .HasNoErrors()
                .HasType("Test.MyControl", out var controlTypeSymbol)
                .ValidateType(controlTypeSymbol, controlType => controlType
                    .HasMembers(MethodKind.Constructor, 2)
                    .HasConstructor(out var ctor1, DriverType)
                    .ValidateMethod(ctor1, c => c.HasAccessibility(Access.Public))
                    .HasConstructor(out var ctor2, DriverType, typeof(string))
                    .ValidateMethod(ctor2, c => c.HasAccessibility(Access.Public)));
        }

        [TestMethod]
        public void UIMapControl_WithoutOptions()
        {
            var childControlType = BaseType == typeof(View) ? typeof(WebElement) : BaseType;
            var source = GetNamespaceDeclaration(
                GetControlClassDelcaration(
                    "MyControl",
                    BaseType,
                    GetUIMapControlDeclaration("ChildControl", childControlType)));

            var validator = AnalyzerTestRunner.CompileAndGenerate(source, new Generator());
            validator
                .HasNoErrors()
                .HasType("Test.MyControl", out var controlTypeSymbol)
                .ValidateType(controlTypeSymbol, controlType => controlType
                    .HasMethod(out var onControlInitialized, "OnChildControlInitialized", MethodKind.Ordinary)
                    .ValidateMethod(onControlInitialized, m => m.IsPartial().HasAccessibility(Access.Private))
                    .HasMethod(out var initControls, "Initialize", Array.Empty<INamedTypeSymbol>())
                    .ValidateMethod(initControls, init => init
                        .HasBodyExpression<AssignmentExpressionSyntax>("ChildControlPropertyAssignment", assign => assign
                            .LeftIs<IdentifierNameSyntax>(x => x.HasName("ChildControl"))
                            .RightIs<ObjectCreationExpressionSyntax>(objectCreation => objectCreation
                                .HasArguments(BaseType == typeof(View) ? 2 : 3)
                                .Conditional(BaseType != typeof(View), x => x.HasArgument<ThisExpressionSyntax>(2))
                                .IsSymbol<IMethodSymbol>(right => right
                                    .IsContainedIn(childControlType)
                                    .HasParameters(BaseType == typeof(View) ? new[] { DriverType, CtorLocationKeyType } : new[] { DriverType, CtorLocationKeyType, ParentControlType }))))
                        .HasBodyExpression<InvocationExpressionSyntax>("OnChildControlInitializedCall", invoke => invoke
                            .IsSymbol<IMethodSymbol>(method => method
                                .HasName("OnChildControlInitialized")
                                .HasParameters())))
                    .HasField("ByChildControl", locationKey => locationKey
                        .IsStatic()
                        .HasAccessibility(Access.Private)
                        .HasSyntax<VariableDeclaratorSyntax>(declaration => declaration
                            .HasInitializer(initializer => initializer
                                .HasValue<InvocationExpressionSyntax>(invocation => invocation
                                    .IsSymbol<IMethodSymbol>(x => x.IsContainedIn(InitLocationKeyType).HasName("Id"))
                                    .HasArguments<LiteralExpressionSyntax>(x => x.IsEqualTo("ChildControl")))))));
        }

        [TestMethod]
        [DataRow(nameof(IdStyle.Unset), "ChildControl", DisplayName = nameof(IdStyle.Unset))]
        [DataRow(nameof(IdStyle.None), "ChildControl", DisplayName = nameof(IdStyle.None))]
        [DataRow(nameof(IdStyle.CamelCase), "childControl", DisplayName = nameof(IdStyle.CamelCase))]
        [DataRow(nameof(IdStyle.KebabCase), "child-control", DisplayName = nameof(IdStyle.KebabCase))]
        [DataRow(nameof(IdStyle.PascalCase), "ChildControl", DisplayName = nameof(IdStyle.PascalCase))]
        [DataRow(nameof(IdStyle.UpperCase), "CHILDCONTROL", DisplayName = nameof(IdStyle.UpperCase))]
        [DataRow(nameof(IdStyle.LowerCase), "childcontrol", DisplayName = nameof(IdStyle.LowerCase))]
        public void UIMapControl_WithSpecifiedIdStyle(string idstyleName, string expectedId)
        {
            var childControlType = BaseType == typeof(View) ? typeof(WebElement) : BaseType;
            var source = GetNamespaceDeclaration(
                GetControlClassDelcaration(
                    "MyControl",
                    BaseType,
                    GetUIMapControlDeclaration("ChildControl", childControlType, $"IdStyle = Rocketcress.Core.Attributes.IdStyle.{idstyleName}")));

            var validator = AnalyzerTestRunner.CompileAndGenerate(source, new Generator());
            validator
                .HasNoErrors()
                .HasType("Test.MyControl", out var controlTypeSymbol)
                .ValidateType(controlTypeSymbol, controlType => controlType
                    .HasField("ByChildControl", locationKey => locationKey
                        .HasSyntax<VariableDeclaratorSyntax>(declaration => declaration
                            .HasInitializer(expressionBody => expressionBody
                                .HasValue<InvocationExpressionSyntax>(invocation => invocation
                                    .IsSymbol<IMethodSymbol>(x => x.IsContainedIn(InitLocationKeyType).HasName("Id"))
                                    .HasArguments<LiteralExpressionSyntax>(x => x.IsEqualTo(expectedId)))))));
        }

        [TestMethod]
        public void UIMapControl_WithSpecifiedIdStyle_Disabled()
        {
            var childControlType = BaseType == typeof(View) ? typeof(WebElement) : BaseType;
            var source = GetNamespaceDeclaration(
                GetControlClassDelcaration(
                    "MyControl",
                    BaseType,
                    GetUIMapControlDeclaration("ChildControl", childControlType, $"IdStyle = Rocketcress.Core.Attributes.IdStyle.{nameof(IdStyle.Disabled)}")));

            var validator = AnalyzerTestRunner.CompileAndGenerate(source, new Generator());
            validator
                .HasNoErrors()
                .HasType("Test.MyControl", out var controlTypeSymbol)
                .ValidateType(controlTypeSymbol, controlType => controlType
                    .HasField("ByChildControl", locationKey => locationKey
                        .HasSyntax<VariableDeclaratorSyntax>(declaration => declaration
                            .HasInitializer(expressionBody => expressionBody
                                .Conditional(BaseType == typeof(UITestControl), x => x
                                    .HasValue<MemberAccessExpressionSyntax>(invocation => invocation
                                        .IsSymbol<IPropertySymbol>(x => x.IsContainedIn(InitLocationKeyType).HasName("Empty"))))
                                .Conditional(BaseType != typeof(UITestControl), x => x
                                    .HasValue<InvocationExpressionSyntax>(invocation => invocation
                                        .IsSymbol<IMethodSymbol>(x => x.IsContainedIn(InitLocationKeyType).HasName("XPath"))
                                        .HasArguments<LiteralExpressionSyntax>(x => x.IsEqualTo(".//*"))))))));
        }

        [TestMethod]
        public void UIMapControl_WithSpecifiedId()
        {
            var childControlType = BaseType == typeof(View) ? typeof(WebElement) : BaseType;
            var source = GetNamespaceDeclaration(
                GetControlClassDelcaration(
                    "MyControl",
                    BaseType,
                    GetUIMapControlDeclaration("ChildControl", childControlType, "Id = \"blub\"")));

            var validator = AnalyzerTestRunner.CompileAndGenerate(source, new Generator());
            validator
                .HasNoErrors()
                .HasType("Test.MyControl", out var controlTypeSymbol)
                .ValidateType(controlTypeSymbol, controlType => controlType
                    .HasField("ByChildControl", locationKey => locationKey
                        .HasSyntax<VariableDeclaratorSyntax>(declaration => declaration
                            .HasInitializer(expressionBody => expressionBody
                                .HasValue<InvocationExpressionSyntax>(invocation => invocation
                                    .IsSymbol<IMethodSymbol>(x => x.IsContainedIn(InitLocationKeyType).HasName("Id"))
                                    .HasArguments<LiteralExpressionSyntax>(x => x.IsEqualTo("blub")))))));
        }

        [TestMethod]
        public void UIMapControl_WithStaticLocationKeyInitializer()
        {
            var childControlType = BaseType == typeof(View) ? typeof(WebElement) : BaseType;
            var source = GetNamespaceDeclaration(
                GetControlClassDelcaration(
                    "MyControl",
                    BaseType,
                    GetUIMapControlDeclaration("ChildControl", childControlType, initCode: $" = InitUsing<{childControlType.FullName}>(() => {InitLocationKeyType.FullName}.Id(\"abc\"));")));

            var validator = AnalyzerTestRunner.CompileAndGenerate(source, new Generator());
            validator
                .HasNoErrors()
                .HasType("Test.MyControl", out var controlTypeSymbol)
                .ValidateType(controlTypeSymbol, controlType => controlType
                    .DoesNotHaveProperty("ByChildControl")
                    .HasField("ByChildControl", locationKey => locationKey
                        .IsStatic()
                        .HasAccessibility(Access.Private)
                        .HasSyntax<VariableDeclaratorSyntax>(declaration => declaration
                            .HasInitializer(expressionBody => expressionBody
                                .HasValue<InvocationExpressionSyntax>(invocation => invocation
                                    .IsSymbol<IMethodSymbol>(x => x.IsContainedIn(InitLocationKeyType).HasName("Id"))
                                    .HasArguments<LiteralExpressionSyntax>(x => x.IsEqualTo("abc")))))));
        }

        [TestMethod]
        public void UIMapControl_WithInstanceLocationKeyInitializer()
        {
            var childControlType = BaseType == typeof(View) ? typeof(WebElement) : BaseType;
            var source = GetNamespaceDeclaration(
                GetControlClassDelcaration(
                    "MyControl",
                    BaseType,
                    GetUIMapControlDeclaration("ChildControl", childControlType, initCode: $" = InitUsing<{childControlType.FullName}>(x => {InitLocationKeyType.FullName}.Id(\"abc\"));")));

            var validator = AnalyzerTestRunner.CompileAndGenerate(source, new Generator());
            validator
                .HasNoErrors()
                .HasType("Test.MyControl", out var controlTypeSymbol)
                .ValidateType(controlTypeSymbol, controlType => controlType
                    .DoesNotHaveField("ByChildControl")
                    .HasProperty("ByChildControl", locationKey => locationKey
                        .IsNotStatic()
                        .HasAccessibility(Access.Private)
                        .HasSyntax<PropertyDeclarationSyntax>(declaration => declaration
                            .HasExpressionBody(expressionBody => expressionBody
                                .HasExpression<InvocationExpressionSyntax>(invocation => invocation
                                    .IsSymbol<IMethodSymbol>(x => x.IsContainedIn(GetInstanceInitUsingParameterType(validator.Compilation, controlTypeSymbol, InitLocationKeyType, false)).HasName("Invoke"))
                                    .HasArguments<ThisExpressionSyntax>())))));
        }

        private static INamedTypeSymbol GetInstanceInitUsingParameterType(Compilation compilation, INamedTypeSymbol controlTypeSymbol, Type locationKeyType, bool asExpression)
        {
            var funcTypeSymbol = compilation.GetTypeByMetadataName("System.Func`2")!;
            var byTypeSymbol = compilation.GetTypeByMetadataName(locationKeyType.FullName!)!;

            var construcedFuncSymbol = funcTypeSymbol.Construct(controlTypeSymbol, byTypeSymbol);
            if (!asExpression)
                return construcedFuncSymbol;

            var expressionTypeSymbol = compilation.GetTypeByMetadataName("System.Linq.Expressions.Expression`1")!;
            return expressionTypeSymbol.Construct(construcedFuncSymbol);
        }

        private static void ValidateDefaultConstructors(CompilationValidator validator, INamedTypeSymbol controlTypeSymbol, Type baseClass)
        {
            if (baseClass == typeof(WebElement))
            {
                validator.ValidateType(controlTypeSymbol, controlType => controlType
                    .HasMembers(MethodKind.Constructor, 4)
                    .HasConstructor(out var ctor1, typeof(WebDriver), typeof(OpenQA.Selenium.By))
                    .ValidateMethod(ctor1, c => c.HasAccessibility(Access.Public))
                    .HasConstructor(out var ctor2, typeof(WebDriver), typeof(OpenQA.Selenium.By), typeof(OpenQA.Selenium.ISearchContext))
                    .ValidateMethod(ctor2, c => c.HasAccessibility(Access.Public))
                    .HasConstructor(out var ctor3, typeof(WebDriver), typeof(OpenQA.Selenium.IWebElement))
                    .ValidateMethod(ctor3, c => c.HasAccessibility(Access.Public))
                    .HasConstructor(out var ctor4, typeof(WebDriver))
                    .ValidateMethod(ctor4, c => c.HasAccessibility(Access.Protected)));
            }
            else if (baseClass == typeof(View))
            {
                validator.ValidateType(controlTypeSymbol, controlType => controlType
                    .HasMembers(MethodKind.Constructor, 1)
                    .HasConstructor(out var ctor4, typeof(WebDriver))
                    .ValidateMethod(ctor4, c => c.HasAccessibility(Access.Public)));
            }
            else if (baseClass == typeof(UITestControl))
            {
                validator.ValidateType(controlTypeSymbol, controlType => controlType
                    .HasMembers(MethodKind.Constructor, 6)
                    .HasConstructor(out var ctor1, typeof(Application), typeof(UIAutomation.By))
                    .ValidateMethod(ctor1, c => c.HasAccessibility(Access.Public))
                    .HasConstructor(out var ctor2, typeof(Application), typeof(IUITestControl))
                    .ValidateMethod(ctor2, c => c.HasAccessibility(Access.Public))
                    .HasConstructor(out var ctor3, typeof(Application), typeof(UIAutomation.By), typeof(AutomationElement))
                    .ValidateMethod(ctor3, c => c.HasAccessibility(Access.Public))
                    .HasConstructor(out var ctor4, typeof(Application), typeof(UIAutomation.By), typeof(IUITestControl))
                    .ValidateMethod(ctor4, c => c.HasAccessibility(Access.Public))
                    .HasConstructor(out var ctor5, typeof(Application), typeof(AutomationElement))
                    .ValidateMethod(ctor5, c => c.HasAccessibility(Access.Public))
                    .HasConstructor(out var ctor6, typeof(Application))
                    .ValidateMethod(ctor6, c => c.HasAccessibility(Access.Protected)));
            }
        }
    }

    [TestClass]
    public class WebElementGeneratorTests : GeneratorTests<WebElement>
    {
    }

    [TestClass]
    public class ViewGeneratorTests : GeneratorTests<View>
    {
    }

    [TestClass]
    public class UITestControlGeneratorTests : GeneratorTests<UITestControl>
    {
    }
}