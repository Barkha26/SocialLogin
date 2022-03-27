using UnityEngine;
using System;

public interface IPopup
{
    /// <summary>
    /// Enables the popup.
    /// </summary>
    void EnablePopup();

    /// <summary>
    /// Disables the popup.
    /// </summary>
    void DisablePopup();

    /// <summary>
    /// Sets the popup.
    /// </summary>
    /// <param name="message">Message.</param>
    /// <param name="submitBtnEnabled">If set to <c>true</c> submit button enabled.</param>
    /// <param name="submit">Submit.</param>
    void SetPopup(string message, bool submitBtnEnabled = true, Action<IPopup> submit = null);

    /// <summary>
    /// Sets the popup.
    /// </summary>
    /// <param name="message">Message.</param>
    /// <param name="submitBtnEnabled">If set to <c>true</c> submit button enabled.</param>
    /// <param name="cancelBtnEnabled">If set to <c>true</c> cancel button enabled.</param>
    /// <param name="submit">Submit.</param>
    /// <param name="cancel">Cancel.</param>
    void SetPopup(string message, bool submitBtnEnabled = true, bool cancelBtnEnabled = true, Action<IPopup> submit = null, Action<IPopup> cancel = null);

    /// <summary>
    /// Sets the popup.
    /// </summary>
    /// <param name="displayTex">Display tex.</param>
    /// <param name="btnText">Button text.</param>
    /// <param name="message">Message.</param>
    /// <param name="disableBtn">If set to <c>true</c> disable button.</param>
    /// <param name="mAction">M action.</param>
    /// <param name="disableCloseButton">If set to <c>true</c> disable close button.</param>
    void SetPopup(Sprite displayTex, string btnText, string message, bool disableBtn, Action<IPopup> mAction, bool disableCloseButton);

    /// <summary>
    /// Raises the cancel button pressed event.
    /// </summary>
    void OnCancelButtonPressed();

    /// <summary>
    /// Raises the submit button pressed event.
    /// </summary>
    void OnSubmitButtonPressed();

    /// <summary>
    /// Gets a value indicating whether this instance is visible.
    /// </summary>
    /// <value><c>true</c> if this instance is visible; otherwise, <c>false</c>.</value>
    bool IsVisible { get; }

}
