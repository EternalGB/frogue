using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class Swooper : MonoBehaviour
{

	public float detectionRange;
	public LayerMask targets;
	public float swoopSpeed;
	public float armingTime;
	Collider2D target;
	bool isSwooping = false;
	bool acquiringTarget = false;
	Vector2 swoopDest;
	float parabolicParam = 1;
	float startingX;
	Vector2 nextPos;

	Animator anim;

	void OnEnable()
	{
		anim = GetComponent<Animator>();
	}

	void Update()
	{
		if(isSwooping) {
			nextPos.x = Mathf.Clamp(nextPos.x - swoopSpeed*Time.deltaTime,-startingX,startingX);
			nextPos.y = SimpleQuadratic(parabolicParam,nextPos.x);
			//Debug.Log ("Fly moving to " + (swoopDest + nextPos));
			rigidbody2D.MovePosition(swoopDest + nextPos);
			if(nextPos.x == -startingX)
				anim.SetTrigger("wait");
		} else if(!acquiringTarget) {
			target = Physics2D.OverlapCircle(transform.position,detectionRange,targets);
			if(target) {
				anim.SetTrigger("swoop");
				acquiringTarget = true;
				StartCoroutine(Timers.Countdown(armingTime,BeginSwoop));
			}
		}
	}

	float SimpleQuadratic(float a, float x)
	{
		return a*Mathf.Pow(x,2);
	}

	void BeginSwoop()
	{
		if(target) {
			isSwooping = true;
			swoopDest = target.transform.position;
			parabolicParam = (transform.position.y - swoopDest.y)/Mathf.Pow(transform.position.x - swoopDest.x,2);
			startingX = transform.position.x - swoopDest.x;
			nextPos.x = startingX;
		}
		acquiringTarget = false;
	}
	
	
	
}

