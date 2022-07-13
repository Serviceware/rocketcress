using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rocketcress.SourceGenerators.Extensions;
using Rocketcress.SourceGenerators.UIMapParts.Common;

namespace Rocketcress.SourceGenerators.UIMapParts.Models;

internal record ControlDefinition(
    IPropertySymbol Property,
    ITypeSymbol ControlType,
    bool Initialize,
    string? ParentName,
    IPropertySymbol? Parent,
    LocationKey? LocationKey,
    ISymbol? ExistingLocationKey)
{
    public static ControlDefinition[] GetAll(UIMapPartsGeneratorContext context, GenerateUIMapPartsOptions classOptions)
    {
        var result = new List<ControlDefinition>();

        foreach (var member in context.TypeSymbol.GetMembers())
        {
            if (member is not IPropertySymbol propertySymbol || !propertySymbol.TryGetAttribute(context.TypeSymbols.UIMapControlAttribute, out var attribute))
                continue;

            var options = UIMapControlOptions.FromAttribute(attribute, classOptions);

            ITypeSymbol controlType = propertySymbol.Type;
            ISymbol? existingLocationKey = context.TypeSymbol.GetMembers($"By{propertySymbol.Name}").FirstOrDefault(x => x is IFieldSymbol || x is IPropertySymbol);
            LocationKey? locationKey = null;
            if (existingLocationKey is null)
            {
                locationKey = LocationKey.Get(context, propertySymbol, options, out var controlTypeOverride);
                if (controlTypeOverride is not null)
                    controlType = controlTypeOverride;
            }

            bool initialize = options.Initialize;
            IPropertySymbol? parent = null;
            if (options.ParentControl is not null && options.ParentControl != "this")
            {
                parent = context.TypeSymbol.GetAllProperties(options.ParentControl).FirstOrDefault();
                if (parent is null)
                    initialize = false;
            }

            var controlDefinition = new ControlDefinition(
                propertySymbol,
                controlType,
                initialize,
                options.ParentControl,
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
}
