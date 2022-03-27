using UnityEngine;
using System.Collections;

public interface IScreen
{

    /// <summary>
    /// Activate the current screen gameobject.
    /// </summary>
    void Activate();

    /// <summary>
    /// Deactivate the Current screen gameobject.
    /// </summary>
    void Deactivate();

    /// <summary>
    /// Raises the back button pressed event for this screen.
    /// </summary>
    void OnBackButtonPressed();

    /// <summary>
    /// Raises the button pressed event.
    /// </summary>
    void OnButtonPressed();

    /// <summary>
    /// Handle the back button pressed event for a screen.
    /// </summary>
    void DeviceBackButtonPressed();

    /// <summary>
    /// Determines whether this screen is enabled.
    /// </summary>
    /// <returns><c>true</c> if this instance is screen enabled; otherwise, <c>false</c>.</returns>
    bool IsScreenEnabled{ get; }
}
