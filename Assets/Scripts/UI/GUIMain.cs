using UnityEngine;
using System.Collections;

public class GUIMain : MonoBehaviour
{

	public GUISkin defaultSkin;
	public Texture hungerBarBack;
	public Texture hungerBarFront;

	float origWidth = 1920;
	float origHeight = 1080;
	Vector3 scale;

	bool showDeathScreen = false;
	float metersTraveled;

	void Start()
	{
		FrogController.Die += HandlePlayerDeath;
	}

	void OnGUI()
	{
		scale.x = Screen.width/origWidth;
		scale.y = Screen.height/origHeight;
		scale.z = 1;
		Matrix4x4 lastMat = GUI.matrix;
		GUI.matrix = Matrix4x4.TRS (Vector3.zero,Quaternion.identity,scale);
		GUISkin unityDef = GUI.skin;
		GUI.skin = defaultSkin;
		if(showDeathScreen) {
			GUILayout.BeginArea(new Rect(640,270,640,540));
			GUILayout.BeginVertical();

			GUILayout.Label("You hopped:");
			GUILayout.Label(metersTraveled + "m");
			if(GUILayout.Button("Try Again?"))
				Application.LoadLevel(0);
			/*
			if(GUILayout.Button("Quit"))
				Application.Quit();
			*/
			GUILayout.EndVertical();
			GUILayout.EndArea();
		} else {
			GUI.BeginGroup(new Rect(100,1000,1720,70));
			GUI.Label(new Rect(0,0,200,40),"Food");
			GUIUtil.DrawResourceBar(new Rect(0,35,1720,30),hungerBarBack,hungerBarFront,FrogController.Instance.foodAmount);
			GUI.EndGroup ();
		}

		GUI.skin = unityDef;
		GUI.matrix = lastMat;
	}

	void HandlePlayerDeath()
	{
		showDeathScreen = true;
		metersTraveled = FrogController.Instance.distanceTraveled/10;
	}


}

