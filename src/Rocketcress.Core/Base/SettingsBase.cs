using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Rocketcress.Core.Extensions;
using Rocketcress.Core.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

#nullable disable

namespace Rocketcress.Core.Base
{
    /// <summary>
    /// Base class for settings that are used for test executions.
    /// </summary>
    [Serializable]
    public abstract class SettingsBase
    {
        private static readonly Regex _settingKeyRegex = new Regex(@"(\[(?<Tag>[^\]]*)\])?\s*(?<Key>.*)", RegexOptions.Compiled);

        private LanguageOptions _language;
        private TimeSpan _timeout;

        /// <summary>
        /// Gets or sets the default timeout for the wait operations.
        /// </summary>
        public virtual TimeSpan Timeout
        {
            get => _timeout;
            set
            {
                _timeout = value;
                Wait.DefaultOptions.Timeout = value;
            }
        }

        /// <summary>
        /// Gets or sets the username to login in the tests.
        /// </summary>
        public virtual string Username { get; set; }

        /// <summary>
        /// Gets or sets the password of the username to login in the tests.
        /// </summary>
        public virtual string Password { get; set; }

        /// <summary>
        /// Gets or sets the language which will be set on login.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public virtual LanguageOptions Language
        {
            get => _language;
            set
            {
                _language = value;
                LanguageDependent.DefaultLanguage = (int)_language;
            }
        }

        [NonSerialized]
        private IDictionary<string, object> _otherSettings;

        /// <summary>
        /// Gets the container in which all other settings are saved.
        /// </summary>
        public IDictionary<string, object> OtherSettings
        {
            get => _otherSettings;
            private set => _otherSettings = value;
        }

