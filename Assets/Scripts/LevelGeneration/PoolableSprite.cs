using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class PoolableSprite : PoolableObject
{

	float recycleOffset = 30;

	void Update()
	{
		//recycle the sprite if we go too far
		if(FrogController.Instance.distanceTraveled > transform.position.x + recycleOffset) {

			Destroy();
		}
	}

	public override void Destroy()
	{
		transform.parent = null;
		transform.rotation = Quaternion.identity;
		GetComponent<SpriteRenderer>().sortingLayerID = 0;
		base.Destroy();
	}

}

