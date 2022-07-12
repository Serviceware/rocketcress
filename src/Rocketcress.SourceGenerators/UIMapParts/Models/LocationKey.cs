using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rocketcress.Core.Attributes;
using Rocketcress.SourceGenerators.Extensions;
using System.Text.RegularExpressions;

namespace Rocketcress.SourceGenerators.UIMapParts.Models;

internal abstract class LocationKey
{
    private static readonly Regex PropertyNameSplitRegex = new(@"([A-Z]?[a-z0-9]+|[A-Z]+(?![a-z]))", RegexOptions.Compiled);

    public abstract bool IsStatic { get; }

    public static LocationKey Get(UIMapPartsGeneratorContext context, IPropertySymbol propertySymbol, UIMapControlOptions options, out INamedTypeSymbol? controlTypeOverride)
    {
        controlTypeOverride = null;

        if (propertySymbol.TryGetDeclaringSyntax<PropertyDeclarationSyntax>(out var declarationSyntax) &&
            declarationSyntax.Initializer is not null &&
            declarationSyntax.Initializer.Value is InvocationExpressionSyntax initUsingExpr &&
            initUsingExpr.Expression is GenericNameSyntax initUsingSyntax &&
            initUsingSyntax.Identifier.Text == "InitUsing")
        {
            if (initUsingSyntax.TypeArgumentList.Arguments.Count > 0)
                controlTypeOverride = context.Compilation.GetSemanticModel(initUsingSyntax.SyntaxTree).GetTypeInfo(initUsingSyntax.TypeArgumentList.Arguments[0]).Type as INamedTypeSymbol;

            if (initUsingExpr.ArgumentList.Arguments.Count > 0 && initUsingExpr.ArgumentList.Arguments[0].Expression is LambdaExpressionSyntax lambda && lambda.Body is not null)
            {
                var parameter = lambda is SimpleLambdaExpressionSyntax simpleLambda
                    ? simpleLambda.Parameter
                    : lambda is ParenthesizedLambdaExpressionSyntax parenthesizedLambda && parenthesizedLambda.ParameterList.Parameters.Count > 0
                        ? parenthesizedLambda.ParameterList.Parameters[0]
                        : null;

                return parameter != null
                    ? new InstanceLocationKey(lambda.Body, parameter.Identifier.Text)
                    : new StaticLocationKey(lambda.Body);
            }
        }

        var id = GetNameWithStyle(options.Id, propertySymbol.Name, options.IdStyle, options.IdFormat);
        return new IdLocationKey(id);
    }

    public abstract string BuildInitExpression(UIMapPartsGeneratorContext context);

    private static string? GetNameWithStyle(string? id, string name, IdStyle style, string? format)
    {
        if (style == IdStyle.Disabled || style == IdStyle.Unset)
            return null;

        if (string.IsNullOrWhiteSpace(id))
        {
            var words = PropertyNameSplitRegex.Matches(name).OfType<Match>().Where(x => x.Success).Select(x => x.Value.ToLowerInvariant()).ToArray();
            id = style switch
            {
                IdStyle.PascalCase => string.Concat(words.Select(x => MakeFirstCharacterUpperCase(x))),
                IdStyle.CamelCase => string.Concat(words.Skip(1).Select(x => MakeFirstCharacterUpperCase(x)).Prepend(words[0])),
                IdStyle.KebabCase => string.Join("-", words),
                IdStyle.LowerCase => string.Concat(words),
                IdStyle.UpperCase => string.Concat(words).ToUpperInvariant(),
                _ => name,
            };
        }

        return string.IsNullOrWhiteSpace(format) ? id : string.Format(format, id);

        static string MakeFirstCharacterUpperCase(string word) => word.Length == 0 ? word : (word[0].ToString().ToUpperInvariant() + word.Substring(1));
    }
}

internal class IdLocationKey : LocationKey
{
    private readonly string? _id;

    public IdLocationKey(string? id)
    {
        _id = id;
    }

    public override bool IsStatic { get; } = true;

    public override string BuildInitExpression(UIMapPartsGeneratorContext context)
    {
        if (string.IsNullOrEmpty(_id))
        {
            var locationKeyTypeName = context.UITestTypeSymbols.LocationKeyType.ToUsageString();
            return string.Format(context.EmptyLocationKeyFormat, locationKeyTypeName);
        }
        else
        {
            var locationKeyTypeName = context.UITestTypeSymbols.LocationKeyType.ToUsageString();
            return string.Format(context.IdLocationKeyFormat, locationKeyTypeName, _id);
        }
    }
}

internal class StaticLocationKey : LocationKey
{
    private readonly CSharpSyntaxNode _initBody;

    public StaticLocationKey(CSharpSyntaxNode initBody)
    {
        _initBody = initBody;
    }

    public override bool IsStatic { get; } = true;

    public override string BuildInitExpression(UIMapPartsGeneratorContext context)
    {
        var funcTypeName = context.TypeSymbols.Func1.Construct(context.UITestTypeSymbols.LocationKeyType).ToUsageString();

        if (_initBody is BlockSyntax)
            return $"new {funcTypeName}(() => {_initBody.GetText()}).Invoke()";
        else
            return _initBody.GetText().ToString();
    }
}

internal class InstanceLocationKey : LocationKey
{
    private readonly CSharpSyntaxNode _initBody;
    private readonly string _parameterName;

    public InstanceLocationKey(CSharpSyntaxNode initBody, string parameterName)
    {
        _initBody = initBody;
        _parameterName = parameterName;
    }

    public override bool IsStatic { get; } = false;

    public override string BuildInitExpression(UIMapPartsGeneratorContext context)
    {
        var funcTypeName = context.TypeSymbols.Func2.Construct(context.TypeSymbol, context.UITestTypeSymbols.LocationKeyType).ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

        return $"new {funcTypeName}({_parameterName} => {_initBody.GetText()}).Invoke(this)";
    }
}
