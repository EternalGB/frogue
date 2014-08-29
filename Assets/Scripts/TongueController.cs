using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(SpriteRenderer))]
public class TongueController : MonoBehaviour
{

	public bool isLicking = false;
	bool isReturning = false;
	public float maxDist;
	public float tongueSpeed;
	public float pullPower;
	Vector2 destination;
	float distanceTravelled = 0;
	public float cooldown;
	public bool onCooldown = false;

	LineRenderer lr;
	SpriteRenderer sr;
	public Transform mouth;
	public LayerMask grippable;

	void Start()
	{
		lr = GetComponent<LineRenderer>();
		sr = GetComponent<SpriteRenderer>();
		lr.SetVertexCount(2);
		lr.enabled = false;
		sr.enabled = false;
	}

	public bool CanLick()
	{
		return !isLicking && !onCooldown;
	}

	public void Lick(Vector2 position)
	{
		isLicking = true;
		onCooldown = true;
		StartCoroutine(Timers.Countdown(cooldown,ResetCooldown));
		lr.enabled = true;
		sr.enabled = true;
		Vector2 mouthPos = mouth.position;
		Vector2 relativeDest = Vector2.ClampMagnitude(position - mouthPos, maxDist);
		destination = mouthPos + relativeDest;
		distanceTravelled = 0;
	}

	void Update()
	{
		if(isLicking) {
			if(Vector2.Distance(transform.position,destination) < 0.2)
				isReturning = true;
			if(isReturning) {
				transform.position = Vector2.MoveTowards(transform.position,mouth.position,tongueSpeed);
				if(Vector2.Distance(transform.position,mouth.position) < 0.2) {
					ResetTongue();
					ClearCollected();
				}
			} else {
				Vector2 nextPos = Vector2.MoveTowards(transform.position,destination,tongueSpeed);
				distanceTravelled += Vector2.Distance(transform.position,nextPos);
				transform.position = nextPos;
				if(distanceTravelled >= maxDist)
					isReturning = true;
			}
			UpdateLineRenderer();
		}
	}

	void UpdateLineRenderer()
	{
		lr.SetPosition(0,mouth.position);
		lr.SetPosition(1,transform.position);
	}

	void ResetTongue()
	{
		isLicking = false;
		isReturning = false;
		transform.position = mouth.position;
		lr.enabled = false;
		sr.enabled = false;
		distanceTravelled = 0;
	}

	void ClearCollected()
	{
		PoolablePickup[] pickups = GetComponentsInChildren<PoolablePickup>();
		foreach(PoolablePickup p in pickups) {
			FrogController.Instance.fliesCollected++;
			p.transform.parent = null;
			p.Destroy();
		}
	}

	void ResetCooldown()
	{
		onCooldown = false;
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(isLicking) {
			isReturning = true;
			if(col.gameObject.layer == LayerMask.NameToLayer("Pickup")) {

				col.transform.parent = transform;
			} else if(Util.InLayerMask(grippable,col.gameObject.layer)) {
				Vector3 force = (transform.position - FrogController.Instance.transform.position).normalized*pullPower;
				FrogController.Instance.rigidbody2D.velocity = Vector2.zero;
				FrogController.Instance.rigidbody2D.velocity += (Vector2)force;
			}
		}
	}

}

