using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderMenuAnim : MonoBehaviour
{
    public GameObject panelMenu;
    public GameObject disablePanelBtn;

    public void ShowHideMenu()
    {
        if (panelMenu != null)
        {
            Animator animator = panelMenu.GetComponent<Animator>();
            if(animator!=null)
            {
                bool isOpen = animator.GetBool("show");
                animator.SetBool("show", !isOpen);
            }
        }
    }
    public void DisableOverlayBtn()
    {
        disablePanelBtn.SetActive(false);
    }

    public void EnableOverlayBtn()
    {
        disablePanelBtn.SetActive(true);

    }

}