        /// <summary>
        /// Gets a list of all defined Settings-Type prefixes.
        /// </summary>
        public IList<SettingsType> SettingsTypes { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsBase"/> class.
        /// </summary>
        protected SettingsBase()
        {
            Timeout = TimeSpan.FromMinutes(1);
            Language = LanguageOptions.English;
            OtherSettings = new Dictionary<string, object>();
            SettingsTypes = new List<SettingsType>();
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
        public (T1, T2) Get<T1, T2>(string key1, string key2)
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
        public (T1, T2, T3) Get<T1, T2, T3>(string key1, string key2, string key3)
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
        public (T1, T2, T3, T4) Get<T1, T2, T3, T4>(string key1, string key2, string key3, string key4)
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
        public (T1, T2, T3, T4, T5) Get<T1, T2, T3, T4, T5>(string key1, string key2, string key3, string key4, string key5)
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
        public (T1, T2, T3, T4, T5, T6) Get<T1, T2, T3, T4, T5, T6>(string key1, string key2, string key3, string key4, string key5, string key6)
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
        public (T1, T2, T3, T4, T5, T6, T7) Get<T1, T2, T3, T4, T5, T6, T7>(string key1, string key2, string key3, string key4, string key5, string key6, string key7)
            => (Get<T1>(key1), Get<T2>(key2), Get<T3>(key3), Get<T4>(key4), Get<T5>(key5), Get<T6>(key6), Get<T7>(key7));
#pragma warning restore SA1414 // Tuple types in signatures should have element names

        /// <summary>
        /// Gets a specific setting.
        /// </summary>
        /// <typeparam name="T">The type of the setting value.</typeparam>
        /// <param name="settingKey">The setting key to get the value of.</param>
        /// <returns>The value of the setting.</returns>
        public virtual T Get<T>(string settingKey) => Get<T>(settingKey, default);

        /// <summary>
        /// Gets a specific setting.
        /// </summary>
        /// <typeparam name="T">The type of the setting value.</typeparam>
        /// <param name="settingKey">The setting key to get the value of.</param>
        /// <param name="defaultValue">The value to return if the setting key is not defined.</param>
        /// <returns>The value of the setting.</returns>
        public virtual T Get<T>(string settingKey, T defaultValue)
        {
            if (this[settingKey] == null)
                return defaultValue;
            if (_getterFunctions.TryGetValue(typeof(T), out var func))
                return (T)func(this, settingKey);
            return this[settingKey] is JToken value ? value.ToObject<T>() : defaultValue;
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
        public bool TryGet<T1, T2>(string key1, string key2, out (T1, T2) value)
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
        public bool TryGet<T1, T2, T3>(string key1, string key2, string key3, out (T1, T2, T3) value)
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
        public bool TryGet<T1, T2, T3, T4>(string key1, string key2, string key3, string key4, out (T1, T2, T3, T4) value)
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
        public bool TryGet<T1, T2, T3, T4, T5>(string key1, string key2, string key3, string key4, string key5, out (T1, T2, T3, T4, T5) value)
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
        public bool TryGet<T1, T2, T3, T4, T5, T6>(string key1, string key2, string key3, string key4, string key5, string key6, out (T1, T2, T3, T4, T5, T6) value)
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
        public bool TryGet<T1, T2, T3, T4, T5, T6, T7>(string key1, string key2, string key3, string key4, string key5, string key6, string key7, out (T1, T2, T3, T4, T5, T6, T7) value)
        {
            bool r = true;
            value = (TryGet<T1>(key1, ref r), TryGet<T2>(key2, ref r), TryGet<T3>(key3, ref r), TryGet<T4>(key4, ref r), TryGet<T5>(key5, ref r), TryGet<T6>(key6, ref r), TryGet<T7>(key7, ref r));
            return r;
        }
#pragma warning restore SA1414 // Tuple types in signatures should have element names

        private T TryGet<T>(string settingKey, ref bool success)
        {
            success &= TryGet(settingKey, out T value);
            return value;
        }

        /// <summary>
        /// Tries to get a specific setting.
        /// </summary>
        /// <typeparam name="T">The type of the setting value.</typeparam>
        /// <param name="settingKey">The setting key to get the value of.</param>
        /// <param name="value">The value of the setting.</param>
        /// <returns>True if the setting could be retrieved; otherwise false.</returns>
        public virtual bool TryGet<T>(string settingKey, out T value)
        {
            if (OtherSettings.ContainsKey(settingKey))
            {
                if (_getterFunctions.TryGetValue(typeof(T), out var func))
                {
                    value = (T)func(this, settingKey);
                    return true;
                }

                if (typeof(T).IsClass && this[settingKey] is JToken token && token != null)
                {
                    value = token.ToObject<T>();
                    return true;
                }
            }

            value = default;
            return false;
        }

        /// <summary>
        /// Gets a specific setting.
        /// </summary>
        /// <param name="setting">The setting key to get the value of.</param>
        /// <returns>The value of the setting.</returns>
        public virtual object this[string setting]
        {
            get
            {
                if (OtherSettings.TryGetValue(setting, out var r1))
                    return r1;

                var keyWithoutTag = _settingKeyRegex.Match(setting).Groups["Key"].Value;
                if (OtherSettings.TryFirst(x => _settingKeyRegex.Match(x.Key).Groups["Key"].Value == keyWithoutTag, out var r2))
                    return r2.Value;

                Logger.LogWarning("The settings key \"{0}\" is not available in provided settings.", setting);
                return null;
            }
            set => OtherSettings[setting] = value;
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
        public static T GetFromFile<T>(Assembly testAssembly, string settingsFolder, bool forceDefault)
            where T : SettingsBase
            => GetFromFile<T>(GetSettingFile(testAssembly, settingsFolder, forceDefault));

        /// <summary>
        /// Gets the correct settings file for the current execution.
        /// </summary>
        /// <param name="testAssembly">The assembly for which to determine the correct setting file.</param>
        /// <param name="settingsFolder">The folder in which settings files should be searched in.</param>
        /// <param name="forceDefault">Determines if the default file should be used (settings.json).</param>
        /// <returns>Returns the path to a settings file.</returns>
        public static string GetSettingFile(Assembly testAssembly, string settingsFolder, bool forceDefault)
        {
            var deployDirectory = Path.GetDirectoryName(testAssembly.Location);
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

            string GetSettingsFile(string dir, string prefix)
            {
                string result = null;
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
        public static T GetFromFile<T>(string filePath)
            where T : SettingsBase
        {
            if (!File.Exists(filePath))
            {
                Logger.LogError("Settings file \"{0}\" not found", filePath);
                throw new Exception("The settings were not found. Either override the property 'Settings' or create a file 'settings.json' " +
                    "(and optional 'settings_debug.json') in the project and add the script CopySettingsFiles.ps1 to your projects post build events.\n");
            }

            var jsonSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
            };
            var result = JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath), jsonSettings);
            result.OtherSettings = result.OtherSettings.ToDictionary(x => GetKey(x.Key), x => x.Value);
            return result;
        }

        /// <summary>
        /// Loads the settings from two files that will be merged.
        /// </summary>
        /// <typeparam name="T">The type of the setting class to deserialize.</typeparam>
        /// <param name="filePath">The path to the settings file to load.</param>
        /// <param name="defaultFilePath">The path to the settings file that is used as a fallback.</param>
        /// <returns>The loaded settings.</returns>
        public static T GetFromFiles<T>(string filePath, string defaultFilePath)
            where T : SettingsBase
        {
            if (!File.Exists(filePath) && !File.Exists(defaultFilePath))
            {
                Logger.LogError("Settings files \"{0}\" and \"{1}\" not found", filePath, defaultFilePath);
                throw new Exception("The settings were not found. Either override the property 'Settings' or create a file 'settings.json' " +
                    "(and optional 'settings_debug.json') in the project and add the script CopySettingsFiles.ps1 to your projects post build events.\n");
            }

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

            return result.ToObject<T>();

            static void RemoveTags(JObject obj)
            {
                var osToken = obj[nameof(OtherSettings)];
                if (osToken != null)
                {
                    foreach (var property in osToken.Children<JProperty>().ToList())
                    {
                        var newName = GetKey(property.Name);
                        if (newName != property.Name)
                        {
                            property.AddAfterSelf(new JProperty(newName, property.Value));
                            property.Remove();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves the key without a tag from a key string.
        /// </summary>
        /// <param name="keyWithTag">The key string to extract from.</param>
        /// <returns>Returns a settings key without the tag. E.g. "[str] Test" -> "Test".</returns>
        public static string GetKey(string keyWithTag)
        {
            var g = _settingKeyRegex.Match(keyWithTag).Groups["Key"];
            return g.Success ? g.Value : keyWithTag;
        }

        #region Getter functions
        private static readonly IDictionary<Type, Func<SettingsBase, string, object>> _getterFunctions = new Dictionary<Type, Func<SettingsBase, string, object>>
        {
            [typeof(object)] = (s, key) => s[key],
            [typeof(string)] = (s, key) => GetString(s, key),
            [typeof(short)] = (s, key) => (short)GetInteger(s, key),
            [typeof(int)] = (s, key) => (int)GetInteger(s, key),
            [typeof(long)] = (s, key) => GetInteger(s, key),
            [typeof(float)] = (s, key) => (float)GetDouble(s, key),
            [typeof(double)] = (s, key) => GetDouble(s, key),
            [typeof(bool)] = (s, key) => GetBool(s, key),
            [typeof(DateTime)] = (s, key) => GetDateTime(s, key),
            [typeof(Uri)] = (s, key) => GetUri(s, key),
        };

        private static double GetDouble(SettingsBase settings, string settingKey)
        {
            var value = settings[settingKey];
            double result;
            if (value is long lValue)
                result = lValue;
            else if (value is double dValue)
                result = dValue;
            else if (value == null)
                result = default;
            else
                result = double.Parse(value.ToString(), CultureInfo.InvariantCulture);
            return result;
        }

        private static DateTime GetDateTime(SettingsBase settings, string settingKey)
        {
            var value = settings[settingKey];
            DateTime result;
            if (value is DateTime dtValue)
                result = dtValue;
            else if (value == null)
                result = default;
            else
                result = DateTime.Parse(value.ToString(), CultureInfo.InvariantCulture);
            return result;
        }

        private static string GetString(SettingsBase settings, string settingKey)
        {
            var value = settings[settingKey];
            return value is string || value == null ? (string)value : value.ToString();
        }

        private static long GetInteger(SettingsBase settings, string settingKey)
        {
            var value = settings[settingKey];
            long result;
            if (value is long lValue)
                result = lValue;
            else if (value == null)
                result = default;
            else
                result = long.Parse(value.ToString(), CultureInfo.InvariantCulture);
            return result;
        }

        private static bool GetBool(SettingsBase settings, string settingKey)
        {
            var value = settings[settingKey];
            bool result;
            if (value is bool bValue)
                result = bValue;
            else if (value == null)
                result = default;
            else
                result = bool.Parse(value.ToString());
            return result;
        }

        private static Uri GetUri(SettingsBase settings, string settingKey)
        {
            return new Uri(GetString(settings, settingKey));
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
            public string TypeName { get; set; }

            /// <summary>
            /// Gets or sets the tag name.
            /// </summary>
            public string TagName { get; set; }
        }
    }
}
