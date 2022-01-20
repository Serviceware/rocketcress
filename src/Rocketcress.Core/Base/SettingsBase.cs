using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rocketcress.Core.Attributes;
using Rocketcress.Core.Common;
using System.Globalization;
using System.Reflection;

namespace Rocketcress.Core.Base;

/// <summary>
/// Base class for settings that are used for test executions.
/// </summary>
[Serializable]
public class SettingsBase
{
    /// <summary>
    /// Gets or sets the default timeout for the wait operations.
    /// </summary>
    public virtual TimeSpan Timeout { get; set; }

    /// <summary>
    /// Gets or sets the username to login in the tests.
    /// </summary>
    public virtual string? Username { get; set; }

    /// <summary>
    /// Gets or sets the password of the username to login in the tests.
    /// </summary>
    public virtual string? Password { get; set; }

    /// <summary>
    /// Gets or sets the language the test should be executed with.
    /// </summary>
    public virtual CultureInfo Language { get; set; }

    /// <summary>
    /// Gets the container in which all other settings are saved.
    /// </summary>
    [field: NonSerialized]
    public IDictionary<string, object?> OtherSettings { get; }

    /// <summary>
    /// Gets a list of all defined Settings-Type prefixes.
    /// </summary>
    public IList<SettingsType> SettingsTypes { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SettingsBase"/> class.
    /// </summary>
    public SettingsBase()
    {
        Timeout = TimeSpan.FromMinutes(1);
        Language = CultureInfo.GetCultureInfo("en-US");
        OtherSettings = new Dictionary<string, object?>(new SettingKeyEqualityComparer());
        SettingsTypes = new List<SettingsType>();
    }

    /// <summary>
    /// Checks if the settings contain all the settings keys defined by the given type.
    /// </summary>
    /// <param name="type">The type of the main SettingsKeys class.</param>
    public void CheckRequiredKeys(Type type)
    {
        var requiredKeys = AddKeysClassAttribute.GetKeys(type);
        var missingSettings = (from x in requiredKeys
                               where !OtherSettings.ContainsKey(x)
                               select x).ToArray();
        if (missingSettings.Length > 0)
        {
            Logger.LogWarning("The following required setting keys were not provided in the settings file:" +
                Environment.NewLine + "\t- " + string.Join(Environment.NewLine + "\t- ", missingSettings));
        }
    }

    /// <summary>
    /// Gets a specific setting.
    /// </summary>
    /// <typeparam name="T">The type of the setting value.</typeparam>
    /// <param name="settingKey">The setting key to get the value of.</param>
    /// <returns>The value of the setting.</returns>
    public virtual T? Get<T>(string settingKey) => Get<T>(settingKey, default);

    /// <summary>
    /// Gets a specific setting.
    /// </summary>
    /// <typeparam name="T">The type of the setting value.</typeparam>
    /// <param name="settingKey">The setting key to get the value of.</param>
    /// <param name="defaultValue">The value to return if the setting key is not defined.</param>
    /// <returns>The value of the setting.</returns>
    public virtual T? Get<T>(string settingKey, T? defaultValue)
    {
        if (!OtherSettings.ContainsKey(settingKey))
            return defaultValue;
        return TryGet(settingKey, out T? value) ? value : defaultValue;
    }

#pragma warning disable SA1414 // Tuple types in signatures should have element names
    /// <summary>
    /// Gets a set of specific settings.
    /// </summary>
    /// <typeparam name="T1">The type of the first setting value.</typeparam>
    /// <typeparam name="T2">The type of the second setting value.</typeparam>
    /// <param name="key1">The first setting key to get the value of.</param>
    /// <param name="key2">The second setting key to get the value of.</param>
    /// <returns>A tuple of setting values.</returns>
    public (T1?, T2?) Get<T1, T2>(string key1, string key2)
        => (Get<T1>(key1), Get<T2>(key2));

    /// <summary>
    /// Gets a set of specific settings.
    /// </summary>
    /// <typeparam name="T1">The type of the first setting value.</typeparam>
    /// <typeparam name="T2">The type of the second setting value.</typeparam>
    /// <typeparam name="T3">The type of the third setting value.</typeparam>
    /// <param name="key1">The first setting key to get the value of.</param>
    /// <param name="key2">The second setting key to get the value of.</param>
    /// <param name="key3">The third setting key to get the value of.</param>
    /// <returns>A tuple of setting values.</returns>
    public (T1?, T2?, T3?) Get<T1, T2, T3>(string key1, string key2, string key3)
        => (Get<T1>(key1), Get<T2>(key2), Get<T3>(key3));

    /// <summary>
    /// Gets a set of specific settings.
    /// </summary>
    /// <typeparam name="T1">The type of the first setting value.</typeparam>
    /// <typeparam name="T2">The type of the second setting value.</typeparam>
    /// <typeparam name="T3">The type of the third setting value.</typeparam>
    /// <typeparam name="T4">The type of the fourth setting value.</typeparam>
    /// <param name="key1">The first setting key to get the value of.</param>
    /// <param name="key2">The second setting key to get the value of.</param>
    /// <param name="key3">The third setting key to get the value of.</param>
    /// <param name="key4">The fourth setting key to get the value of.</param>
    /// <returns>A tuple of setting values.</returns>
    public (T1?, T2?, T3?, T4?) Get<T1, T2, T3, T4>(string key1, string key2, string key3, string key4)
        => (Get<T1>(key1), Get<T2>(key2), Get<T3>(key3), Get<T4>(key4));

    /// <summary>
    /// Gets a set of specific settings.
    /// </summary>
    /// <typeparam name="T1">The type of the first setting value.</typeparam>
    /// <typeparam name="T2">The type of the second setting value.</typeparam>
    /// <typeparam name="T3">The type of the third setting value.</typeparam>
    /// <typeparam name="T4">The type of the fourth setting value.</typeparam>
    /// <typeparam name="T5">The type of the fifth setting value.</typeparam>
    /// <param name="key1">The first setting key to get the value of.</param>
    /// <param name="key2">The second setting key to get the value of.</param>
    /// <param name="key3">The third setting key to get the value of.</param>
    /// <param name="key4">The fourth setting key to get the value of.</param>
    /// <param name="key5">The fifth setting key to get the value of.</param>
    /// <returns>A tuple of setting values.</returns>
    public (T1?, T2?, T3?, T4?, T5?) Get<T1, T2, T3, T4, T5>(string key1, string key2, string key3, string key4, string key5)
        => (Get<T1>(key1), Get<T2>(key2), Get<T3>(key3), Get<T4>(key4), Get<T5>(key5));

    /// <summary>
    /// Gets a set of specific settings.
    /// </summary>
    /// <typeparam name="T1">The type of the first setting value.</typeparam>
    /// <typeparam name="T2">The type of the second setting value.</typeparam>
    /// <typeparam name="T3">The type of the third setting value.</typeparam>
    /// <typeparam name="T4">The type of the fourth setting value.</typeparam>
    /// <typeparam name="T5">The type of the fifth setting value.</typeparam>
    /// <typeparam name="T6">The type of the sixth setting value.</typeparam>
    /// <param name="key1">The first setting key to get the value of.</param>
    /// <param name="key2">The second setting key to get the value of.</param>
    /// <param name="key3">The third setting key to get the value of.</param>
    /// <param name="key4">The fourth setting key to get the value of.</param>
    /// <param name="key5">The fifth setting key to get the value of.</param>
    /// <param name="key6">The sixth setting key to get the value of.</param>
    /// <returns>A tuple of setting values.</returns>
    public (T1?, T2?, T3?, T4?, T5?, T6?) Get<T1, T2, T3, T4, T5, T6>(string key1, string key2, string key3, string key4, string key5, string key6)
        => (Get<T1>(key1), Get<T2>(key2), Get<T3>(key3), Get<T4>(key4), Get<T5>(key5), Get<T6>(key6));

    /// <summary>
    /// Gets a set of specific settings.
    /// </summary>
    /// <typeparam name="T1">The type of the first setting value.</typeparam>
    /// <typeparam name="T2">The type of the second setting value.</typeparam>
    /// <typeparam name="T3">The type of the third setting value.</typeparam>
    /// <typeparam name="T4">The type of the fourth setting value.</typeparam>
    /// <typeparam name="T5">The type of the fifth setting value.</typeparam>
    /// <typeparam name="T6">The type of the sixth setting value.</typeparam>
    /// <typeparam name="T7">The type of the seventh setting value.</typeparam>
    /// <param name="key1">The first setting key to get the value of.</param>
    /// <param name="key2">The second setting key to get the value of.</param>
    /// <param name="key3">The third setting key to get the value of.</param>
    /// <param name="key4">The fourth setting key to get the value of.</param>
    /// <param name="key5">The fifth setting key to get the value of.</param>
    /// <param name="key6">The sixth setting key to get the value of.</param>
    /// <param name="key7">The seventh setting key to get the value of.</param>
    /// <returns>A tuple of setting values.</returns>
    public (T1?, T2?, T3?, T4?, T5?, T6?, T7?) Get<T1, T2, T3, T4, T5, T6, T7>(string key1, string key2, string key3, string key4, string key5, string key6, string key7)
        => (Get<T1>(key1), Get<T2>(key2), Get<T3>(key3), Get<T4>(key4), Get<T5>(key5), Get<T6>(key6), Get<T7>(key7));
#pragma warning restore SA1414 // Tuple types in signatures should have element names

    /// <summary>
    /// Tries to get a specific setting.
    /// </summary>
    /// <typeparam name="T">The type of the setting value.</typeparam>
    /// <param name="settingKey">The setting key to get the value of.</param>
    /// <param name="value">The value of the setting.</param>
    /// <returns>True if the setting could be retrieved; otherwise false.</returns>
    public virtual bool TryGet<T>(string settingKey, out T? value)
    {
        if (OtherSettings.ContainsKey(settingKey))
        {
            var objValue = this[settingKey];
            if (typeof(T).IsClass && objValue == null)
            {
                value = default;
                return true;
            }

            if (typeof(T).IsInstanceOfType(objValue))
            {
                value = (T?)objValue;
                return true;
            }

            if (_getterFunctions.TryGetValue(typeof(T), out var func))
            {
                value = (T?)func(objValue);
                return true;
            }

            if (typeof(T).IsClass && objValue is JToken token)
            {
                value = token.ToObject<T>();
                return true;
            }
        }

        value = default;
        return false;
    }

#pragma warning disable SA1414 // Tuple types in signatures should have element names
    /// <summary>
    /// Tries to get a set of specific settings.
    /// </summary>
    /// <typeparam name="T1">The type of the first setting value.</typeparam>
    /// <typeparam name="T2">The type of the second setting value.</typeparam>
    /// <param name="key1">The first setting key to get the value of.</param>
    /// <param name="key2">The second setting key to get the value of.</param>
    /// <param name="value">A tuple of setting values.</param>
    /// <returns>True if all the settings could be retrieved; otherwise false.</returns>
    public bool TryGet<T1, T2>(string key1, string key2, out (T1?, T2?) value)
    {
        bool r = true;
        value = (TryGet<T1>(key1, ref r), TryGet<T2>(key2, ref r));
        return r;
    }

    /// <summary>
    /// Tries to get a set of specific settings.
    /// </summary>
    /// <typeparam name="T1">The type of the first setting value.</typeparam>
    /// <typeparam name="T2">The type of the second setting value.</typeparam>
    /// <typeparam name="T3">The type of the third setting value.</typeparam>
    /// <param name="key1">The first setting key to get the value of.</param>
    /// <param name="key2">The second setting key to get the value of.</param>
    /// <param name="key3">The third setting key to get the value of.</param>
    /// <param name="value">A tuple of setting values.</param>
    /// <returns>True if all the settings could be retrieved; otherwise false.</returns>
    public bool TryGet<T1, T2, T3>(string key1, string key2, string key3, out (T1?, T2?, T3?) value)
    {
        bool r = true;
        value = (TryGet<T1>(key1, ref r), TryGet<T2>(key2, ref r), TryGet<T3>(key3, ref r));
        return r;
    }

    /// <summary>
    /// Tries to get a set of specific settings.
    /// </summary>
    /// <typeparam name="T1">The type of the first setting value.</typeparam>
    /// <typeparam name="T2">The type of the second setting value.</typeparam>
    /// <typeparam name="T3">The type of the third setting value.</typeparam>
    /// <typeparam name="T4">The type of the fourth setting value.</typeparam>
    /// <param name="key1">The first setting key to get the value of.</param>
    /// <param name="key2">The second setting key to get the value of.</param>
    /// <param name="key3">The third setting key to get the value of.</param>
    /// <param name="key4">The fourth setting key to get the value of.</param>
    /// <param name="value">A tuple of setting values.</param>
    /// <returns>True if all the settings could be retrieved; otherwise false.</returns>
    public bool TryGet<T1, T2, T3, T4>(string key1, string key2, string key3, string key4, out (T1?, T2?, T3?, T4?) value)
    {
        bool r = true;
        value = (TryGet<T1>(key1, ref r), TryGet<T2>(key2, ref r), TryGet<T3>(key3, ref r), TryGet<T4>(key4, ref r));
        return r;
    }

    /// <summary>
    /// Tries to get a set of specific settings.
    /// </summary>
    /// <typeparam name="T1">The type of the first setting value.</typeparam>
    /// <typeparam name="T2">The type of the second setting value.</typeparam>
    /// <typeparam name="T3">The type of the third setting value.</typeparam>
    /// <typeparam name="T4">The type of the fourth setting value.</typeparam>
    /// <typeparam name="T5">The type of the fifth setting value.</typeparam>
    /// <param name="key1">The first setting key to get the value of.</param>
    /// <param name="key2">The second setting key to get the value of.</param>
    /// <param name="key3">The third setting key to get the value of.</param>
    /// <param name="key4">The fourth setting key to get the value of.</param>
    /// <param name="key5">The fifth setting key to get the value of.</param>
    /// <param name="value">A tuple of setting values.</param>
    /// <returns>True if all the settings could be retrieved; otherwise false.</returns>
    public bool TryGet<T1, T2, T3, T4, T5>(string key1, string key2, string key3, string key4, string key5, out (T1?, T2?, T3?, T4?, T5?) value)
    {
        bool r = true;
        value = (TryGet<T1>(key1, ref r), TryGet<T2>(key2, ref r), TryGet<T3>(key3, ref r), TryGet<T4>(key4, ref r), TryGet<T5>(key5, ref r));
        return r;
    }

    /// <summary>
    /// Tries to get a set of specific settings.
    /// </summary>
    /// <typeparam name="T1">The type of the first setting value.</typeparam>
    /// <typeparam name="T2">The type of the second setting value.</typeparam>
    /// <typeparam name="T3">The type of the third setting value.</typeparam>
    /// <typeparam name="T4">The type of the fourth setting value.</typeparam>
    /// <typeparam name="T5">The type of the fifth setting value.</typeparam>
    /// <typeparam name="T6">The type of the sixth setting value.</typeparam>
    /// <param name="key1">The first setting key to get the value of.</param>
    /// <param name="key2">The second setting key to get the value of.</param>
    /// <param name="key3">The third setting key to get the value of.</param>
    /// <param name="key4">The fourth setting key to get the value of.</param>
    /// <param name="key5">The fifth setting key to get the value of.</param>
    /// <param name="key6">The sixth setting key to get the value of.</param>
    /// <param name="value">A tuple of setting values.</param>
    /// <returns>True if all the settings could be retrieved; otherwise false.</returns>
    public bool TryGet<T1, T2, T3, T4, T5, T6>(string key1, string key2, string key3, string key4, string key5, string key6, out (T1?, T2?, T3?, T4?, T5?, T6?) value)
    {
        bool r = true;
        value = (TryGet<T1>(key1, ref r), TryGet<T2>(key2, ref r), TryGet<T3>(key3, ref r), TryGet<T4>(key4, ref r), TryGet<T5>(key5, ref r), TryGet<T6>(key6, ref r));
        return r;
    }

    /// <summary>
    /// Tries to get a set of specific settings.
    /// </summary>
    /// <typeparam name="T1">The type of the first setting value.</typeparam>
    /// <typeparam name="T2">The type of the second setting value.</typeparam>
    /// <typeparam name="T3">The type of the third setting value.</typeparam>
    /// <typeparam name="T4">The type of the fourth setting value.</typeparam>
    /// <typeparam name="T5">The type of the fifth setting value.</typeparam>
    /// <typeparam name="T6">The type of the sixth setting value.</typeparam>
    /// <typeparam name="T7">The type of the seventh setting value.</typeparam>
    /// <param name="key1">The first setting key to get the value of.</param>
    /// <param name="key2">The second setting key to get the value of.</param>
    /// <param name="key3">The third setting key to get the value of.</param>
    /// <param name="key4">The fourth setting key to get the value of.</param>
    /// <param name="key5">The fifth setting key to get the value of.</param>
    /// <param name="key6">The sixth setting key to get the value of.</param>
    /// <param name="key7">The seventh setting key to get the value of.</param>
    /// <param name="value">A tuple of setting values.</param>
    /// <returns>True if all the settings could be retrieved; otherwise false.</returns>
    public bool TryGet<T1, T2, T3, T4, T5, T6, T7>(string key1, string key2, string key3, string key4, string key5, string key6, string key7, out (T1?, T2?, T3?, T4?, T5?, T6?, T7?) value)
    {
        bool r = true;
        value = (TryGet<T1>(key1, ref r), TryGet<T2>(key2, ref r), TryGet<T3>(key3, ref r), TryGet<T4>(key4, ref r), TryGet<T5>(key5, ref r), TryGet<T6>(key6, ref r), TryGet<T7>(key7, ref r));
        return r;
    }
#pragma warning restore SA1414 // Tuple types in signatures should have element names

    /// <summary>
    /// Gets a specific setting.
    /// </summary>
    /// <param name="settingKey">The setting key to get the value of.</param>
    /// <returns>The value of the setting.</returns>
    public virtual object? this[string settingKey]
    {
        get
        {
            if (OtherSettings.TryGetValue(settingKey, out var r1))
                return r1;
            throw new KeyNotFoundException($"A setting with the key \"{settingKey}\" was not found in the provided settings.");
        }
        set => OtherSettings[settingKey] = value;
    }

    /// <summary>
    /// Determines the correct settings file for a given assembly and loads the settings.
    /// </summary>
    /// <typeparam name="T">The type of the setting class to deserialize.</typeparam>
    /// <param name="testAssembly">The assembly for which to determine the correct setting file.</param>
    /// <param name="forceDefault">Determines wether to forcly load the default setting file (settings.json).</param>
    /// <returns>The loaded settings.</returns>
    public static T GetFromFile<T>(Assembly testAssembly, bool forceDefault)
        where T : SettingsBase
        => GetFromFile<T>(testAssembly, null, forceDefault);

    /// <summary>
    /// Determines the correct settings file for a given assembly and loads the settings.
    /// </summary>
    /// <typeparam name="T">The type of the setting class to deserialize.</typeparam>
    /// <param name="testAssembly">The assembly for which to determine the correct setting file.</param>
    /// <param name="settingsFolder">The path, relative to the directory in which the assembly is located, to the setting files.</param>
    /// <param name="forceDefault">Determines wether to forcly load the default setting file (settings.json).</param>
    /// <returns>The loaded settings.</returns>
    public static T GetFromFile<T>(Assembly testAssembly, string? settingsFolder, bool forceDefault)
        where T : SettingsBase
        => GetFromFile<T>(GetSettingFile(testAssembly, settingsFolder, forceDefault));

    /// <summary>
    /// Gets the correct settings file for the current execution.
    /// </summary>
    /// <param name="testAssembly">The assembly for which to determine the correct setting file.</param>
    /// <param name="settingsFolder">The folder in which settings files should be searched in.</param>
    /// <param name="forceDefault">Determines if the default file should be used (settings.json).</param>
    /// <returns>Returns the path to a settings file.</returns>
    public static string? GetSettingFile(Assembly testAssembly, string? settingsFolder, bool forceDefault)
    {
        var deployDirectory = Path.GetDirectoryName(testAssembly.Location) ?? string.Empty;
        var settingsDirectory = Path.Combine(deployDirectory, settingsFolder ?? "TestSettings");
        var settingsPrefix = testAssembly.GetName().Name + ".";

        var filePath = GetSettingsFile(settingsDirectory, settingsPrefix);
        if (!File.Exists(filePath))
            filePath = GetSettingsFile(settingsDirectory, null);
        if (!File.Exists(filePath))
            filePath = GetSettingsFile(deployDirectory, settingsPrefix);
        if (!File.Exists(filePath))
            filePath = GetSettingsFile(deployDirectory, null);

        return filePath;

        string? GetSettingsFile(string dir, string? prefix)
        {
            string? result = null;
            if (!forceDefault)
            {
                result = Path.Combine(dir, prefix + $"settings_{Environment.MachineName}.json");
                if (!File.Exists(result) && TestHelper.IsDebugConfiguration)
                    result = Path.Combine(dir, prefix + "settings_debug.json");
            }

            if (forceDefault || !File.Exists(result))
                result = Path.Combine(dir, prefix + "settings.json");
            return result;
        }
    }

    /// <summary>
    /// Loads a settings file.
    /// </summary>
    /// <typeparam name="T">The type of the setting class to deserialize.</typeparam>
    /// <param name="filePath">The path to the settings file to load.</param>
    /// <returns>The loaded settings.</returns>
    public static T GetFromFile<T>(string? filePath)
        where T : SettingsBase
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("filePath needs to be a valid file location.", nameof(filePath));

        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Settings file \"{0}\" was not found.");

