using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CommonButtonBottomScreen : BaseUIScreen
{
    public override void OnBackButtonPressed()
    {
    }

    public override void OnButtonPressed()
    {
        string btn = EventSystem.current.currentSelectedGameObject.name;
        ButtonPressed(btn);
    }

    public void ButtonPressed(string btnName)
    {
        ScreenManager.Instance.DeactivateAllScreens();
        switch (btnName)
        {
            case "Home_Btn":
                ScreenManager.Instance.ActivateScreen<HomeScreen>();
                break;

            case "Engage_Btn":
                ScreenManager.Instance.ActivateScreen<EngageScreen>();
                break;

            case "MyCertificate_Btn":
                ScreenManager.Instance.ActivateScreen<MyLearningScreen>();
                break;
        }
        Activate();
        ScreenManager.Instance.ActivateScreen<CommonButtonTopScreen>();

    }

    public override void OnScreenEnabled()
    {
    }

    public override void OnScreenDisabled()
    {
    }
}
