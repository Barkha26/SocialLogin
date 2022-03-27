using UnityEngine;
using UnityEngine.UI;

public class InstructionManual : MonoBehaviour
{

    [SerializeField]
    private Text instructions;

    [SerializeField]
    private GameObject[] beforeStateCoachmarks;
    [SerializeField]
    private GameObject[] afterStateCoachmarks;

    private void Start()
    {
        ShowInstructions("<color=red>Step 1:</color> P - Pull the pin.");
        HideAllCoachMarks();
        ShowAfterCoachMark(0);
    }

    public void ShowInstructions(string message)
	{
        instructions.text = message;
	}

    private void HideAllCoachMarks()
	{
        for (int i = 0; i < afterStateCoachmarks.Length; i++)
		{
            afterStateCoachmarks[i].SetActive(false);
		}
	}

    public void ShowAfterCoachMark(int id)
	{
        afterStateCoachmarks[id].SetActive(true);
	}

    public void HideBeforeCoachMark(int id)
	{
        beforeStateCoachmarks[id].SetActive(false);
    }

}
