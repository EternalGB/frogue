using UnityEngine;
using System.Collections;

public class AimedTurret : MonoBehaviour
{

	public Transform barrel;
	public GameObject projectile;
	ObjectPool projPool;
	public float maxRotSpeed;
	Collider2D target;
	public float projVelocity;
	public float reloadingTime;
	bool canFire = true;
	public float detectionRadius;
	public LayerMask targets;
	Animator anim;

	void OnEnable()
	{
		target = null;
		anim = GetComponent<Animator>();
		projPool = PoolManager.Instance.GetPoolByRepresentative(projectile);
	}

	void Update()
	{
		target = Physics2D.OverlapCircle(transform.position,detectionRadius,targets);
		if(target) {
			anim.SetBool("haveTarget",true);
			Vector3 vectorToTarget = target.transform.position - transform.position;
			float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
			Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
			transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * maxRotSpeed);

			Color tmp = Color.white;
			RaycastHit2D rayhit = Physics2D.Raycast(transform.position,transform.right,detectionRadius,targets);
			if(rayhit.collider) {
				Fire();
				tmp = Color.red;
			}
			Debug.DrawRay(transform.position,transform.right,tmp);
		} else {
			anim.SetBool("haveTarget",false);
		}
	}

	void Fire()
	{
		if(canFire) {
			anim.SetTrigger("fire");
			GameObject proj = projPool.GetPooled();
			Physics2D.IgnoreCollision(proj.collider2D,collider2D);
			proj.SetActive(true);
			proj.transform.position = barrel.position;
			proj.transform.rotation = barrel.rotation;
			proj.rigidbody2D.velocity = (barrel.position - transform.position).normalized*projVelocity;
			StartCoroutine(Timers.Countdown(reloadingTime,Reload));
			canFire = false;
		}
	}

	void Reload()
	{
		canFire = true;
	}
			
}

