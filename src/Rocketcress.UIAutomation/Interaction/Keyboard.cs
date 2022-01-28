using Rocketcress.Core;
using Rocketcress.Core.WindowsApiTypes;
using Rocketcress.UIAutomation.Controls;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Input;

namespace Rocketcress.UIAutomation.Interaction;

/// <summary>
/// Manages keyboard actions.
/// </summary>
public static class Keyboard
{
    private static readonly Regex SendKeysEscapeRegex = new("[+^%~(){}]", RegexOptions.Compiled);

    /// <summary>
    /// Presses the specified modifier keys.
    /// </summary>
    /// <param name="modifierKeys">The modifier keys.</param>
    public static void PressModifierKeys(ModifierKeys modifierKeys) => PressModifierKeysImplementation(null, modifierKeys);

    /// <summary>
    /// Presses the specified modifier keys.
    /// </summary>
    /// <param name="control">The control to send keys to.</param>
    /// <param name="modifierKeys">The modifier keys.</param>
    public static void PressModifierKeys(UITestControl control, ModifierKeys modifierKeys) => PressModifierKeysImplementation(control, modifierKeys);

    /// <summary>
    /// Releases the specified modifier keys.
    /// </summary>
    /// <param name="modifierKeys">The modifier keys.</param>
    public static void ReleaseModifierKeys(ModifierKeys modifierKeys) => ReleaseModifierKeysImplementation(null, modifierKeys);

    /// <summary>
    /// Releases the specified modifier keys.
    /// </summary>
    /// <param name="control">The control to release keys from.</param>
    /// <param name="modifierKeys">The modifier keys.</param>
    public static void ReleaseModifierKeys(UITestControl control, ModifierKeys modifierKeys) => ReleaseModifierKeysImplementation(control, modifierKeys);

    /// <summary>
    /// Sends the specified keys.
    /// </summary>
    /// <param name="control">The control.</param>
    /// <param name="text">The text.</param>
    /// <param name="modifierKeys">The modifier keys.</param>
    /// <param name="escapeString">If set to <c>true</c> the <paramref name="text"/> string is escaped.</param>
    public static void SendKeys(UITestControl control, string text, ModifierKeys modifierKeys, bool escapeString) => SendKeysImplementation(control, text, modifierKeys, escapeString);

    /// <summary>
    /// Sends the specified keys.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="modifierKeys">The modifier keys.</param>
    /// <param name="escapeString">If set to <c>true</c> the <paramref name="text"/> string is escaped.</param>
    public static void SendKeys(string text, ModifierKeys modifierKeys, bool escapeString) => SendKeysImplementation(null, text, modifierKeys, escapeString);

    /// <summary>
    /// Sends the specified keys.
    /// </summary>
    /// <param name="control">The control.</param>
    /// <param name="text">The text.</param>
    /// <param name="modifierKeys">The modifier keys.</param>
    public static void SendKeys(UITestControl control, string text, ModifierKeys modifierKeys) => SendKeysImplementation(control, text, modifierKeys, false);

    /// <summary>
    /// Sends the specified keys.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="modifierKeys">The modifier keys.</param>
    public static void SendKeys(string text, ModifierKeys modifierKeys) => SendKeysImplementation(null, text, modifierKeys, false);

    /// <summary>
    /// Sends the specified keys.
    /// </summary>
    /// <param name="control">The control.</param>
    /// <param name="text">The text.</param>
    /// <param name="escapeString">If set to <c>true</c> the <paramref name="text"/> string is escaped.</param>
    public static void SendKeys(UITestControl control, string text, bool escapeString) => SendKeysImplementation(control, text, ModifierKeys.None, escapeString);

    /// <summary>
    /// Sends the specified keys.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="escapeString">If set to <c>true</c> the <paramref name="text"/> string is escaped.</param>
    public static void SendKeys(string text, bool escapeString) => SendKeysImplementation(null, text, ModifierKeys.None, escapeString);

    /// <summary>
    /// Sends the specified keys.
    /// </summary>
    /// <param name="control">The control.</param>
    /// <param name="text">The text.</param>
    public static void SendKeys(UITestControl control, string text) => SendKeysImplementation(control, text, ModifierKeys.None, false);

    /// <summary>
    /// Sends the specified keys.
    /// </summary>
    /// <param name="text">The text.</param>
    public static void SendKeys(string text) => SendKeysImplementation(null, text, ModifierKeys.None, false);

    private static void PressModifierKeysImplementation(UITestControl control, ModifierKeys modifierKeys)
    {
        control?.SetFocus();
        foreach (var key in GetModifierKeys(modifierKeys))
            WindowsApiHelper.keybd_event(key, 0, KeyboardEvent.KeyDown, 0);
    }

    private static void ReleaseModifierKeysImplementation(UITestControl control, ModifierKeys modifierKeys)
    {
        control?.SetFocus();
        foreach (var key in GetModifierKeys(modifierKeys))
            WindowsApiHelper.keybd_event(key, 0, KeyboardEvent.KeyUp, 0);
    }

    private static void SendKeysImplementation(UITestControl control, string text, ModifierKeys modifierKeys, bool escapeString)
    {
        string keys = text;
        if (escapeString)
            keys = SendKeysEscapeRegex.Replace(keys, "{$0}");

        control?.SetFocus();
        PressModifierKeysImplementation(null, modifierKeys);
        System.Windows.Forms.SendKeys.SendWait(keys);
        ReleaseModifierKeysImplementation(null, modifierKeys);
    }

    private static IEnumerable<Keys> GetModifierKeys(ModifierKeys modifierKeys)
    {
        if (modifierKeys.HasFlag(ModifierKeys.Alt))
            yield return Keys.Alt;
        if (modifierKeys.HasFlag(ModifierKeys.Control))
            yield return Keys.ControlKey;
        if (modifierKeys.HasFlag(ModifierKeys.Shift))
            yield return Keys.ShiftKey;
        if (modifierKeys.HasFlag(ModifierKeys.Windows))
            yield return Keys.LWin;
    }
}
