using UnityEngine;

public class ARInitializationSetup : MonoBehaviour
{

    public enum SetupType { Wall, Tabletop };
    public SetupType setupType = SetupType.Wall;

    [SerializeField]
    private Transform imageTarget;
    [SerializeField]
    private Transform props;
    [SerializeField]
    private Transform vfxWorld;
    [SerializeField]
    private Transform world;

	private void Awake()
	{
        CheckSetupTypeOrientMarkerContent();
    }

	public void CheckSetupTypeOrientMarkerContent()
	{
        switch (setupType)
		{
            case SetupType.Wall:

                if (props.eulerAngles == new Vector3(0f, 0f, 0f))
				{
                    props.SetParent(world, true);
                    vfxWorld.SetParent(world, true);

                    props.localPosition = new Vector3(0f, 0f, -4f);
                    vfxWorld.localPosition = new Vector3(0f, 0f, -4f);

                    imageTarget.eulerAngles = new Vector3(-90f, 0f, 0f);

                    props.SetParent(imageTarget, true);
                    vfxWorld.SetParent(imageTarget, true);

                    imageTarget.eulerAngles = new Vector3(0f, 0f, 0f);
                }

                break;

            case SetupType.Tabletop:

                if (props.eulerAngles == new Vector3(90f, 0f, 0f))
				{
                    imageTarget.eulerAngles = new Vector3(-90f, 0f, 0f);

                    props.SetParent(world, true);
                    vfxWorld.SetParent(world, true);

                    imageTarget.eulerAngles = new Vector3(0f, 0f, 0f);

                    props.position = Vector3.zero;
                    vfxWorld.position = Vector3.zero;

                    props.SetParent(imageTarget, true);
                    vfxWorld.SetParent(imageTarget, true);
                }
                
                break;
		}
	}

}
