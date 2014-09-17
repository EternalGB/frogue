using UnityEngine;
using System.Collections.Generic;

public class FrogSelectGUI : MonoBehaviour
{

	public GUISkin defaultSkin;
	public List<GameObject> frogs;
	public FrogSelector selector;

	float origWidth = 1920;
	float origHeight = 1080;
	Vector3 scale;

	float frogIconSize = 200;
	Vector2 frogSelectScrollPos = Vector2.zero;

	void OnGUI()
	{
		scale.x = Screen.width/origWidth;
		scale.y = Screen.height/origHeight;
		scale.z = 1;
		Matrix4x4 lastMat = GUI.matrix;
		GUI.matrix = Matrix4x4.TRS (Vector3.zero,Quaternion.identity,scale);
		GUISkin unityDef = GUI.skin;
		GUI.skin = defaultSkin;

		GUI.Label (new Rect(0,0,1920,200),"SELECT YOUR FROG",GUI.skin.GetStyle("Title"));
		//frogs group
		GUI.BeginGroup(new Rect(20,220,930,840));
		frogSelectScrollPos = GUILayout.BeginScrollView(frogSelectScrollPos,GUILayout.Width(930), GUILayout.Height(840));
		foreach(GameObject frog in frogs) {
			Sprite icon = frog.GetComponent<SpriteRenderer>().sprite;
			GUILayout.BeginHorizontal();
			if(GUILayout.Button(icon.texture, GUILayout.Width (frogIconSize),GUILayout.Height(frogIconSize))) {
				selector.frog = frog;
			}
			GUILayout.Label(FrogDescriptionText.displayNames[frog.name], defaultSkin.GetStyle("Title"));
			GUILayout.EndHorizontal();
		}
		GUILayout.EndScrollView();
		GUI.EndGroup();
		//description group
		GUI.BeginGroup(new Rect(970,220,930,840));
		GUIContent image = new GUIContent(selector.frog.GetComponent<SpriteRenderer>().sprite.texture);
		GUI.Label(new Rect(295,0,340,340),image, defaultSkin.GetStyle("BigFrogDisplay"));
		Color color = Color.black;
		GUILayout.BeginArea(new Rect(0,340,930,400));
		GUILayout.BeginVertical();
		foreach(string prop in FrogDescriptionText.properties[selector.frog.name]) {
			if(prop[0] == '+')
				color = Color.green;
			if(prop[0] == '-')
				color = Color.red;
			defaultSkin.GetStyle("Properties").normal.textColor = color;
			GUILayout.Label(prop,defaultSkin.GetStyle("Properties"),GUILayout.Width(930));
			color = Color.black;
		}
		GUILayout.EndVertical();
		GUILayout.EndArea();
		GUI.contentColor = Color.white;
		if(GUI.Button(new Rect(0,740,930,100),"GO!"))
			Application.LoadLevel("main");
		GUI.EndGroup();

		GUI.skin = unityDef;
		GUI.matrix = lastMat;
	}

}

