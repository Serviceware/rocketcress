using Microsoft.CodeAnalysis;
using Rocketcress.Core.Attributes;
using Rocketcress.SourceGenerators.Common;
using Rocketcress.SourceGenerators.Extensions;

namespace Rocketcress.SourceGenerators.UIMapParts.Models
{
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:Field names should not use Hungarian notation", Justification = "Id is fine.")]
    internal readonly struct GenerateUIMapPartsOptions
    {
        public GenerateUIMapPartsOptions(
            bool generateDefaultConstructors,
            IdStyle idStyle,
            string? idFormat)
        {
            GenerateDefaultConstructors = generateDefaultConstructors;
            IdStyle = idStyle;
            IdFormat = idFormat;
        }

        public bool GenerateDefaultConstructors { get; }
        public IdStyle IdStyle { get; }
        public string? IdFormat { get; }

        public static GenerateUIMapPartsOptions Get(UIMapPartsGeneratorContext context)
        {
            bool generateDefaultConstructors = GenerateUIMapPartsAttributeDefaults.GenerateDefaultConstructors;
            IdStyle idStyle = GenerateUIMapPartsAttributeDefaults.IdStyle;
            bool hasIdStyle = false;
            string? idFormat = GenerateUIMapPartsAttributeDefaults.IdFormat;
            bool hasIdFormat = false;

            bool isFirst = true;
            INamedTypeSymbol currentType = context.TypeSymbol;
            while (currentType is not null && (!hasIdStyle || !hasIdFormat))
            {
                if (!context.TypeSymbol.TryGetAttribute(context.TypeSymbols.GenerateUIMapPartsAttribute, out var attribute))
                    continue;

                foreach (var argument in attribute.NamedArguments)
                {
                    if (argument.Key == "GenerateDefaultConstructors")
                    {
                        if (isFirst && argument.Value.Value is bool boolValue)
                            generateDefaultConstructors = boolValue;
                    }
                    else if (argument.Key == "IdStyle")
                    {
                        if (!hasIdStyle && argument.Value.Value is int intValue)
                        {
                            idStyle = (IdStyle)intValue;
                            hasIdStyle = true;
                        }
                    }
                    else if (argument.Key == "IdFormat")
                    {
                        if (!hasIdFormat)
                        {
                            idFormat = argument.Value.Value as string;
                            hasIdFormat = true;
                        }
                    }
                }

                isFirst = false;
                currentType = currentType.ContainingType;
            }

            return new GenerateUIMapPartsOptions(
                generateDefaultConstructors,
                idStyle,
                idFormat);
        }

        public static bool GetGenerateDefaultConstructors(TypeSymbols typeSymbols, INamedTypeSymbol typeSymbol)
        {
            if (!typeSymbol.TryGetAttribute(typeSymbols.GenerateUIMapPartsAttribute, out var attribute))
                return false;

            foreach (var argument in attribute.NamedArguments)
            {
                if (argument.Key == "GenerateDefaultConstructors")
                {
                    if (argument.Value.Value is bool boolValue)
                        return boolValue;
                }
            }

            return false;
        }
    }
}
