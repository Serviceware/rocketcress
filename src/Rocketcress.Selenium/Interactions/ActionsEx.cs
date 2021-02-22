using Rocketcress.Core.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;

namespace Rocketcress.Selenium.Interactions
{
    /// <summary>
    /// Provides an extensible mechanism for building advanced interactions with the browser.
    /// </summary>
    public class ActionsEx : Actions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionsEx"/> class.
        /// </summary>
        /// <param name="driver">The driver on which the actions will be performed.</param>
        public ActionsEx(IWebDriver driver) : base(driver)
        {
        }

        /// <summary>
        /// Clicks the mouse at the last known mouse coordinates.
        /// </summary>
        /// <returns>A self-reference to this <see cref="ActionsEx"/>.</returns>
        public new ActionsEx Click() { base.Click(); return this; }
        /// <summary>
        /// Clicks the mouse on the specified element.
        /// </summary>
        /// <param name="onElement">The element on which to click.</param>
        /// <returns>A self-reference to this <see cref="ActionsEx"/>.</returns>
        public new ActionsEx Click(IWebElement onElement) { base.Click(onElement); return this; }
        /// <summary>
        /// Clicks and holds the mouse button at the last known mouse coordinates.
        /// </summary>
        /// <returns>A self-reference to this <see cref="ActionsEx"/>.</returns>
        public new ActionsEx ClickAndHold() { base.ClickAndHold(); return this; }
        /// <summary>
        /// Clicks and holds the mouse button down on the specified element.
        /// </summary>
        /// <param name="onElement">The element on which to click and hold.</param>
        /// <returns>A self-reference to this <see cref="ActionsEx"/>.</returns>
        public new ActionsEx ClickAndHold(IWebElement onElement) { base.ClickAndHold(onElement); return this; }
        /// <summary>
        /// Right-clicks the mouse at the last known mouse coordinates.
        /// </summary>
        /// <returns>A self-reference to this <see cref="ActionsEx"/>.</returns>
        public new ActionsEx ContextClick() { base.ContextClick(); return this; }
        /// <summary>
        /// Right-clicks the mouse on the specified element.
        /// </summary>
        /// <param name="onElement">The element on which to right-click.</param>
        /// <returns>A self-reference to this <see cref="ActionsEx"/>.</returns>
        public new ActionsEx ContextClick(IWebElement onElement) { base.ContextClick(onElement); return this; }
        /// <summary>
        /// Double-clicks the mouse at the last known mouse coordinates.
        /// </summary>
        /// <returns>A self-reference to this <see cref="ActionsEx"/>.</returns>
        public new ActionsEx DoubleClick() { base.DoubleClick(); return this; }
        /// <summary>
        /// Double-clicks the mouse on the specified element.
        /// </summary>
        /// <param name="onElement">The element on which to double-click.</param>
        /// <returns>A self-reference to this <see cref="ActionsEx"/>.</returns>
        public new ActionsEx DoubleClick(IWebElement onElement) { base.DoubleClick(onElement); return this; }
        /// <summary>
        /// Performs a drag-and-drop operation from one element to another.
        /// </summary>
        /// <param name="source">The element on which the drag operation is started.</param>
        /// <param name="target">The element on which the drop is performed.</param>
        /// <returns>A self-reference to this <see cref="ActionsEx"/>.</returns>
        public new ActionsEx DragAndDrop(IWebElement source, IWebElement target) { base.DragAndDrop(source, target); return this; }
        /// <summary>
        /// Performs a drag-and-drop operation on one element to a specified offset.
        /// </summary>
        /// <param name="source">The element on which the drag operation is started.</param>
        /// <param name="offsetX">The horizontal offset to which to move the mouse.</param>
        /// <param name="offsetY">The vertical offset to which to move the mouse.</param>
        /// <returns>A self-reference to this <see cref="ActionsEx"/>.</returns>
        public new ActionsEx DragAndDropToOffset(IWebElement source, int offsetX, int offsetY) { base.DragAndDropToOffset(source, offsetX, offsetY); return this; }
        /// <summary>
        /// Sends a modifier key down message to the browser.
        /// </summary>
        /// <param name="theKey">The key to be sent.</param>
        /// <returns>A self-reference to this <see cref="ActionsEx"/>.</returns>
        public new ActionsEx KeyDown(string theKey) { base.KeyDown(theKey); return this; }
        /// <summary>
        /// Sends a modifier key down message to the specified element in the browser.
        /// </summary>
        /// <param name="element">The element to which to send the key command.</param>
        /// <param name="theKey">The key to be sent.</param>
        /// <returns>A self-reference to this <see cref="ActionsEx"/>.</returns>
        public new ActionsEx KeyDown(IWebElement element, string theKey) { base.KeyDown(element, theKey); return this; }
        /// <summary>
        /// Sends multiple modifier key down messages to the browser.
        /// </summary>
        /// <param name="theKeys">The keys to be sent.</param>
        /// <returns>A self-reference to this <see cref="ActionsEx"/>.</returns>
        public ActionsEx KeysDown(params string[] theKeys) { theKeys.ForEach(x => base.KeyDown(x)); return this; }
        /// <summary>
        /// Sends multiple modifier key down messages to the specified element in the browser.
        /// </summary>
        /// <param name="element">The element to which to send the key command.</param>
        /// <param name="theKeys">The keys to be sent.</param>
        /// <returns>A self-reference to this <see cref="ActionsEx"/>.</returns>
        public ActionsEx KeysDown(IWebElement element, params string[] theKeys) { theKeys.ForEach(x => base.KeyDown(element, x)); return this; }
        /// <summary>
        /// Sends a modifier key up message to the browser.
        /// </summary>
        /// <param name="theKey">The key to be sent.</param>
        /// <returns>A self-reference to this <see cref="ActionsEx"/>.</returns>
        public new ActionsEx KeyUp(string theKey) { base.KeyUp(theKey); return this; }
        /// <summary>
        /// Sends a modifier up down message to the specified element in the browser.
        /// </summary>
        /// <param name="element">The element to which to send the key command.</param>
        /// <param name="theKey">The key to be sent.</param>
        /// <returns>A self-reference to this <see cref="ActionsEx"/>.</returns>
        public new ActionsEx KeyUp(IWebElement element, string theKey) { base.KeyUp(element, theKey); return this; }
        /// <summary>
        /// Sends multiple modifier key up messages to the browser.
        /// </summary>
        /// <param name="theKeys">The keys to be sent.</param>
        /// <returns>A self-reference to this <see cref="ActionsEx"/>.</returns>
        public ActionsEx KeysUp(params string[] theKeys) { theKeys.ForEach(x => base.KeyUp(x)); return this; }
        /// <summary>
        /// Sends multiple modifier up down messages to the specified element in the browser.
        /// </summary>
        /// <param name="element">The element to which to send the key command.</param>
        /// <param name="theKeys">The keys to be sent.</param>
        /// <returns>A self-reference to this <see cref="ActionsEx"/>.</returns>
        public ActionsEx KeysUp(IWebElement element, params string[] theKeys) { theKeys.ForEach(x => base.KeyUp(element, x)); return this; }
        /// <summary>
        /// Moves the mouse to the specified offset of the last known mouse coordinates.
        /// </summary>
        /// <param name="offsetX">The horizontal offset to which to move the mouse.</param>
        /// <param name="offsetY">The vertical offset to which to move the mouse.</param>
        /// <returns>A self-reference to this <see cref="ActionsEx"/>.</returns>
        public new ActionsEx MoveByOffset(int offsetX, int offsetY) { base.MoveByOffset(offsetX, offsetY); return this; }
        /// <summary>
        /// Moves the mouse to the specified element.
        /// </summary>
        /// <param name="toElement">The element to which to move the mouse.</param>
        /// <returns>A self-reference to this <see cref="ActionsEx"/>.</returns>
        public new ActionsEx MoveToElement(IWebElement toElement) { base.MoveToElement(toElement); return this; }
        /// <summary>
        /// Moves the mouse to the specified offset of the top-left corner of the specified element.
        /// </summary>
        /// <param name="toElement">The element to which to move the mouse.</param>
        /// <param name="offsetX">The horizontal offset to which to move the mouse.</param>
        /// <param name="offsetY">The vertical offset to which to move the mouse.</param>
        /// <returns>A self-reference to this <see cref="ActionsEx"/>.</returns>
        public new ActionsEx MoveToElement(IWebElement toElement, int offsetX, int offsetY) { base.MoveToElement(toElement, offsetX, offsetY); return this; }
        /// <summary>
        /// Moves the mouse to the specified offset of the top-left corner of the specified element.
        /// </summary>
        /// <param name="toElement">The element to which to move the mouse.</param>
        /// <param name="offsetX">The horizontal offset to which to move the mouse.</param>
        /// <param name="offsetY">The vertical offset to which to move the mouse.</param>
        /// <param name="offsetOrigin">The origin for the offset values.</param>
        /// <returns>A self-reference to this <see cref="ActionsEx"/>.</returns>
        public new ActionsEx MoveToElement(IWebElement toElement, int offsetX, int offsetY, MoveToElementOffsetOrigin offsetOrigin) { base.MoveToElement(toElement, offsetX, offsetY, offsetOrigin); return this; }
        /// <summary>
        /// Releases the mouse button at the last known mouse coordinates.
        /// </summary>
        /// <returns>A self-reference to this <see cref="ActionsEx"/>.</returns>
        public new ActionsEx Release() { base.Release(); return this; }
        /// <summary>
        /// Releases the mouse button on the specified element.
        /// </summary>
        /// <param name="onElement">The element on which to release the button.</param>
        /// <returns>A self-reference to this <see cref="ActionsEx"/>.</returns>
        public new ActionsEx Release(IWebElement onElement) { base.Release(onElement); return this; }
        /// <summary>
        /// Sends a sequence of keystrokes to the browser.
        /// </summary>
        /// <param name="keysToSend">The keystrokes to send to the browser.</param>
        /// <returns>A self-reference to this <see cref="ActionsEx"/>.</returns>
        public new ActionsEx SendKeys(string keysToSend) { base.SendKeys(keysToSend); return this; }
        /// <summary>
        /// Sends a sequence of keystrokes to the specified element in the browser.
        /// </summary>
        /// <param name="element">The element to which to send the keystrokes.</param>
        /// <param name="keysToSend">The keystrokes to send to the browser.</param>
        /// <returns>A self-reference to this <see cref="ActionsEx"/>.</returns>
        public new ActionsEx SendKeys(IWebElement element, string keysToSend) { base.SendKeys(element, keysToSend); return this; }

        /// <summary>
        /// Executes an action while a specified set of keys are pressed.
        /// </summary>
        /// <param name="action">The action to execute while keys are pressed.</param>
        /// <param name="keys">The keys to press.</param>
        /// <returns>A self-reference to this <see cref="ActionsEx"/>.</returns>
        public ActionsEx WhileKeysPressed(Action<ActionsEx> action, params string[] keys)
        {
            KeysDown(keys);
            action(this);
            KeysUp(keys);
            return this;
        }
        /// <summary>
        /// Executes an action while a specified set of keys are pressed on a specified element..
        /// </summary>
        /// <param name="element">The element to which to send the key commands.</param>
        /// <param name="action">The action to execute while keys are pressed.</param>
        /// <param name="keys">The keys to press.</param>
        /// <returns>A self-reference to this <see cref="ActionsEx"/>.</returns>
        public ActionsEx WhileKeysPressed(IWebElement element, Action<ActionsEx> action, params string[] keys)
        {
            KeysDown(element, keys);
            action(this);
            KeysUp(element, keys);
            return this;
        }
    }
}
