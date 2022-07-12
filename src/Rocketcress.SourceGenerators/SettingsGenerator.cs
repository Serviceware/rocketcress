using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Newtonsoft.Json.Linq;
using Rocketcress.SourceGenerators.Common;
using Rocketcress.SourceGenerators.Extensions;
using System.Text;
using System.Text.RegularExpressions;
using static Rocketcress.SourceGenerators.Common.CodeGenerationHelper;

namespace Rocketcress.SourceGenerators;

/// <summary>
/// Generator for creating accessor file for settings files.
/// </summary>
/// <seealso cref="Microsoft.CodeAnalysis.ISourceGenerator" />
[Generator]
public class SettingsGenerator : ISourceGenerator
{
    private static readonly string[] KnownSettingsTypeNames = new[]
    {
        "Rocketcress.Selenium.Settings",
        "Rocketcress.UIAutomation.Settings",
    };

    /// <inheritdoc/>
    public void Initialize(GeneratorInitializationContext context)
    {
    }

    /// <inheritdoc/>
    public void Execute(GeneratorExecutionContext context)
    {
        var debugGeneratorSymbol = context.Compilation.GetTypeByMetadataName("Rocketcress.Core.Attributes.DebugGeneratorAttribute");

        if (debugGeneratorSymbol is not null)
        {
            var assemblyAttributes = context.Compilation.Assembly.GetAttributes();
            if (assemblyAttributes.Any(x => SymbolEqualityComparer.Default.Equals(x.AttributeClass, debugGeneratorSymbol)))
                LaunchDebuggerOnBuild();
        }

        context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.rootnamespace", out var globalNamespaceName);
        var settingsFile = context.AdditionalFiles.FirstOrDefault(x => Path.GetFileName(x.Path) == "settings.json");
        if (settingsFile != null)
        {
            context.AnalyzerConfigOptions.GetOptions(settingsFile).TryGetValue("build_metadata.AdditionalFiles.SettingsType", out var settingsType);

            var testBaseSymbol = context.Compilation.GetTypeByMetadataName("Rocketcress.Core.Base.TestBase`2");

            IEnumerable<INamedTypeSymbol> testClasses;
            if (testBaseSymbol is not null)
            {
                var unboundTestBaseSymbol = testBaseSymbol.ConstructUnboundGenericType();
                testClasses = from typeSymbol in context.Compilation.SourceModule.GlobalNamespace.GetNamespaceTypes()
                              where typeSymbol.GetAllBaseTypes()
                                  .Where(x => x.IsGenericType)
                                  .Select(x => x.ConstructUnboundGenericType())
                                  .Contains(unboundTestBaseSymbol, SymbolEqualityComparer.Default)
                              select typeSymbol;
            }
            else
            {
                testClasses = Array.Empty<INamedTypeSymbol>();
            }

            var source = GenerateFromFile(settingsFile.Path, globalNamespaceName!, GetSettingsType(context, settingsType!), testClasses);

            context.AddSource(CreateHintName("Settings", nameof(SettingsGenerator)), SourceText.From(source, Encoding.UTF8));
        }
    }

    private static string GenerateFromFile(string filePath, string namespaceName, string settingsType, IEnumerable<INamedTypeSymbol> testClasses)
    {
        var metadata = GetMetadata(filePath);
        var sb = new SourceBuilder();

        sb.AppendLine("using Rocketcress.Core.Attributes;")
          .AppendLine("using Rocketcress.Core.Base;")
          .AppendLine("using Rocketcress.Core.Models;")
          .AppendLine();

        using (sb.AddBlock($"namespace {namespaceName}"))
        {
            using (sb.AddRegion("Setting Key Classes"))
                GenerateKeyClasses(sb, metadata);

            sb.AppendLine();

            using (sb.AddRegion("Setting Classes"))
                GenerateSettingClasses(sb, metadata);
        }

        sb.AppendLine();

        using (sb.AddRegion("Test Class Extensions"))
            GenerateTestClassExtensions(sb, metadata, testClasses);

        return sb.ToString();
    }

    private static string GetSettingsType(GeneratorExecutionContext context, string definedSettingsType)
    {
        if (!string.IsNullOrWhiteSpace(definedSettingsType))
            return definedSettingsType;

        foreach (var typeName in KnownSettingsTypeNames)
        {
            if (context.Compilation.GetTypeByMetadataName(typeName) is not null)
                return typeName;
        }

        return "SettingsBase";
    }

