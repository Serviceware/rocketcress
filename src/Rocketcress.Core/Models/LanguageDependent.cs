using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Globalization;
using System.Reflection;

namespace Rocketcress.Core.Models;

/// <summary>
/// This class is just used to store the DefaultLanguage of the LanguageDependent classes.
/// </summary>
public abstract class LanguageDependent
{
    /// <summary>
    /// Gets or sets the default language when resolving the value of a <see cref="LanguageDependent"/>-instance.
    /// </summary>
    public static CultureInfo DefaultLanguage { get; set; }

    static LanguageDependent()
    {
        DefaultLanguage = TestContextBase.CurrentContext?.Settings?.Language
            ?? CultureInfo.GetCultureInfo("en-US");
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LanguageDependent"/> class.
    /// </summary>
    protected LanguageDependent()
    {
    }
}

/// <summary>
/// Represents a value that can be translated to other languages.
/// </summary>
/// <typeparam name="T">The type of elements that are selected by language.</typeparam>
[JsonConverter(typeof(LanguageDependentJsonConverter))]
public class LanguageDependent<T>
{
    /// <summary>
    /// Gets or sets all the stored values for each language id.
    /// </summary>
    protected IDictionary<int, T> Objects { get; set; }

    /// <summary>
    /// Gets the value of the invariant language.
    /// </summary>
    public T? Invariant => this[CultureInfo.InvariantCulture.LCID];

    /// <summary>
    /// Initializes a new instance of the <see cref="LanguageDependent{T}"/> class.
    /// </summary>
    public LanguageDependent()
    {
        Objects = new Dictionary<int, T>();
    }

    #region Public Methods

    /// <summary>
    /// Sets the value for a given language.
    /// </summary>
    /// <param name="language">The id of the language.</param>
    /// <param name="value">The new value.</param>
    /// <returns>Returns the current instance of the LanguageDependent class to chain multiple method calls.</returns>
    public LanguageDependent<T> SetLanguage(int language, T value)
    {
        Objects[language] = value;
        return this;
    }

    /// <summary>
    /// Sets the value for a given language.
    /// </summary>
    /// <param name="language">The language.</param>
    /// <param name="value">The new value.</param>
    /// <returns>Returns the current instance of the LanguageDependent class to chain multiple method calls.</returns>
    public LanguageDependent<T> SetLanguage(CultureInfo language, T value)
    {
        Objects[language.LCID] = value;
        return this;
    }

    /// <summary>
    /// Check if a value for a language exists.
    /// </summary>
    /// <param name="language">The id of the language.</param>
    /// <returns>Returns true if a value for the specified language exists; otherwise false.</returns>
    public bool Contains(int language)
        => Objects.ContainsKey(language);

    /// <summary>
    /// Check if a value for a language exists.
    /// </summary>
    /// <param name="language">The language.</param>
    /// <returns>Returns true if a value for the specified language exists; otherwise false.</returns>
    public bool Contains(CultureInfo language)
        => Objects.ContainsKey(language.LCID);

    #endregion

    #region Indexer

    /// <summary>
    /// Retrieves a value.
    /// </summary>
    /// <param name="language">The id of the language for which the value should be retrieved.</param>
    /// <returns>Returns the value for the specified language.</returns>
    [MaybeNull]
    public T this[int language]
    {
        get
        {
            var l = CultureInfo.GetCultureInfo(language);
            while (!Objects.ContainsKey(l.LCID) && !l.IsNeutralCulture && l.LCID != CultureInfo.InvariantCulture.LCID)
            {
                l = l.Parent;
            }

            return Objects.TryGetValue(l.LCID, out T? t) ? t : Objects.Values.FirstOrDefault();
        }
        set => Objects[language] = value;
    }

    /// <summary>
    /// Retrieves a value.
    /// </summary>
    /// <param name="language">The language for which the value should be retrieved.</param>
    /// <returns>Returns the value for the specified language.</returns>
    [MaybeNull]
    public T this[CultureInfo language]
    {
        get => this[language.LCID];
        set => this[language.LCID] = value;
    }

    #endregion

    /// <summary>
    /// Retrieves the value of the default language.
    /// </summary>
    /// <returns>Returns the value of the default language.</returns>
    public override string ToString()
    {
        return this[LanguageDependent.DefaultLanguage]?.ToString() ?? "(null)";
    }

    /// <summary>
    /// Gets the value for the current language of a <see cref="LanguageDependent{T}"/> object.
    /// </summary>
    /// <param name="s">The <see cref="LanguageDependent{T}"/> object to get the value from.</param>
    public static implicit operator T?(LanguageDependent<T> s)
    {
        return s[LanguageDependent.DefaultLanguage];
    }
}

/// <summary>
/// Provides Extensions for the LanguageDependent class.
/// </summary>
public static class LanguageDependentExtensions
{
    /// <summary>
    /// Sets the value for the english language.
    /// </summary>
    /// <typeparam name="T">The type of element of the LanguageDependent.</typeparam>
    /// <param name="obj">LanguageDependent instance.</param>
    /// <param name="value">The new value.</param>
    /// <returns>Returns the current instance of the LanguageDependent class to chain multiple method calls.</returns>
    public static LanguageDependent<T> SetEnglish<T>(this LanguageDependent<T> obj, T value)
        => obj.SetLanguage(CultureInfo.GetCultureInfo("en"), value);

