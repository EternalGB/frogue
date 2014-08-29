using UnityEngine;
using System.Collections;

public class PoolableProjectile : PoolableObject
{

	float lifetime = 10;

	void OnEnable()
	{
		StartCoroutine(Timers.Countdown(lifetime,Destroy));
	}


	void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.layer != LayerMask.NameToLayer("Frog"))
			Destroy();
	}


}

