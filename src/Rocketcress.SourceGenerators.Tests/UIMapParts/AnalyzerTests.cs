using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rocketcress.Selenium;
using Rocketcress.Selenium.Controls;
using Rocketcress.SourceGenerators.Tests.Runner;
using Rocketcress.SourceGenerators.UIMapParts;
using Rocketcress.UIAutomation.Controls;
using static Rocketcress.SourceGenerators.Tests.UIMapParts.Helper;

namespace Rocketcress.SourceGenerators.Tests.UIMapParts
{
    [TestClass]
    public class AnalyzerTests
    {
        [TestMethod]
        [DataRow(typeof(WebElement), DisplayName = nameof(WebElement))]
        [DataRow(typeof(View), DisplayName = nameof(View))]
        [DataRow(typeof(UITestControl), DisplayName = nameof(UITestControl))]
        public void UIMapPartsClass_Partial_NoDiagnostics(Type baseClass)
        {
            var source = GetNamespaceDeclaration(
                GetControlClassDelcaration("MyControl", baseClass, isPartial: true));

            AnalyzerTestRunner.CompileAndAnalyze(source, new Analyzer())
                .HasNoWarningsOrErrors();
        }

        [TestMethod]
        [DataRow(typeof(WebElement), DisplayName = nameof(WebElement))]
        [DataRow(typeof(View), DisplayName = nameof(View))]
        [DataRow(typeof(UITestControl), DisplayName = nameof(UITestControl))]
        public void UIMapPartsClass_NonPartial_Error(Type baseClass)
        {
            var source = GetNamespaceDeclaration(
                GetControlClassDelcaration("MyControl", baseClass, isPartial: false));

            AnalyzerTestRunner.CompileAndAnalyze(source, new Analyzer())
                .HasError("RCG0101", 1);
        }
    }
}
