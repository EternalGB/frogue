using UnityEngine;
using System.Collections;

public class PoolablePickup : PoolableObject
{

	public float recycleOffset;

	protected override void Init ()
	{
		
	}
	
	protected override void PreDestroy ()
	{
		
	}
	
	protected override void Reset ()
	{
		
	}

	void Update()
	{
		if(enabled) {
			if(transform.position.x + recycleOffset < FrogController.Instance.distanceTraveled) {
				Destroy();
			}
		}
	}

}

