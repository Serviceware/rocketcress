using Rocketcress.Core;
using Rocketcress.Core.WindowsApiTypes;
using Rocketcress.UIAutomation.Controls;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Input;

namespace Rocketcress.UIAutomation.Interaction
{
    public static class Keyboard
    {
        private static readonly Regex SendKeysEscapeRegex = new Regex("[+^%~(){}]", RegexOptions.Compiled);

        #region PressModifierKeys
        public static void PressModifierKeys(ModifierKeys modifierKeys) => PressModifierKeysImplementation(null, modifierKeys);
        public static void PressModifierKeys(UITestControl control, ModifierKeys modifierKeys) => PressModifierKeysImplementation(control, modifierKeys);
        private static void PressModifierKeysImplementation(UITestControl control, ModifierKeys modifierKeys)
        {
            control?.SetFocus();
            foreach (var key in GetModifierKeys(modifierKeys))
                WindowsApiHelper.keybd_event(key, 0, KeyboardEvent.KeyDown, 0);
        }
        #endregion

        #region ReleaseModifierKeys
        public static void ReleaseModifierKeys(ModifierKeys modifierKeys) => ReleaseModifierKeysImplementation(null, modifierKeys);
        public static void ReleaseModifierKeys(UITestControl control, ModifierKeys modifierKeys) => ReleaseModifierKeysImplementation(control, modifierKeys);
        private static void ReleaseModifierKeysImplementation(UITestControl control, ModifierKeys modifierKeys)
        {
            control?.SetFocus();
            foreach (var key in GetModifierKeys(modifierKeys))
                WindowsApiHelper.keybd_event(key, 0, KeyboardEvent.KeyUp, 0);
        }
        #endregion

        #region SendKeys
        public static void SendKeys(UITestControl control, string text, ModifierKeys modifierKeys, bool escapeString) => SendKeysImplementation(control, text, modifierKeys, escapeString);
        public static void SendKeys(string text, ModifierKeys modifierKeys, bool escapeString) => SendKeysImplementation(null, text, modifierKeys, escapeString);
        public static void SendKeys(UITestControl control, string text, ModifierKeys modifierKeys) => SendKeysImplementation(control, text, modifierKeys, false);
        public static void SendKeys(string text, ModifierKeys modifierKeys) => SendKeysImplementation(null, text, modifierKeys, false);
        public static void SendKeys(UITestControl control, string text, bool escapeString) => SendKeysImplementation(control, text, ModifierKeys.None, escapeString);
        public static void SendKeys(string text, bool escapeString) => SendKeysImplementation(null, text, ModifierKeys.None, escapeString);
        public static void SendKeys(UITestControl control, string text) => SendKeysImplementation(control, text, ModifierKeys.None, false);
        public static void SendKeys(string text) => SendKeysImplementation(null, text, ModifierKeys.None, false);
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
        #endregion

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
}
