using UnityEngine;
using System.Collections.Generic;

public class PoolableObstacle : PoolableObject
{

	float recycleOffset = 20;
	public delegate void DestroyHandler(GameObject destroyed);
	public static event DestroyHandler ObjDestroyed;

	void Update()
	{
		if(enabled) {
			if(transform.position.x + recycleOffset < FrogController.Instance.distanceTraveled) {
				Destroy();
			}
		}
	}

	public override void Destroy()
	{
		ObjDestroyed(gameObject);
		base.Destroy();
	}
	
	List<PhysicsMaterial2D> savedPhysMats;
	int savedLayer;

	void DisableObstacle(float duration)
	{
		//disable any behaviours that aren't obstacle behaviours
		MonoBehaviour[] behaviours = GetComponents<MonoBehaviour>();
		foreach(MonoBehaviour b in behaviours) {
			if(b.GetType() != typeof(PoolableObstacle))
				b.enabled = false;
		}
		//disable any animators that may exist
		Animator anim;
		if(anim = GetComponent<Animator>())
			anim.enabled = false;
		//remove any physics materials
		savedPhysMats = new List<PhysicsMaterial2D>();
		Collider2D[] colliders = GetComponents<Collider2D>();
		foreach(Collider2D c in colliders) {
			savedPhysMats.Add(c.sharedMaterial);
			c.sharedMaterial = null;
		}
		//reset layer to default
		savedLayer = gameObject.layer;
		gameObject.layer = 0;
		StartCoroutine(Timers.Countdown(duration,EnableObstacle));
	}

	void EnableObstacle()
	{
		//reverse disable
		MonoBehaviour[] behaviours = GetComponents<MonoBehaviour>();
		foreach(MonoBehaviour b in behaviours) {
			if(b.GetType() != typeof(PoolableObstacle))
				b.enabled = true;
		}

		Animator anim;
		if(anim = GetComponent<Animator>())
			anim.enabled = true;

		Collider2D[] colliders = GetComponents<Collider2D>();
		for(int i = 0; i < colliders.Length; i++) {
			colliders[i].sharedMaterial = savedPhysMats[i];
		}

		gameObject.layer = savedLayer;
	}

}

