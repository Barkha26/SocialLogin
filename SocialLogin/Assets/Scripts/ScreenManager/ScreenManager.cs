using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenManager : MonoBehaviour
{
    //Local reference for initiliasing
    private static ScreenManager _instance;

    public List<BaseUIScreen> uiScreens;

    //Property
    public static ScreenManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ScreenManager>();
            }
            return _instance;
        }
    }

    public List<BaseUIScreen> activatedScreens;
    public List<BaseUIScreen> startUpScreens;

    [Tooltip("You can place your popups too here")]
    public PopupScreen signoutPopup;
    public PopupScreen errorPopups;

    // Use this for initialization
    void Awake()
    {
        DeactivateAllScreens();
    }

    void Start()
    {
        int count = startUpScreens.Count;
        for (int i = 0; i < count; i++)
        {
            ActivateScreen(startUpScreens[i]);
        }
    }


    /// <summary>
    /// Activates the screen.
    /// </summary>
    /// <param name="iScreen">I screen.</param>
    public void ActivateScreen(BaseUIScreen iScreen)
    {
        if (!activatedScreens.Contains((BaseUIScreen)iScreen))
        {
            activatedScreens.Add((BaseUIScreen)iScreen);
        }
        iScreen.Activate();
    }

    /// <summary>
    /// Activates the screen.
    /// </summary>
    public void ActivateScreen<T>() where T : IScreen
    {
        IScreen iScreen = uiScreens.Find(t => t.GetType().Name == typeof(T).Name);
        if (!activatedScreens.Contains((BaseUIScreen)iScreen))
        {
            activatedScreens.Add((BaseUIScreen)iScreen);
        }
        iScreen.Activate();

    }

    /// <summary>
    /// Deactivates the screen.
    /// </summary>
    public void DeactivateScreen<T>() where T : IScreen
    {
        IScreen iScreen = uiScreens.Find(t => t.GetType().Name == typeof(T).Name);
        StartCoroutine(DelayToDeactivate(iScreen));
    }

    //Delay for transistion
    private IEnumerator DelayToDeactivate(IScreen s)
    {
        s.Deactivate();
        yield return null;// new WaitForSeconds(0.0f);
    }

    //Gets the T screen's BaseUIScreen componen
    internal T GetScreen<T>() where T : BaseUIScreen
    {
        return (T)uiScreens.Find(delegate (BaseUIScreen t)
        {
            //Debug.Log(t.gameObject.name + " ##########");
             return (t.GetType().Name == typeof(T).Name);
        });
    }

    //Gets the T screen's BaseUIScreen componen
    internal void RemoveScreen(BaseUIScreen iScreen)
    {
        if (activatedScreens.Count > 0 && activatedScreens.Contains(iScreen))
        {
            activatedScreens.Remove(iScreen);
        }

        //return uiScreens.Find(t => t.GetType().Name == typeof(T).Name);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (activatedScreens.Count > 0)
            {
                activatedScreens[activatedScreens.Count - 1].DeviceBackButtonPressed();
            }
        }
    }

    public void DeactivateAllScreens()
    {
        for (int i = 0; i < uiScreens.Count; i++)
        {
            uiScreens[i].Deactivate();
        }
    }

    public void LogoutGameActions()
    {
        DeactivateAllScreens();
    }

    void OnDestroy()
    {
        _instance = null;
    }
}
