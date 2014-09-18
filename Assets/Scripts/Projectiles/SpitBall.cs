using UnityEngine;
using System.Collections;

public class SpitBall : PoolableProjectile
{

	public LayerMask destructionMask;

	void OnTriggerEnter2D(Collider2D col)
	{
		Debug.Log ("Spit ball hit " + col.gameObject.name);
		if(col.gameObject.GetComponent<PoolableObstacle>()) {
			Debug.Log("Spit ball disabling " + col.gameObject.GetInstanceID());
			col.gameObject.SendMessage("DisableObstacle",5);
		}

		if(Util.InLayerMask(destructionMask,col.gameObject.layer))
			Destroy();
	}

}

