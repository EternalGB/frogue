using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class GravZone : PoolableObstacle
{

	public float minGravMagnitude,maxGravMagnitude;
	public Vector2 minSize, maxSize;
	public Material mat;
	public Transform zoneArea;
	BoxCollider2D boxCol;
	float flowSpeed = 0.3f;

	void OnEnable()
	{
		boxCol = (BoxCollider2D)collider2D;
		SetArea(boxCol.center,RandomUtil.RandomVector(minSize,maxSize));
	}

	public void SetArea(Vector2 center, Vector2 size)
	{
		boxCol.center = center;
		boxCol.size = size;
		zoneArea.localScale = size;
		zoneArea.renderer.material.mainTextureScale = size;
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.rigidbody2D != null && !col.rigidbody2D.isKinematic) {
			col.rigidbody2D.gravityScale = 0;
			//col.rigidbody2D.velocity = Vector2.zero;
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if(col.rigidbody2D != null && !col.rigidbody2D.isKinematic) {
			col.rigidbody2D.gravityScale = 1;
		}
	}

	float textureFlow = 0;

	void Update()
	{
		zoneArea.renderer.material.mainTextureOffset = new Vector2(0,textureFlow);
		textureFlow = -Mathf.Repeat(Time.time*flowSpeed,1);
	}

}

