using UnityEngine;
using System.Collections;

public class Food : PoolablePickup
{

	public float foodValue;

	public override void PickupEffect ()
	{
		FrogController.Instance.foodAmount = Mathf.Clamp
			(FrogController.Instance.foodAmount + foodValue,0,1);
	}

}

