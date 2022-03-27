using UnityEngine;
using System.Collections;

public class BaseUIScreen : MonoBehaviour, IScreen
{
    /// <summary>
    /// Activate the current screen gameobject.
    /// </summary>
    public void Activate()
    {
        gameObject.SetActive(true);

        OnScreenEnabled();
    }

    /// <summary>
    /// Deactivate the Current screen gameobject.
    /// </summary>
    public void Deactivate()
    {
        gameObject.SetActive(false);
        OnScreenDisabled();
    }

    /// <summary>
    /// Raises the back button pressed event for this screen.
    /// </summary>
    public virtual void OnBackButtonPressed()
    {
        //Debug.Log("Back");
    }

    /// <summary>
    /// Raises the screen enabled event.
    /// </summary>
    public virtual void OnScreenEnabled()
    {
    
    }

    /// <summary>
    /// Raises the screen disabled event.
    /// </summary>
    public virtual void OnScreenDisabled()
    {

    }

    /// <summary>
    /// Raises the button pressed event.
    /// </summary>
    public virtual void OnButtonPressed()
    {

    }

    /// <summary>
    /// Handle the back button pressed event for a screen.
    /// </summary>
    public virtual void DeviceBackButtonPressed()
    {
        
    }

    /// <summary>
    /// Determines whether this screen is enabled.
    /// </summary>
    /// <returns>true</returns>
    /// <c>false</c>
    /// <value><c>true</c> if this instance is screen enabled; otherwise, <c>false</c>.</value>
    public bool IsScreenEnabled
    {
        get{ return gameObject.activeSelf; }
    }
}
