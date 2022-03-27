using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class ImageDownloader : MonoBehaviour
{
    private RawImage image;
    private void Start()
    {
        image = GetComponent<RawImage>();
    }
    public IEnumerator CallAPITextureType(string url)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();

            if (request.error != null)
                Debug.Log(request.error);
            else
            {
                Texture myTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                ObjectManager.Triggerevent(ObjectEventVariables.responseRecieved);
                image.texture = myTexture;
                //image.SetNativeSize();

                image.rectTransform.anchorMin = new Vector2(0, 0);
                image.rectTransform.anchorMax = new Vector2(1, 1);
                image.rectTransform.pivot = new Vector2(0.5f, 0.5f);

            }
        }
    }
}
