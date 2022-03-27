using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class PopupScreen : MonoBehaviour, IPopup
{
    public Image image;

    public Text titleLbl;
    public Text messageLbl;

    public Button yesBtn;
    private Action<IPopup> submitEvent;

    public Button noBtn;
    private Action<IPopup> cancelEvent;

    /// <summary>
    /// Enables the popup.
    /// </summary>
    public void EnablePopup()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Disables the popup.
    /// </summary>
    public void DisablePopup()
    {
        gameObject.SetActive(false);

        this.submitEvent = null;
        this.cancelEvent = null;
        if (titleLbl != null)
            titleLbl.text = string.Empty;
        if (messageLbl != null)
            messageLbl.text = string.Empty;
    }

    /// <summary>
    /// Sets the popup.
    /// </summary>
    /// <param name="message">Message.</param>
    /// <param name="submitBtnEnabled">If set to <c>true</c> submit button enabled.</param>
    /// <param name="submit">Submit.</param>
    public void SetPopup(string message, bool submitBtnEnabled = true, Action<IPopup> submit = null)
    {
        if (messageLbl)
        {
            messageLbl.text = message;
        }

        if (yesBtn)
        {
            yesBtn.gameObject.SetActive(submitBtnEnabled);
        }
        EnablePopup();

        this.submitEvent = submit;
    }

    /// <summary>
    /// Sets the popup.
    /// </summary>
    /// <param name="message">Message.</param>
    /// <param name="submitBtnEnabled">If set to <c>true</c> submit button enabled.</param>
    /// <param name="cancelBtnEnabled">If set to <c>true</c> cancel button enabled.</param>
    /// <param name="submit">Submit.</param>
    /// <param name="cancel">Cancel.</param>
    public void SetPopup(string message, bool submitBtnEnabled = true, bool cancelBtnEnabled = true, Action<IPopup> submit = null, Action<IPopup> cancel = null)
    {
        if (messageLbl)
        {
            messageLbl.text = message;
        }

        if (yesBtn)
        {
            yesBtn.gameObject.SetActive(submitBtnEnabled);
        }

        if (noBtn)
        {
            noBtn.gameObject.SetActive(cancelBtnEnabled);
        }

        EnablePopup();
        this.submitEvent = submit;
        this.cancelEvent = cancel;
    }

    /// <summary>
    /// Sets the popup.
    /// </summary>
    /// <param name="heading">Heading.</param>
    /// <param name="message">Message.</param>
    /// <param name="submitBtnEnabled">If set to <c>true</c> submit button enabled.</param>
    /// <param name="submit">Submit.</param>
    public void SetPopup(string heading, string message, bool submitBtnEnabled = true, Action<IPopup> submit = null)
    {
        if (titleLbl)
        {
            titleLbl.text = heading;
        }

        if (messageLbl)
        {
            messageLbl.text = message;
        }

        if (yesBtn)
        {
            yesBtn.gameObject.SetActive(submitBtnEnabled);
        }

        EnablePopup();
        this.submitEvent = submit;
    }

    /// <summary>
    /// Sets the popup.
    /// </summary>
    /// <param name="heading">Heading.</param>
    /// <param name="message">Message.</param>
    /// <param name="submitBtnEnabled">If set to <c>true</c> submit button enabled.</param>
    /// <param name="cancelBtnEnabled">If set to <c>true</c> cancel button enabled.</param>
    /// <param name="submit">Submit.</param>
    /// <param name="cancel">Cancel.</param>
    public void SetPopup(string heading, string message, bool submitBtnEnabled = true, bool cancelBtnEnabled = true, Action<IPopup> submit = null, Action<IPopup> cancel = null)
    {
        if (titleLbl)
        {
            titleLbl.text = heading;
        }

        if (messageLbl)
        {
            messageLbl.text = message;
        }

        if (yesBtn)
        {
            yesBtn.gameObject.SetActive(submitBtnEnabled);
        }

        if (noBtn)
        {
            noBtn.gameObject.SetActive(cancelBtnEnabled);
        }

        EnablePopup();
        this.submitEvent = submit;
        this.cancelEvent = cancel;
    }

    /// <summary>
    /// Sets the popup.
    /// </summary>
    /// <param name="displayTex">Display tex.</param>
    /// <param name="btnText">Button text.</param>
    /// <param name="message">Message.</param>
    /// <param name="disableBtn">If set to <c>true</c> disable button.</param>
    /// <param name="mAction">M action.</param>
    /// <param name="disableCloseButton">If set to <c>true</c> disable close button.</param>
    public void SetPopup(Sprite displayTex, string btnText, string message, bool disableBtn, Action<IPopup> mAction, bool disableCloseButton)
    {

    }

    public void SetPopup(string message, Sprite sprite, bool submitBtnEnabled = true, Action<IPopup> submit = null)
    {

        if (titleLbl)
        {
            titleLbl.text = message;
        }

        if (messageLbl != null)
            messageLbl.text = message;


        if (image != null)
        {
            image.gameObject.SetActive(true);
            image.sprite = sprite;
        }

        if (yesBtn)
        {
            yesBtn.gameObject.SetActive(submitBtnEnabled);
        }

        EnablePopup();

        this.submitEvent = submit;
    }

    public void SetPopup(string message, string spineAnimName, bool isAnimRequired, bool submitBtnEnabled = true, Action<IPopup> submit = null)
    {
        Debug.Log("Setpopup Called for Animation");

        if (titleLbl)
        {
            titleLbl.text = message;
        }

        if (messageLbl != null) 
            messageLbl.text = message;

        //if (uIAnimation != null && isAnimRequired)
        //{
        //    image.gameObject.SetActive(false);
        //    uIAnimation.gameObject.SetActive(true);
        //    uIAnimation.PlayAnimation(spineAnimName);
        //}

        if (yesBtn)
        {
            yesBtn.gameObject.SetActive(submitBtnEnabled);
        }

        EnablePopup();

        this.submitEvent = submit;
    }


    public void SetPopup(Sprite sprite, bool submitBtnEnabled = true, Action<IPopup> submit = null)
    {

        if (image != null)
        {
            image.gameObject.SetActive(true);
            image.sprite = sprite;
        }

        if (yesBtn)
        {
            yesBtn.gameObject.SetActive(submitBtnEnabled);
        }

        EnablePopup();

        this.submitEvent = submit;
    }

    /// <summary>
    /// Ons the cancel button pressed.
    /// </summary>
    public void OnCancelButtonPressed()
    {
        if (cancelEvent != null)
        {
            cancelEvent.Invoke(this);
        }
    }

    /// <summary>
    /// Ons the submit button pressed.
    /// </summary>
    public void OnSubmitButtonPressed()
    {
        if (submitEvent != null)
        {
            submitEvent.Invoke(this);
        }
    }

    /// <summary>
    /// Gets a value indicating whether this <see cref="T:PopupScreen"/> is visible.
    /// </summary>
    /// <value><c>true</c> if is visible; otherwise, <c>false</c>.</value>
    public bool IsVisible
    {
        get { return gameObject.activeSelf; }
    }
}
