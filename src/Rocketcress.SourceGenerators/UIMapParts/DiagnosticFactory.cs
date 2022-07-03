using Microsoft.CodeAnalysis;
using Rocketcress.SourceGenerators.UIMapParts.Selenium;
using Rocketcress.SourceGenerators.UIMapParts.UIAutomation;

namespace Rocketcress.SourceGenerators
{
    internal static partial class DiagnosticFactory
    {
        public static class UIMapParts
        {
            private const string Category = "Rocketcress.UIMapPartsGenerator";

            private static readonly DiagnosticDescriptor ClassMustBePartialDescriptor = new(
                GetId(1),
                "Classes using the GenerateUIMapParts attribute must be partial",
                "The type '{0}' has the GenerateUIMapParts attribute but is not partial. For the source generator to work properly, the class must be partial.",
                Category,
                DiagnosticSeverity.Error,
                true);

            private static readonly DiagnosticDescriptor AllParentTypesMustBePartialDescriptor = new(
                GetId(2),
                "Classes containing nested classes using the GenerateUIMapParts attribute must be partial",
                "The tpye '{0}' has the GenerateUIMapParts attribute but parent class '{1}' is not partial. For the source generator to work properly, all parent classes of '{0}' must be partial.",
                Category,
                DiagnosticSeverity.Error,
                true);

            private static readonly DiagnosticDescriptor BadBaseTypeDescriptor = new(
                GetId(3),
                "Classes using the GenerateUIMapParts attribute must derive from supported type",
                $"The type '{{0}}' has the GenerateUIMapParts attribute but it does not derive from a supported base type. Supported base types are '{SeleniumTypeSymbols.Names.UIMapBaseClassType}' and '{UIAutomationTypeSymbols.Names.UIMapBaseClassType}'.",
                Category,
                DiagnosticSeverity.Error,
                true);

            private static readonly DiagnosticDescriptor MissingParentControlDescriptor = new(
                GetId(4),
                "Defined ParentControl on UIMapAttributes must exist as property in same class.",
                "The UIMap control '{0}' on type '{1}' defines '{2}' as its parent. A property '{2}' does not exist in class '{1}' though.",
                Category,
                DiagnosticSeverity.Error,
                true);

            public static Diagnostic ClassMustBePartial(Location? classNameLocation, string typeName)
                => Diagnostic.Create(ClassMustBePartialDescriptor, classNameLocation, typeName);

            public static Diagnostic AllParentTypesMustBePartial(Location? nonPartialClassNameLocation, string uimapTypeName, string nonPartialTypeName)
                => Diagnostic.Create(AllParentTypesMustBePartialDescriptor, nonPartialClassNameLocation, uimapTypeName, nonPartialTypeName);

            public static Diagnostic BadBaseType(Location? classNameLocation, string typeName)
                => Diagnostic.Create(BadBaseTypeDescriptor, classNameLocation, typeName);

            public static Diagnostic MissingParentControl(Location? parentControlArgumentLocation, string controlPropertyName, string uimapTypeName, string parentControlName)
                => Diagnostic.Create(MissingParentControlDescriptor, parentControlArgumentLocation, controlPropertyName, uimapTypeName, parentControlName);

            private static string GetId(int id) => DiagnosticFactory.GetId(100 + id);
        }
    }
}
