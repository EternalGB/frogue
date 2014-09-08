using UnityEngine;
using System.Collections;

public class GUIUtil 
{

	public static void DrawResourceBar(Rect backRect, Texture backBar, Texture frontBar, float percentFull)
	{
		GUI.DrawTexture(backRect,backBar,ScaleMode.StretchToFill);
		Rect frontRect = backRect;
		frontRect.width *= percentFull;
		GUI.DrawTexture(frontRect,frontBar,ScaleMode.StretchToFill);
	}

}

