using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class GravZone : PoolableObstacle
{

	public float minGravMagnitude,maxGravMagnitude;
	public Vector2 minSize, maxSize;
	Vector2 gravity;
	public Sprite directionalSprite;
	public Sprite noDirectionSprite;
	public Transform zoneArea;
	SpriteRenderer sr;
	BoxCollider2D boxCol;
	float flowSpeed = 0.1f;

	void OnEnable()
	{
		sr = GetComponent<SpriteRenderer>();
		boxCol = (BoxCollider2D)collider2D;
		SetGravity(Random.insideUnitCircle.normalized*Random.Range (minGravMagnitude,maxGravMagnitude));
		SetArea(boxCol.center,RandomUtil.RandomVector(minSize,maxSize));
	}

	public void SetArea(Vector2 center, Vector2 size)
	{
		boxCol.center = center;
		boxCol.size = size;
		zoneArea.localScale = size;
		zoneArea.renderer.material.mainTextureScale = size;
	}

	public void SetGravity(Vector2 newGrav)
	{
		gravity = newGrav;
		if(gravity != Vector2.zero) {
			sr.sprite = directionalSprite;
			Quaternion rot = Quaternion.FromToRotation(Vector2.right,newGrav.normalized);
			transform.rotation = rot;
		} else {
			sr.sprite = noDirectionSprite;
			transform.rotation = Quaternion.identity;
		}
		flowSpeed = gravity.magnitude/2;
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.rigidbody2D != null && !col.rigidbody2D.isKinematic) {
			col.rigidbody2D.gravityScale = 0;
			//col.rigidbody2D.velocity = Vector2.zero;
		}
	}

	void OnTriggerStay2D(Collider2D col)
	{
		if(col.rigidbody2D != null && !col.rigidbody2D.isKinematic) {
			col.rigidbody2D.velocity += gravity*Time.fixedDeltaTime;
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
		zoneArea.renderer.material.mainTextureOffset = new Vector2(textureFlow,0);
		textureFlow = -Mathf.Repeat(Time.time*flowSpeed,1);
	}

}

