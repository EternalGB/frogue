using UnityEngine;
using System.Collections;

public class PoolablePickup : PoolableObject
{

	public float recycleOffset;




	void Update()
	{
		if(enabled) {
			if(transform.position.x + recycleOffset < FrogController.Instance.distanceTraveled) {
				Destroy();
			}
		}
	}

}

