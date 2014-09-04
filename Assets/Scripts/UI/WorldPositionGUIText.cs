using UnityEngine;
using System.Collections;

public class WorldPositionGUIText : MonoBehaviour
{

	public GUIText text;

	void Update()
	{
		text.transform.position = Camera.main.WorldToViewportPoint(transform.position);
	}

}

