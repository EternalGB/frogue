using UnityEngine;
using System.Collections;

public class FrogSelector : MonoBehaviour
{

	public GameObject frog;

	void Start()
	{
		DontDestroyOnLoad(this);
	}

	void OnLevelWasLoaded()
	{
		if(Application.loadedLevelName == "main") {
			GameObject.Instantiate(frog,new Vector3(0,2),Quaternion.identity);
		}
	}

}

