using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class SnowThrower : MonoBehaviour
{

	public GameObject projectile;
	ObjectPool projPool;
	public Transform throwingPos;
	Vector2 throwingDir;
	//public float throwForce;
	public float detectionRadius;
	public LayerMask targets;
	public float reloadTime;
	bool canFire = true;
	Transform target;

	Animator anim;

	void OnEnable()
	{
		anim = GetComponent<Animator>();
		projPool = PoolManager.Instance.GetPoolByRepresentative(projectile);
		throwingDir = new Vector2(-Mathf.Sqrt(2)/2f,Mathf.Sqrt(2)/2f);
		canFire = true;
	}

	void Update()
	{
		Collider2D col = Physics2D.OverlapCircle(transform.position,detectionRadius,targets);
		if(col) {
			target = col.transform;
			Fire();
		} else {
			target = null;
		}
	}

	void Fire()
	{
		if(target != null && canFire) {
			anim.SetTrigger("throwing");
			GameObject proj = projPool.GetPooled();
			Physics2D.IgnoreCollision(collider2D,proj.collider2D);
			proj.SetActive(true);
			proj.transform.position = throwingPos.position;
			Vector2 power = Util.GetLaunchPower(throwingDir,throwingPos.position,target.position,0.75f);
			proj.rigidbody2D.velocity = new Vector2(power.x*throwingDir.x,power.y*throwingDir.y);

			canFire = false;
			Reload ();
		}
	}

	void Reload()
	{
		anim.SetBool("reloading",true);
		StartCoroutine(Timers.Countdown(reloadTime,SetReady));
	}

	void SetReady()
	{
		anim.SetBool("reloading",false);
		canFire = true;
		//anim.SetTrigger("stand");
	}

}

