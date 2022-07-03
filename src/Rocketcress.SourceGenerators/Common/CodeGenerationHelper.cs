using Microsoft.CodeAnalysis;
using Rocketcress.Core.Extensions;
using Rocketcress.SourceGenerators.Extensions;
using System.Text.RegularExpressions;

namespace Rocketcress.SourceGenerators.Common;

/// <summary>
/// Contains helper methods and values for C# 9 Source Generators.
/// </summary>
internal static class CodeGenerationHelper
{
    /// <summary>
    /// Creates a name out of a <see cref="ISymbol"/> that can be used for the generated file.
    /// </summary>
    /// <param name="symbol">The type symbol to use.</param>
    /// <param name="generatorName">The name of the generator.</param>
    /// <returns>Returns a name that can be used for a generated file.</returns>
    public static string CreateHintName(this ISymbol symbol, string generatorName)
    {
        return CreateHintName(symbol, x => (x, generatorName).GetHashCode());
    }

    /// <summary>
    /// Creates a file name that can be used for a generated file.
    /// </summary>
    /// <param name="name">The name of the file to use.</param>
    /// <param name="generatorName">The name of the generator.</param>
    /// <returns>Returns a name that can be used for a generated file.</returns>
    public static string CreateHintName(string name, string generatorName)
        => CreateHintName(name, (name, generatorName).GetHashCode());

    /// <summary>
    /// Creates a name out of a <see cref="ISymbol"/> that can be used for the generated file.
    /// </summary>
    /// <param name="symbol">The type symbol to use.</param>
    /// <param name="generatorName">The name of the generator.</param>
    /// <param name="additionalGenerationInfo">Additional info to create a unique hash for this generation.</param>
    /// <returns>Returns a name that can be used for a generated file.</returns>
    public static string CreateHintName(this ISymbol symbol, string generatorName, string additionalGenerationInfo)
    {
        return CreateHintName(symbol, x => (x, generatorName, additionalGenerationInfo).GetHashCode());
    }

    /// <summary>
    /// Creates a file name that can be used for a generated file.
    /// </summary>
    /// <param name="name">The name of the file to use.</param>
    /// <param name="generatorName">The name of the generator.</param>
    /// <param name="additionalGenerationInfo">Additional info to create a unique hash for this generation.</param>
    /// <returns>Returns a name that can be used for a generated file.</returns>
    public static string CreateHintName(string name, string generatorName, string additionalGenerationInfo)
        => CreateHintName(name, (name, generatorName, additionalGenerationInfo).GetHashCode());

    /// <summary>
    /// Launches the debugger if the generator was not executed from an IDE.
    /// </summary>
    public static void LaunchDebuggerOnBuild()
    {
        var processName = Process.GetCurrentProcess().ProcessName;
        if (!Debugger.IsAttached && processName != "ServiceHub.RoslynCodeAnalysisService" && processName != "devenv")
            Debugger.Launch();
    }

    private static string CreateHintName(ISymbol symbol, Func<string, int> hashFunc)
    {
        string unescapedFileName;
        string hashBaseName;

        switch (symbol)
        {
            case IAssemblySymbol assembly:
                unescapedFileName = assembly.Name;
                hashBaseName = assembly.Name;
                break;
            default:
                unescapedFileName = symbol.Name;
                hashBaseName = symbol.ToDisplayString(SymbolExtensions.DefinitionFormat.WithGenericsOptions(SymbolDisplayGenericsOptions.IncludeTypeParameters));
                break;
        }

        var name = Regex.Replace(unescapedFileName, @"[\.+]", "-");
        return CreateHintName(name, hashFunc(hashBaseName));
    }

    private static string CreateHintName(string name, int hash)
    {
        return $"{name}-{BitConverter.GetBytes(hash).ToHexString()}";
    }
}
