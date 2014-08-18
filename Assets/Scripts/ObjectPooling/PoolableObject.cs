using UnityEngine;
using System.Collections;

public abstract class PoolableObject : MonoBehaviour
{



	public void Destroy()
	{
		gameObject.SetActive (false);
	}



}

