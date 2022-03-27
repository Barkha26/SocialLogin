using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class UISpriteAtlas : MonoBehaviour
{

	[Header("Sprite Atlas References")]
	[SerializeField]
	private SpriteAtlas spriteAtlas;

	[Header("String References")]
	[SerializeField]
	private string spriteName;

	[Header("Image References")]
	[SerializeField]
	private Image image;

	private void Start()
    {
		Initialize();
    }

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize()
	{
		PrepareSpriteForAtlas();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void PrepareSpriteForAtlas()
    {
		image.sprite = spriteAtlas.GetSprite(spriteName);
		image.preserveAspect = true;
	}

}
