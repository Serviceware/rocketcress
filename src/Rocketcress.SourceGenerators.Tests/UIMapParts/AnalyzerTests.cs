using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rocketcress.Selenium;
using Rocketcress.Selenium.Controls;
using Rocketcress.SourceGenerators.Tests.Runner;
using Rocketcress.SourceGenerators.UIMapParts;
using Rocketcress.UIAutomation.Controls;
using static Rocketcress.SourceGenerators.Tests.UIMapParts.Helper;

namespace Rocketcress.SourceGenerators.Tests.UIMapParts
{
    public class AnalyzerTests<T>
    {
        protected Type BaseType => typeof(T);

        [TestMethod]
        public void UIMapPartsClass_Partial_NoDiagnostics()
        {
            var source = GetNamespaceDeclaration(
                GetControlClassDelcaration("MyControl", BaseType, isPartial: true));

            AnalyzerTestRunner.CompileAndAnalyze(source, new Analyzer())
                .HasNoWarningsOrErrors();
        }

        [TestMethod]
        public void UIMapPartsClass_NonPartial_Error()
        {
            var source = GetNamespaceDeclaration(
                GetControlClassDelcaration("MyControl", BaseType, isPartial: false));

            AnalyzerTestRunner.CompileAndAnalyze(source, new Analyzer())
                .HasError("RCG0101", 1);
        }

        [TestMethod]
        public void UIMapPartsClass_ParentClassNotPartial_Error()
        {
            var source = GetNamespaceDeclaration(
                $@"public class C1 {{ public partial class C2 {{ {GetControlClassDelcaration("MyControl", BaseType)} }} }}");

            AnalyzerTestRunner.CompileAndAnalyze(source, new Analyzer())
                .HasError("RCG0102", 1);
        }

        [TestMethod]
        public void UIMapPartsClass_ParentClassPartial_NoDiagnostics()
        {
            var source = GetNamespaceDeclaration(
                $@"public partial class C1 {{ public partial class C2 {{ {GetControlClassDelcaration("MyControl", BaseType)} }} }}");

            AnalyzerTestRunner.CompileAndAnalyze(source, new Analyzer())
                .HasNoWarningsOrErrors();
        }

        [TestMethod]
        public void UIMapControl_MissingParentControl_Error()
        {
            var source = GetNamespaceDeclaration(
                GetControlClassDelcaration(
                    "MyControl",
                    BaseType,
                    GetUIMapControlDeclaration("ChildControl", BaseType, "ParentControl = \"ParentControl\"")));

            AnalyzerTestRunner.CompileAndAnalyze(source, new Analyzer())
                .HasError("RCG0104", 1);
        }

        [TestMethod]
        public void UIMapControl_ParentControlExists_NoDiagnostics()
        {
            var source = GetNamespaceDeclaration(
                GetControlClassDelcaration(
                    "MyControl",
                    BaseType,
                    GetClassContent(
                        $"public {BaseType.FullName} ParentControl {{ get; }}",
                        GetUIMapControlDeclaration("ChildControl", BaseType, "ParentControl = \"ParentControl\""))));

            AnalyzerTestRunner.CompileAndAnalyze(source, new Analyzer())
                .HasNoWarningsOrErrors();
        }

        [TestMethod]
        public void UIMapControl_ParentLoop_Error()
        {
            var source = GetNamespaceDeclaration(
                GetControlClassDelcaration(
                    "MyControl",
                    BaseType,
                    GetClassContent(
                        GetUIMapControlDeclaration("Control1", BaseType, "ParentControl = \"Control2\""),
                        GetUIMapControlDeclaration("Control2", BaseType, "ParentControl = \"Control3\""),
                        GetUIMapControlDeclaration("Control3", BaseType, "ParentControl = \"Control1\""))));

            AnalyzerTestRunner.CompileAndAnalyze(source, new Analyzer())
                .HasError("RCG0105", 1);
        }
    }

    [TestClass]
    public class AnalyzerTests
    {
        [TestMethod]
        public void UIMapPartsClass_WrongBaseType_Error()
        {
            var source = GetNamespaceDeclaration(
                GetControlClassDelcaration("MyControl", typeof(object)));

            AnalyzerTestRunner.CompileAndAnalyze(source, new Analyzer())
                .HasError("RCG0103", 1);
        }
    }

    [TestClass]
    public class WebElementAnalyzerTests : AnalyzerTests<WebElement>
    {
    }

    [TestClass]
    public class ViewAnalyzerTests : AnalyzerTests<View>
    {
    }

    [TestClass]
    public class UITestControlAnalyzerTests : AnalyzerTests<UITestControl>
    {
    }
}