    /// <summary>
    /// Sets the value for the german language.
    /// </summary>
    /// <typeparam name="T">The type of element of the LanguageDependent.</typeparam>
    /// <param name="obj">LanguageDependent instance.</param>
    /// <param name="value">The new value.</param>
    /// <returns>Returns the current instance of the LanguageDependent class to chain multiple method calls.</returns>
    public static LanguageDependent<T> SetGerman<T>(this LanguageDependent<T> obj, T value)
        => obj.SetLanguage(CultureInfo.GetCultureInfo("de"), value);

    /// <summary>
    /// Sets the value for the french language.
    /// </summary>
    /// <typeparam name="T">The type of element of the LanguageDependent.</typeparam>
    /// <param name="obj">LanguageDependent instance.</param>
    /// <param name="value">The new value.</param>
    /// <returns>Returns the current instance of the LanguageDependent class to chain multiple method calls.</returns>
    public static LanguageDependent<T> SetFrench<T>(this LanguageDependent<T> obj, T value)
        => obj.SetLanguage(CultureInfo.GetCultureInfo("fr"), value);

    /// <summary>
    /// Sets the value for the italian language.
    /// </summary>
    /// <typeparam name="T">The type of element of the LanguageDependent.</typeparam>
    /// <param name="obj">LanguageDependent instance.</param>
    /// <param name="value">The new value.</param>
    /// <returns>Returns the current instance of the LanguageDependent class to chain multiple method calls.</returns>
    public static LanguageDependent<T> SetItalian<T>(this LanguageDependent<T> obj, T value)
        => obj.SetLanguage(CultureInfo.GetCultureInfo("it"), value);

    /// <summary>
    /// Sets the value for a given language.
    /// </summary>
    /// <param name="obj">LanguageDependent instance of type Point.</param>
    /// <param name="language">The language.</param>
    /// <param name="x">The x-coordinate.</param>
    /// <param name="y">The y-coordinate.</param>
    /// <returns>Returns the current instance of the LanguageDependent class to chain multiple method calls.</returns>
    public static LanguageDependent<System.Drawing.Point> SetLanguage(this LanguageDependent<System.Drawing.Point> obj, CultureInfo language, int x, int y)
    {
        return obj.SetLanguage(language, new System.Drawing.Point(x, y));
    }

    /// <summary>
    /// Sets the value for the german language.
    /// </summary>
    /// <param name="obj">LanguageDependent instance of type Point.</param>
    /// <param name="x">The x-coordinate.</param>
    /// <param name="y">The y-coordinate.</param>
    /// <returns>Returns the current instance of the LanguageDependent class to chain multiple method calls.</returns>
    public static LanguageDependent<System.Drawing.Point> SetGerman(this LanguageDependent<System.Drawing.Point> obj, int x, int y)
    {
        return obj.SetLanguage(CultureInfo.GetCultureInfo("de"), new System.Drawing.Point(x, y));
    }

    /// <summary>
    /// Sets the value for the english language.
    /// </summary>
    /// <param name="obj">LanguageDependent instance of type Point.</param>
    /// <param name="x">The x-coordinate.</param>
    /// <param name="y">The y-coordinate.</param>
    /// <returns>Returns the current instance of the LanguageDependent class to chain multiple method calls.</returns>
    public static LanguageDependent<System.Drawing.Point> SetEnglish(this LanguageDependent<System.Drawing.Point> obj, int x, int y)
    {
        return obj.SetLanguage(CultureInfo.GetCultureInfo("en"), new System.Drawing.Point(x, y));
    }

    /// <summary>
    /// Sets the value for the french language.
    /// </summary>
    /// <param name="obj">LanguageDependent instance of type Point.</param>
    /// <param name="x">The x-coordinate.</param>
    /// <param name="y">The y-coordinate.</param>
    /// <returns>Returns the current instance of the LanguageDependent class to chain multiple method calls.</returns>
    public static LanguageDependent<System.Drawing.Point> SetFrench(this LanguageDependent<System.Drawing.Point> obj, int x, int y)
    {
        return obj.SetLanguage(CultureInfo.GetCultureInfo("fr"), new System.Drawing.Point(x, y));
    }

