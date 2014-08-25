using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class PoolableSprite : PoolableObject
{

	float recycleOffset = 40;

	void Update()
	{
		//recycle the platform if we go too far
		if(FrogController.Instance.distanceTraveled > transform.position.x + recycleOffset)
			Destroy();
	}

	void Destroy()
	{
		transform.parent = null;
		gameObject.SetActive (false);
	}

}