        var jsonSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
        };
        var result = JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath), jsonSettings) ?? throw new NullReferenceException("The loaded settings are null.");
        return result;
    }

    /// <summary>
    /// Loads the settings from two files that will be merged.
    /// </summary>
    /// <typeparam name="T">The type of the setting class to deserialize.</typeparam>
    /// <param name="filePath">The path to the settings file to load.</param>
    /// <param name="defaultFilePath">The path to the settings file that is used as a fallback.</param>
    /// <returns>The loaded settings.</returns>
    public static T GetFromFiles<T>(string? filePath, string? defaultFilePath)
        where T : SettingsBase
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("filePath needs to be a valid file location.", nameof(filePath));
        if (string.IsNullOrWhiteSpace(defaultFilePath))
            throw new ArgumentException("defaultFilePath needs to be a valid file location.", nameof(defaultFilePath));

        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Settings file \"{filePath}\" was not found.");
        if (!File.Exists(defaultFilePath))
            throw new FileNotFoundException($"Settings file \"{defaultFilePath}\" was not found.");

        var result = JObject.Parse(File.ReadAllText(defaultFilePath));
        RemoveTags(result);
        if (File.Exists(filePath) && filePath != defaultFilePath)
        {
            var overwrite = JObject.Parse(File.ReadAllText(filePath));
            RemoveTags(overwrite);
            result.Merge(overwrite, new JsonMergeSettings
            {
                MergeArrayHandling = MergeArrayHandling.Replace,
                MergeNullValueHandling = MergeNullValueHandling.Merge,
            });
        }

        return result.ToObject<T>() ?? throw new NullReferenceException("The loaded settings are null.");

        static void RemoveTags(JObject obj)
        {
            var osToken = obj[nameof(OtherSettings)];
            if (osToken != null)
            {
                foreach (var property in osToken.Children<JProperty>().ToList())
                {
                    var newName = SettingKeyEqualityComparer.GetKey(property.Name);
                    if (newName != property.Name)
                    {
                        property.AddAfterSelf(new JProperty(newName, property.Value));
                        property.Remove();
                    }
                }
            }
        }
    }

    private T? TryGet<T>(string settingKey, ref bool success)
    {
        success &= TryGet(settingKey, out T? value);
        return value;
    }

    #region Getter functions
    private static readonly Dictionary<Type, Func<object?, object?>> _getterFunctions = new()
    {
        [typeof(object)] = (value) => value,
        [typeof(string)] = (value) => GetString(value),
        [typeof(short)] = (value) => checked((short)GetInteger(value)),
        [typeof(int)] = (value) => checked((int)GetInteger(value)),
        [typeof(long)] = (value) => GetInteger(value),
        [typeof(float)] = (value) => checked((float)GetDouble(value)),
        [typeof(double)] = (value) => GetDouble(value),
        [typeof(bool)] = (value) => GetBool(value),
        [typeof(DateTime)] = (value) => GetDateTime(value),
        [typeof(Uri)] = (value) => GetUri(value),
    };

    private static double GetDouble(object? value)
    {
        double result = 0D;
        if (value is long lValue)
            result = lValue;
        else if (value is double dValue)
            result = dValue;
        else if (value == null)
            result = default;
        else if (value is not null)
            result = double.Parse(GetString(value)!, CultureInfo.InvariantCulture);
        return result;
    }

    private static DateTime GetDateTime(object? value)
    {
        DateTime result = DateTime.MinValue;
        if (value is DateTime dtValue)
            result = dtValue;
        else if (value == null)
            result = default;
        else if (value is not null)
            result = DateTime.Parse(GetString(value)!, CultureInfo.InvariantCulture);
        return result;
    }

    private static string? GetString(object? value)
    {
        if (value is string || value == null)
            return (string?)value;
        else if (value is IFormattable formattable)
            return formattable.ToString(null, CultureInfo.InvariantCulture);
        else
            return value.ToString();
    }

    private static long GetInteger(object? value)
    {
        long result = 0L;
        if (value is long lValue)
            result = lValue;
        else if (value == null)
            result = default;
        else if (value is not null)
            result = long.Parse(GetString(value)!, CultureInfo.InvariantCulture);
        return result;
    }

    private static bool GetBool(object? value)
    {
        bool result = false;
        if (value is bool bValue)
            result = bValue;
        else if (value == null)
            result = default;
        else if (value is long lValue)
            result = lValue != 0;
        else if (value is not null && long.TryParse(GetString(value), out lValue))
            result = lValue != 0;
        else if (value is not null)
            result = bool.Parse(GetString(value)!);
        return result;
    }

    private static Uri? GetUri(object? value)
    {
        var urlString = GetString(value);
        return urlString is null ? null : new Uri(urlString);
    }
    #endregion

    /// <summary>
    /// Represents a settings type.
    /// </summary>
    public class SettingsType
    {
        /// <summary>
        /// Gets or sets the type name.
        /// </summary>
        public string? TypeName { get; set; }

        /// <summary>
        /// Gets or sets the tag name.
        /// </summary>
        public string? TagName { get; set; }
    }
}