    /// <summary>
    /// Sets the value for the italian language.
    /// </summary>
    /// <param name="obj">LanguageDependent instance of type Point.</param>
    /// <param name="x">The x-coordinate.</param>
    /// <param name="y">The y-coordinate.</param>
    /// <returns>Returns the current instance of the LanguageDependent class to chain multiple method calls.</returns>
    public static LanguageDependent<System.Drawing.Point> SetItalian(this LanguageDependent<System.Drawing.Point> obj, int x, int y)
    {
        return obj.SetLanguage(CultureInfo.GetCultureInfo("it"), new System.Drawing.Point(x, y));
    }
}

/// <summary>
/// Json Converter responsible for the LanguageDependent class.
/// </summary>
public class LanguageDependentJsonConverter : JsonConverter
{
    /// <summary>
    /// Determines whether this instance can convert the specified object type.
    /// </summary>
    /// <param name="objectType">Type of the object.</param>
    /// <returns>true if this instance can convert the specified object type; otherwise, false.</returns>
    public override bool CanConvert(Type objectType)
    {
        return objectType != null && GetSubType(objectType) != null;
    }

    /// <summary>
    /// Reads the JSON representation of the object.
    /// </summary>
    /// <param name="reader">The Newtonsoft.Json.JsonReader to read from.</param>
    /// <param name="objectType">Type of the object.</param>
    /// <param name="existingValue">The existing value of object being read.</param>
    /// <param name="serializer">The calling serializer.</param>
    /// <returns>The object value.</returns>
    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var token = JToken.ReadFrom(reader);
        if (token.Type != JTokenType.Object)
            return null;

        var o = (JObject)token;
        var subType = GetSubType(objectType);
        var dict = Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(typeof(int), subType!))!;

        foreach (var prop in o.Properties())
        {
            ((IDictionary)dict).Add(Convert.ToInt32(prop.Name, CultureInfo.InvariantCulture), prop.Value.ToObject(subType!));
        }

        var result = Activator.CreateInstance(objectType);
        objectType.GetProperty("Objects", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(result, dict);
        return result;
    }

    /// <summary>
    /// Writes the JSON representation of the object.
    /// </summary>
    /// <param name="writer">The Newtonsoft.Json.JsonWriter to write to.</param>
    /// <param name="value">The value.</param>
    /// <param name="serializer">The calling serializer.</param>
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        JToken t;
        if (value == null)
        {
            t = JValue.CreateNull();
        }
        else
        {
            var o = new JObject();
            var dict = (IDictionary)value.GetType().GetProperty("Objects", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(value)!;

            var enumerator = dict.GetEnumerator();
            while (enumerator.MoveNext())
            {
                o.Add(((int)enumerator.Key).ToString(CultureInfo.InvariantCulture), enumerator.Value is null ? JValue.CreateNull() : JToken.FromObject(enumerator.Value));
            }

            t = o;
        }

        t.WriteTo(writer);
    }

    private static Type? GetSubType(Type type)
    {
        var t = type;
        while (t.BaseType != typeof(object))
            t = t.BaseType!;
        if (t?.GetGenericTypeDefinition() == typeof(LanguageDependent<>))
            return t.GetGenericArguments()[0];
        return null;
    }
}

/// <summary>
/// Represents a string that can be translated to other languages.
/// </summary>
[JsonConverter(typeof(LanguageDependentJsonConverter))]
public class MultiLanguageString : LanguageDependent<string>
{
    /// <summary>
    /// Sets the value for a given language.
    /// </summary>
    /// <param name="language">The id of the language.</param>
    /// <param name="value">The new value.</param>
    /// <returns>Returns the current instance of the MultiLanguageString class to chain multiple method calls.</returns>
    public new MultiLanguageString SetLanguage(int language, string value) => (MultiLanguageString)base.SetLanguage(language, value);

    /// <summary>
    /// Sets the value for a given language.
    /// </summary>
    /// <param name="language">The language.</param>
    /// <param name="value">The new value.</param>
    /// <returns>Returns the current instance of the MultiLanguageString class to chain multiple method calls.</returns>
    public new MultiLanguageString SetLanguage(KnownLanguages language, string value) => (MultiLanguageString)base.SetLanguage(language, value);

    /// <summary>
    /// Sets the value for the english language.
    /// </summary>
    /// <param name="value">The new value.</param>
    /// <returns>Returns the current instance of the MultiLanguageString class to chain multiple method calls.</returns>
    public new MultiLanguageString SetEnglish(string value) => SetLanguage(KnownLanguages.English, value);

    /// <summary>
    /// Sets the value for the german language.
    /// </summary>
    /// <param name="value">The new value.</param>
    /// <returns>Returns the current instance of the MultiLanguageString class to chain multiple method calls.</returns>
    public new MultiLanguageString SetGerman(string value) => SetLanguage(KnownLanguages.German, value);

    /// <summary>
    /// Sets the value for the french language.
    /// </summary>
    /// <param name="value">The new value.</param>
    /// <returns>Returns the current instance of the MultiLanguageString class to chain multiple method calls.</returns>
    public new MultiLanguageString SetFrench(string value) => SetLanguage(KnownLanguages.French, value);

    /// <summary>
    /// Sets the value for the italian language.
    /// </summary>
    /// <param name="value">The new value.</param>
    /// <returns>Returns the current instance of the MultiLanguageString class to chain multiple method calls.</returns>
    public new MultiLanguageString SetItalian(string value) => SetLanguage(KnownLanguages.Italian, value);
}
