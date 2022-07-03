using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rocketcress.SourceGenerators.Common;
using Rocketcress.SourceGenerators.Extensions;

namespace Rocketcress.SourceGenerators.UIMapParts.Models
{
    internal record ControlDefinition(
        IPropertySymbol Property,
        bool Initialize,
        IPropertySymbol? Parent,
        LocationKey? LocationKey,
        ISymbol? ExistingLocationKey)
    {
        public static ControlDefinition[] GetAll(UIMapPartsGeneratorContext context)
        {
            var classOptions = GenerateUIMapPartsOptions.Get(context);

            var result = new List<ControlDefinition>();

            foreach (var member in context.TypeSymbol.GetMembers())
            {
                if (member is not IPropertySymbol propertySymbol || !propertySymbol.TryGetAttribute(context.TypeSymbols.UIMapControlAttribute, out var attribute))
                    continue;

                var options = UIMapControlOptions.FromAttribute(attribute, classOptions);

                ISymbol? existingLocationKey = context.TypeSymbol.GetMembers($"By{propertySymbol.Name}").FirstOrDefault(x => x is IFieldSymbol || x is IPropertySymbol);
                LocationKey? locationKey = null;
                if (existingLocationKey is null)
                    locationKey = LocationKey.Get(context, propertySymbol, options);

                bool initialize = options.Initialize;
                IPropertySymbol? parent = null;
                if (options.ParentControl is not null)
                {
                    parent = context.TypeSymbol.GetMembers(options.ParentControl).OfType<IPropertySymbol>().FirstOrDefault();
                    if (parent is null)
                    {
                        var location = GetUIMapControlParentControlLocation(context, propertySymbol);
                        var propertyName = propertySymbol.Name;
                        var uimapTypeName = context.TypeSymbol.Name;
                        var parentControlName = options.ParentControl;
                        context.ReportDiagnostic(DiagnosticFactory.UIMapParts.MissingParentControl(location, propertyName, uimapTypeName, parentControlName));
                        initialize = false;
                    }
                }

                var controlDefinition = new ControlDefinition(
                    propertySymbol,
                    initialize,
                    parent,
                    locationKey,
                    existingLocationKey);

                var index = result.IndexOf(x => x.Parent is not null && x.Parent.Name == propertySymbol.Name);
                if (index > 0)
                    result.Insert(index, controlDefinition);
                else
                    result.Add(controlDefinition);
            }

            return result.ToArray();
        }

        private static PropertyDeclarationSyntax? GetPropertySyntax(IPropertySymbol propertySymbol)
        {
            foreach (var syntaxReference in propertySymbol.DeclaringSyntaxReferences)
            {
                if (syntaxReference.GetSyntax() is PropertyDeclarationSyntax propertyDeclarationSyntax)
                    return propertyDeclarationSyntax;
            }

            return null;
        }

        private static AttributeSyntax? GetUIMapControlSyntax(UIMapPartsGeneratorContext context, IPropertySymbol propertySymbol)
        {
            var propertyDeclarationSyntax = GetPropertySyntax(propertySymbol);
            if (propertyDeclarationSyntax is null)
                return null;

            if (!propertyDeclarationSyntax.TryGetAttributeSyntax(context.Compilation.GetSemanticModel(propertyDeclarationSyntax.SyntaxTree), TypeSymbols.Names.UIMapControlAttribute, out var attributeSyntax))
                return null;

            return attributeSyntax;
        }

        private static Location? GetUIMapControlParentControlLocation(UIMapPartsGeneratorContext context, IPropertySymbol propertySymbol)
        {
            var attributeSyntax = GetUIMapControlSyntax(context, propertySymbol);
            if (attributeSyntax is null)
                return null;

            return attributeSyntax.GetNamedArgumentSyntax("ParentControl")?.GetLocation();
        }
    }
}
