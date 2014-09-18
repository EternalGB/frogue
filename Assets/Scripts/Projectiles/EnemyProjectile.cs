using UnityEngine;
using System.Collections;

public class EnemyProjectile : PoolableProjectile
{

	void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.layer != LayerMask.NameToLayer("Frog"))
			Destroy();
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.layer != LayerMask.NameToLayer("Frog"))
			Destroy();
	}

}