    private static SettingsMetadata GetMetadata(string settingsFilePath)
    {
        var settings = JObject.Parse(File.ReadAllText(settingsFilePath));

        var metadata = new SettingsMetadata();
        if (settings["KeyClasses"] != null)
        {
            foreach (var @class in settings["KeyClasses"]?.ToObject<Dictionary<string, string>>() ?? new Dictionary<string, string>())
            {
                metadata.KeyClasses.Add(new KeyClassMetadata
                {
                    Prefix = @class.Key,
                    Name = @class.Value.EndsWith("Keys") ? @class.Value[0..^4] : @class.Value,
                });
            }
        }
        else
        {
            metadata.KeyClasses.Add(new KeyClassMetadata
            {
                Prefix = "TL_",
                Name = "Translation",
            });
        }

        if (settings["SettingsTypes"] != null)
            metadata.SettingsTypes = settings["SettingsTypes"]?.ToObject<List<SettingsType>>();
        else
            metadata.SettingsTypes = new List<SettingsType>();

        var allKeys = settings["OtherSettings"]?.OfType<JProperty>().Select(x => x.Name).ToArray() ?? System.Array.Empty<string>();
        foreach (var key in allKeys)
        {
            var match = Regex.Match(key, @"\A(\[(?<Tag>[^\]]+)\])?\s*(?<Name>.*)\Z");
            var tag = match.Groups["Tag"].Value;
            var name = match.Groups["Name"].Success ? match.Groups["Name"].Value : key;

            var keyClass = metadata.KeyClasses.Where(x => name.StartsWith(x.Prefix)).FirstOrDefault();
            if (keyClass != null)
                keyClass.Keys.Add(new SettingsKey(key, keyClass.Prefix, tag, name[(keyClass.Prefix?.Length ?? 0)..]));
            else
                metadata.DefaultKeys.Add(new SettingsKey(key, null, tag, name));
        }

        return metadata;
    }

    private static void GenerateKeyClasses(SourceBuilder sb, SettingsMetadata metadata)
    {
        foreach (var @class in metadata.KeyClasses.Where(x => x.Keys.Count > 0))
        {
            sb.AppendLine($"[AddKeysClass(typeof({@class.Name}Keys))]");
        }

        using (sb.AddBlock("public static class SettingKeys"))
            GenerateFields(metadata.DefaultKeys, 0);

        foreach (var @class in metadata.KeyClasses.Where(x => x.Keys.Count > 0))
        {
            sb.AppendLine();
            using (sb.AddBlock($"public static class {@class.Name}Keys"))
                GenerateFields(@class.Keys, @class.Prefix?.Length ?? 0);
        }

        void GenerateFields(IEnumerable<SettingsKey> keys, int prefixLength)
        {
            var hasKey = false;
            foreach (var key in keys)
            {
                hasKey = true;
                sb.AppendLine($"public static readonly string {key.Key} = \"{key.FullKey}\";");
            }

            if (!hasKey)
                sb.AppendLine("// Currently no keys are available in settings file.");
        }
    }

