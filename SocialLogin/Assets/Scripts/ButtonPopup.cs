using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPopup : MonoBehaviour
{
    public GameObject popup;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OpenPopup()
    {
        bool isActive = popup.activeSelf;
        popup.SetActive(!isActive);

    }
}
