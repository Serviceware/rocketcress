using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Newtonsoft.Json.Linq;
using Rocketcress.SourceGenerators.Common;
using System.Text;
using System.Text.RegularExpressions;
using static Rocketcress.SourceGenerators.Common.CodeGenerationHelpers;

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
        context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.rootnamespace", out var globalNamespaceName);
        var settingsFile = context.AdditionalFiles.FirstOrDefault(x => Path.GetFileName(x.Path) == "settings.json");
        if (settingsFile != null)
        {
            context.AnalyzerConfigOptions.GetOptions(settingsFile).TryGetValue("build_metadata.AdditionalFiles.SettingsType", out var settingsType);

            var source = GenerateFromFile(settingsFile.Path, globalNamespaceName, GetSettingsType(context, settingsType));

            context.AddSource(CreateHintName("Settings", nameof(SettingsGenerator)), SourceText.From(source, Encoding.UTF8));
        }
    }

    private static string GenerateFromFile(string filePath, string namespaceName, string settingsType)
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

            sb.AppendLine();

            GenerateSettingsLoader(sb, settingsType);
        }

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
            foreach (var @class in settings["KeyClasses"].ToObject<Dictionary<string, string>>())
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
            metadata.SettingsTypes = settings["SettingsTypes"].ToObject<List<SettingsType>>();
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
                keyClass.Keys.Add(new SettingsKey(key, keyClass.Prefix, tag, name[keyClass.Prefix.Length..]));
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
                GenerateFields(@class.Keys, @class.Prefix.Length);
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
        GenerateSettingClass("Setting", metadata.DefaultKeys);
        foreach (var @class in metadata.KeyClasses.Where(x => x.Keys.Count > 0))
        {
            sb.AppendLine();
            GenerateSettingClass(@class.Name, @class.Keys);
        }

        void GenerateSettingClass(string className, IEnumerable<SettingsKey> keys)
        {
            using (sb.AddBlock($"public static class {className}Values"))
            {
                sb.AppendLine("private static readonly PropertyStorage _properties = new PropertyStorage();")
                  .AppendLine();

                var hasKey = false;
                foreach (var key in keys)
                {
                    hasKey = true;
                    var type = metadata.SettingsTypes.FirstOrDefault(x => x.TagName == key.Tag)?.TypeName ?? "object";
                    sb.AppendLine($"public static {type} {key.Key} => _properties.GetProperty(() => SettingsLoader.Settings.Get<{type}>({className}Keys.{key.Key}));");
                }

                if (!hasKey)
                    sb.AppendLine("// Currently no keys are available in settings file.");
            }
        }
    }

    private static void GenerateSettingsLoader(SourceBuilder sb, string settingsType)
    {
        using (sb.AddBlock("public static class SettingsLoader"))
        {
            sb.AppendLine("private static object _settingsLock = new object();")
              .AppendLine($"private static {settingsType} _settings;")
              .AppendLine();

            using (sb.AddBlock($"public static {settingsType} Settings"))
            using (sb.AddBlock("get"))
            using (sb.AddBlock("lock (_settingsLock)"))
                sb.AppendLine("return _settings ??= LoadSettings();");

            sb.AppendLine();
            using (sb.AddBlock($"private static {settingsType} LoadSettings()"))
            {
                sb.AppendLine("var assembly = typeof(SettingsLoader).Assembly;")
                  .AppendLine("var specificSettingsFile = SettingsBase.GetSettingFile(assembly, null, false);")
                  .AppendLine("var defaultSettingsFile = SettingsBase.GetSettingFile(assembly, null, true);")
                  .AppendLine($"return SettingsBase.GetFromFiles<{settingsType}>(specificSettingsFile, defaultSettingsFile);");
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
        public string FullKey { get; }
        public string? Prefix { get; }
        public string Tag { get; }
        public string Key { get; }

        public SettingsKey(string fullKey, string? prefix, string tag, string key)
        {
            FullKey = fullKey;
            Prefix = prefix;
            Tag = tag;
            Key = key;
        }
    }

    private class SettingsType
    {
        public string? TypeName { get; set; }
        public string? TagName { get; set; }
    }
}
