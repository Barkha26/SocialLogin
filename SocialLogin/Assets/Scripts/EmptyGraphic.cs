using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EmptyGraphic : Graphic
{
	[System.Obsolete]
	protected override void OnFillVBO(List<UIVertex> vbo)
    {
    }
}