    private static void GenerateSettingClasses(SourceBuilder sb, SettingsMetadata metadata)
    {
        using (sb.AddBlock($"public static class SettingsExtensions"))
        {
            var keyClasses = metadata.KeyClasses.Where(x => x.Keys.Count > 0).ToArray();

            if (metadata.DefaultKeys.Any())
            {
                sb.AppendLine($"public static SettingValuesAccessor SettingValues(this TestContextBase context) => GetSettingClassInstance<SettingValuesAccessor>(context.Settings);");
                sb.AppendLine($"public static SettingValuesAccessor Values(this SettingsBase settings) => GetSettingClassInstance<SettingValuesAccessor>(settings);");
            }

            foreach (var @class in keyClasses)
            {
                sb.AppendLine($"public static {@class.Name}ValuesAccessor {@class.Name}Values(this TestContextBase context) => GetSettingClassInstance<{@class.Name}ValuesAccessor>(context.Settings);");
                sb.AppendLine($"public static {@class.Name}ValuesAccessor {@class.Name}Values(this SettingsBase settings) => GetSettingClassInstance<{@class.Name}ValuesAccessor>(settings);");
            }

            if (keyClasses.Any())
                sb.AppendLine();

            if (!keyClasses.Any() && !metadata.DefaultKeys.Any())
            {
                sb.AppendLine("// Currently no keys are available in settings file.");
                return;
            }

            using (sb.AddBlock($"private static T GetSettingClassInstance<T>(SettingsBase settings)"))
            {
                sb.AppendLine("var key = $\"__settings_class_{typeof(T).Name}\";");
                using (sb.AddBlock("if (!settings.OtherSettings.TryGetValue(key, out object instance))"))
                {
                    sb.AppendLine("instance = (T)global::System.Activator.CreateInstance(typeof(T), new object[] { settings });")
                        .AppendLine("settings.OtherSettings[key] = instance;");
                }

                sb.AppendLine()
                    .AppendLine("return (T)instance;");
            }

            if (metadata.DefaultKeys.Any())
            {
                sb.AppendLine();
                GenerateSettingClass("Setting", metadata.DefaultKeys);
            }

            foreach (var @class in keyClasses)
            {
                sb.AppendLine();
                GenerateSettingClass(@class.Name, @class.Keys);
            }
        }

        void GenerateSettingClass(string? className, IEnumerable<SettingsKey> keys)
        {
            using (sb.AddBlock($"public class {className}ValuesAccessor"))
            {
                sb.AppendLine("private readonly SettingsBase _settings;")
                  .AppendLine();

                using (sb.AddBlock($"public {className}ValuesAccessor(SettingsBase settings)"))
                    sb.AppendLine("_settings = settings;");

                foreach (var key in keys)
                {
                    var type = metadata.SettingsTypes.FirstOrDefault(x => x.TagName == key.Tag)?.TypeName ?? "object";

                    sb.AppendLine();
                    using (sb.AddBlock($"public {type} {key.Key}"))
                    {
                        sb.AppendLine($"get => _settings.Get<{type}>({className}Keys.{key.Key});");
                        sb.AppendLine($"set => _settings.OtherSettings[{className}Keys.{key.Key}] = value;");
                    }
                }
            }
        }
    }

    private static void GenerateTestClassExtensions(SourceBuilder sb, SettingsMetadata metadata, IEnumerable<INamedTypeSymbol> testClasses)
    {
        bool isFirst = true;
        foreach (var testClass in testClasses)
        {
            if (!isFirst)
                sb.AppendLine();
            isFirst = false;

            if (testClass.HasPartialModifier())
                GenerateTestClassExtension(testClass);
            else
                sb.AppendLine($"// Not generating partial class for {testClass.ToDefinitionString()} because it is itself not marked as partial.");
        }

        if (isFirst)
            sb.AppendLine("// No test classes found to extend.");

        void GenerateTestClassExtension(INamedTypeSymbol testClass)
        {
            using (sb.AddBlock($"namespace {testClass.ContainingNamespace.ToDisplayString()}"))
            using (sb.AddBlock($"partial class {testClass.ToTypeDefinitionString()}"))
            {
                var keyClasses = metadata.KeyClasses.Where(x => x.Keys.Count > 0).ToArray();

                if (metadata.DefaultKeys.Any())
                    sb.AppendLine("private SettingsExtensions.SettingValuesAccessor SettingValues => Context?.Settings.Values();");

                foreach (var keyClass in keyClasses)
                {
                    sb.AppendLine($"private SettingsExtensions.{keyClass.Name}ValuesAccessor {keyClass.Name}Values => Context?.Settings.{keyClass.Name}Values();");
                }
            }
        }
    }

    private class SettingsMetadata
    {
        public List<SettingsKey> DefaultKeys { get; } = new List<SettingsKey>();
        public List<KeyClassMetadata> KeyClasses { get; } = new List<KeyClassMetadata>();
        public List<SettingsType>? SettingsTypes { get; set; }
    }

    private class KeyClassMetadata
    {
        public string? Prefix { get; set; }
        public string? Name { get; set; }
        public List<SettingsKey> Keys { get; } = new List<SettingsKey>();
    }

    private class SettingsKey
    {
        public SettingsKey(string fullKey, string? prefix, string tag, string key)
        {
            FullKey = fullKey;
            Prefix = prefix;
            Tag = tag;
            Key = key;
        }

        public string FullKey { get; }
        public string? Prefix { get; }
        public string Tag { get; }
        public string Key { get; }
    }

    private class SettingsType
    {
        public string? TypeName { get; set; }
        public string? TagName { get; set; }
    }
}
