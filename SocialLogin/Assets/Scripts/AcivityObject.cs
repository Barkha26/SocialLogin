using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AcivityObject : MonoBehaviour
{
    public Button statusButton;
    public GameObject completedSprite;
    public Text subjectHeading;
    public Text dscrText;
    public Image activityPic;
}

public class Activity
{
    public string id;
    public string name;
    public string user_id;
    public string status;
    public string description;
}
