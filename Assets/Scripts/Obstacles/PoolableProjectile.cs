using UnityEngine;
using System.Collections;

public class PoolableProjectile : PoolableObject
{

	void OnCollisionEnter2D(Collision2D col)
	{
		Destroy();
	}

}

