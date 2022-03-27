using UnityEngine;
using UnityEngine.UI;

public class MyCertificateObject : MonoBehaviour
{
    [SerializeField]private Button share_Btn;
    public RawImage certificateImage;
    public Text courseName;

    [Header("Place sharing texts here")]
    [SerializeField] private string heading;
    [SerializeField] private string subject;
    [SerializeField] private string url;


    public void ShareCertificate()
    {
        Texture2D texture = (Texture2D)certificateImage.texture;

        new NativeShare().AddFile(texture, "Image.png")
                   .SetSubject(heading).SetText(subject).SetUrl(url)
                   .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
                   .Share();
    }
}
