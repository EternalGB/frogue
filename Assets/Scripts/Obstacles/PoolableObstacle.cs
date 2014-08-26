using UnityEngine;
using System.Collections;

public class PoolableObstacle : PoolableObject
{

	float recycleOffset = 20;
	public delegate void DestroyHandler(GameObject destroyed);
	public static event DestroyHandler ObjDestroyed;

	void Update()
	{
		if(enabled) {
			if(transform.position.x + recycleOffset < FrogController.Instance.distanceTraveled) {
				Destroy();
			}
		}
	}

	public override void Destroy()
	{
		ObjDestroyed(gameObject);
		base.Destroy();
	}

}

