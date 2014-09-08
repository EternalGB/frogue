using UnityEngine;
using System.Collections;

public abstract class PoolablePickup : PoolableObject
{

	float recycleOffset = 25;


	public abstract void ApplyEffect();

	void Update()
	{
		if(enabled) {
			if(transform.position.x + recycleOffset < FrogController.Instance.distanceTraveled) {
				Debug.Log(name + " pickup being destroyed");
				Destroy();
			}
		}
	}

}

