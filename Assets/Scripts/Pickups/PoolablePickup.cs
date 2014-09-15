using UnityEngine;
using System.Collections;

public abstract class PoolablePickup : PoolableObject
{

	float recycleOffset = 25;
	public float pointValue;

	public void ApplyEffect()
	{
		FrogController.Instance.score += pointValue;
		PickupEffect();
	}

	public abstract void PickupEffect();

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

