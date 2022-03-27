using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CommonButtonTopScreen : BaseUIScreen
{
    // Start is called before the first frame update
    public override void OnBackButtonPressed()
    {
        //ScreenManager.Instance.ActivateScreen<RegistrationScreen>();
        Deactivate();
    }

    public override void OnButtonPressed()
    {
        string btnName = EventSystem.current.currentSelectedGameObject.name;
        ButtonPressed(btnName);
    }

    private void ButtonPressed(string btnName)
    {
        if (btnName == "Hamburger_Btn")
        {
            //case "Hamburger_Btn":
            //    ScreenManager.Instance.ActivateScreen<HamburgerScreen>();
            //    break;
        }
    }

    private void OnSignIncomplete()
    {
        ScreenManager.Instance.ActivateScreen<SignInScreen>();
        Deactivate();
    }
}
