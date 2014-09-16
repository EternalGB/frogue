using UnityEngine;
using System.Collections;

public class PoolableProjectile : PoolableObject
{

	public float lifetime = 10;

	void OnEnable()
	{
		StartCoroutine(Timers.Countdown(lifetime,Destroy));
	}

}

