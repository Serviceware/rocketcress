using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rocketcress.SourceGenerators.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocketcress.SourceGenerators.Extensions
{
    internal static class SymbolExtensions
    {
        public static readonly SymbolDisplayFormat UsageFormat = new(
            SymbolDisplayGlobalNamespaceStyle.Included,
            SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
            SymbolDisplayGenericsOptions.IncludeTypeParameters,
            SymbolDisplayMemberOptions.IncludeParameters,
            SymbolDisplayDelegateStyle.NameAndSignature,
            SymbolDisplayExtensionMethodStyle.Default,
            SymbolDisplayParameterOptions.IncludeParamsRefOut | SymbolDisplayParameterOptions.IncludeName,
            SymbolDisplayPropertyStyle.NameOnly,
            SymbolDisplayLocalOptions.None,
            SymbolDisplayKindOptions.None,
            SymbolDisplayMiscellaneousOptions.UseSpecialTypes);

        public static readonly SymbolDisplayFormat TypeDefinitionFormat = new(
            SymbolDisplayGlobalNamespaceStyle.Omitted,
            SymbolDisplayTypeQualificationStyle.NameOnly,
            SymbolDisplayGenericsOptions.IncludeTypeParameters | SymbolDisplayGenericsOptions.IncludeVariance,
            SymbolDisplayMemberOptions.IncludeType | SymbolDisplayMemberOptions.IncludeParameters | SymbolDisplayMemberOptions.IncludeRef,
            SymbolDisplayDelegateStyle.NameAndSignature,
            SymbolDisplayExtensionMethodStyle.Default,
            SymbolDisplayParameterOptions.IncludeParamsRefOut | SymbolDisplayParameterOptions.IncludeType | SymbolDisplayParameterOptions.IncludeName | SymbolDisplayParameterOptions.IncludeDefaultValue,
            SymbolDisplayPropertyStyle.NameOnly,
            SymbolDisplayLocalOptions.None,
            SymbolDisplayKindOptions.IncludeMemberKeyword,
            SymbolDisplayMiscellaneousOptions.UseSpecialTypes);

        public static readonly SymbolDisplayFormat DefinitionFormat = new(
            SymbolDisplayGlobalNamespaceStyle.Included,
            SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
            SymbolDisplayGenericsOptions.IncludeTypeParameters | SymbolDisplayGenericsOptions.IncludeTypeConstraints | SymbolDisplayGenericsOptions.IncludeVariance,
            SymbolDisplayMemberOptions.IncludeType | SymbolDisplayMemberOptions.IncludeParameters | SymbolDisplayMemberOptions.IncludeRef,
            SymbolDisplayDelegateStyle.NameAndSignature,
            SymbolDisplayExtensionMethodStyle.Default,
            SymbolDisplayParameterOptions.IncludeParamsRefOut | SymbolDisplayParameterOptions.IncludeType | SymbolDisplayParameterOptions.IncludeName | SymbolDisplayParameterOptions.IncludeDefaultValue,
            SymbolDisplayPropertyStyle.NameOnly,
            SymbolDisplayLocalOptions.None,
            SymbolDisplayKindOptions.IncludeMemberKeyword,
            SymbolDisplayMiscellaneousOptions.UseSpecialTypes);

        public static string ToUsageString(this ISymbol symbol)
            => symbol.ToDisplayString(UsageFormat);

        public static string ToTypeDefinitionString(this ISymbol symbol)
            => symbol.ToDisplayString(TypeDefinitionFormat);

        public static string ToDefinitionString(this ISymbol symbol)
            => symbol.ToDisplayString(DefinitionFormat);

        /// <summary>
        /// Determines all types that are defined inside a given namespace (includes descendant namespaces).
        /// </summary>
        /// <param name="symbol">The namespace to search in.</param>
        /// <returns>Returns an enumerable that enumerates through all types inside the given namespace <paramref name="symbol"/>.</returns>
        public static IEnumerable<INamedTypeSymbol> GetNamespaceTypes(this INamespaceSymbol symbol)
        {
            foreach (var child in symbol.GetTypeMembers())
            {
                yield return child;
            }

            foreach (var ns in symbol.GetNamespaceMembers())
            {
                foreach (var child2 in ns.GetNamespaceTypes())
                {
                    yield return child2;
                }
            }
        }

        /// <summary>
        /// Determines all types that are defined inside a given namespace (includes descendant namespaces) and their types.
        /// </summary>
        /// <param name="symbol">The namespace to search in.</param>
        /// <returns>Returns an enumerable that enumerates through all types inside the given namespace <paramref name="symbol"/> and their types.</returns>
        public static IEnumerable<INamedTypeSymbol> GetNamespaceAndNestedTypes(this INamespaceSymbol symbol)
        {
            foreach (var child in symbol.GetTypeMembers())
            {
                yield return child;
                foreach (var child2 in GetNestedTypes(child))
                    yield return child2;
            }

            foreach (var ns in symbol.GetNamespaceMembers())
            {
                foreach (var child in ns.GetNamespaceTypes())
                {
                    yield return child;
                    foreach (var child2 in GetNestedTypes(child))
                        yield return child2;
                }
            }
        }

        /// <summary>
        /// Determines all nested types that are defined inside a given type.
        /// </summary>
        /// <param name="symbol">The type to search in.</param>
        /// <returns>Returns an enumerable that enumerates through all nested types inside the given type <paramref name="symbol"/>.</returns>
        public static IEnumerable<INamedTypeSymbol> GetNestedTypes(this INamedTypeSymbol symbol)
        {
            foreach (var child in symbol.GetTypeMembers())
            {
                yield return child;
                foreach (var child2 in GetNestedTypes(child))
                    yield return child2;
            }
        }

        /// <summary>
        /// Determines all attributes of a given symbol (includes base types).
        /// </summary>
        /// <param name="symbol">The symbol to search in.</param>
        /// <returns>Returns an enumerable that enumerates through all attributes defined for the symbol and its base types.</returns>
        public static IEnumerable<AttributeData> GetAllAttributes(this ISymbol symbol)
        {
            while (symbol != null)
            {
                foreach (var attribute in symbol.GetAttributes())
                {
                    yield return attribute;
                }

                symbol = (symbol as INamedTypeSymbol)?.BaseType;
            }
        }

        /// <summary>
        /// Determines all base types of a given symbol.
        /// </summary>
        /// <param name="symbol">The symbol to search in.</param>
        /// <returns>Returns an enumerable that enumerates through all base types of the defined type symbol.</returns>
        public static IEnumerable<INamedTypeSymbol> GetAllBaseTypes(this INamedTypeSymbol symbol)
        {
            var current = symbol.BaseType;
            while (current != null)
            {
                yield return current;
                current = current.BaseType;
            }
        }

        public static bool IsAssignableTo(this INamedTypeSymbol symbol, INamedTypeSymbol baseTypeSymbol)
        {
            var current = symbol.BaseType;
            while (current is not null)
            {
                if (SymbolEqualityComparer.Default.Equals(current, baseTypeSymbol))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether the specified symbol has the partial modifier.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <returns>
        ///   <c>true</c> if the specified symbol has the partial modifier; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasPartialModifier(this ISymbol symbol)
        {
            return (from @ref in symbol.DeclaringSyntaxReferences
                    let syntax = @ref.GetSyntax()
                    where syntax is MemberDeclarationSyntax declarationSyntax
                       && declarationSyntax.Modifiers.Any(SyntaxKind.PartialKeyword)
                    select syntax).Any();
        }

        public static bool TryGetAttribute(this ISymbol symbol, INamedTypeSymbol attributeTypeSymbol, out AttributeData attribute)
        {
            foreach (var attributeData in symbol.GetAttributes())
            {
                if (SymbolEqualityComparer.Default.Equals(attributeData.AttributeClass, attributeTypeSymbol))
                {
                    attribute = attributeData;
                    return true;
                }
            }

            attribute = null!;
            return false;
        }

        public static bool TryGetDeclaringSyntax<T>(this ISymbol symbol, out T syntax)
            where T : SyntaxNode
        {
            syntax = symbol.DeclaringSyntaxReferences.Select(x => x.GetSyntax()).OfType<T>().FirstOrDefault();
            return syntax is not null;
        }
    }
}
