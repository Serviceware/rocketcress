using Rocketcress.Selenium;
using Rocketcress.Selenium.Controls;
using Rocketcress.UIAutomation;
using Rocketcress.UIAutomation.Controls;

namespace Rocketcress.SourceGenerators.Tests.UIMapParts
{
    public static class Helper
    {
        public static string GetNamespaceDeclaration(params string[] classDeclarations)
        {
            return $@"namespace Test
{{
    {string.Join(Environment.NewLine + Environment.NewLine, classDeclarations)}
}}";
        }

        public static string GetControlClassDelcaration(string name, Type baseType, string? classContent = null, string? attributeArguments = null, bool isPartial = true)
            => GetControlClassDelcaration(name, baseType.FullName!, classContent, attributeArguments, isPartial);

        public static string GetControlClassDelcaration(string name, string baseType, string? classContent = null, string? attributeArguments = null, bool isPartial = true)
        {
            return $@"    [Rocketcress.Core.Attributes.GenerateUIMapParts({attributeArguments})]
    public {(isPartial ? "partial " : string.Empty)}class {name} : {baseType}
    {{
        {(baseType == typeof(View).FullName ? "public override Rocketcress.Selenium.By RepresentedBy { get; }" : string.Empty)}
{classContent}
    }}";
        }

        public static string GetClassContent(params string[] parts)
            => string.Join(Environment.NewLine, parts);

        public static string GetUIMapControlDeclaration(string name, Type propertyType, string? attributeArguments = null, string? initCode = null)
        {
            return $@"        [Rocketcress.Core.Attributes.UIMapControl({attributeArguments})]
        public {propertyType.FullName} {name} {{ get; private set; }}{initCode}";
        }

        public static Type GetDriverType(Type baseType)
        {
            return baseType switch
            {
                _ when baseType == typeof(WebElement) || baseType == typeof(View) => typeof(WebDriver),
                _ when baseType == typeof(UITestControl) => typeof(Application),
                _ => throw new ArgumentException(null, nameof(baseType)),
            };
        }

        public static Type GetInitLocationKeyType(Type baseType)
        {
            return baseType switch
            {
                _ when baseType == typeof(WebElement) || baseType == typeof(View) => typeof(Selenium.By),
                _ when baseType == typeof(UITestControl) => typeof(UIAutomation.By),
                _ => throw new ArgumentException(null, nameof(baseType)),
            };
        }

        public static Type GetCtorLocationKeyType(Type baseType)
        {
            return baseType switch
            {
                _ when baseType == typeof(WebElement) || baseType == typeof(View) => typeof(OpenQA.Selenium.By),
                _ when baseType == typeof(UITestControl) => typeof(UIAutomation.By),
                _ => throw new ArgumentException(null, nameof(baseType)),
            };
        }

        public static Type GetParentControlType(Type baseType)
        {
            return baseType switch
            {
                _ when baseType == typeof(WebElement) || baseType == typeof(View) => typeof(OpenQA.Selenium.ISearchContext),
                _ when baseType == typeof(UITestControl) => typeof(IUITestControl),
                _ => throw new ArgumentException(null, nameof(baseType)),
            };
        }
    }
}
