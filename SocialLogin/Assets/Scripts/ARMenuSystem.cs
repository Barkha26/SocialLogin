using UnityEngine;

public class ARMenuSystem : MonoBehaviour
{

	[SerializeField]
	private ARInitializationSetup arInitializationSetup;

    public void HandleInputData(int value)
	{
		if (value == 0)
			arInitializationSetup.setupType = ARInitializationSetup.SetupType.Wall;
		if (value == 1)
			arInitializationSetup.setupType = ARInitializationSetup.SetupType.Tabletop;

		arInitializationSetup.CheckSetupTypeOrientMarkerContent();
	}

}
