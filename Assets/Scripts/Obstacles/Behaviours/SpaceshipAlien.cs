using UnityEngine;
using System.Collections;

public class SpaceshipAlien : MonoBehaviour
{

	public Transform firingPos, detectionDirection;
	public GameObject projectile;
	public float projSpeed;
	public float cooldown;
	bool canFire = true;
	public float initSpeedMin,initSpeedMax;
	public float detectionRange;
	public LayerMask targets;
	ObjectPool projPool;

	void OnEnable()
	{
		projPool = PoolManager.Instance.GetPoolByRepresentative(projectile);
		float initSpeed = Random.Range (initSpeedMin,initSpeedMax);
		rigidbody2D.velocity = -Vector2.right*initSpeed;
		rigidbody2D.angularVelocity = RandomUtil.GetRandomElement(new int[]{-1,1})*initSpeed*5;
	}

	RaycastHit2D hit;

	void Update()
	{
		Vector3 firingDir = (detectionDirection.position - firingPos.position).normalized;
		hit = Physics2D.Linecast(firingPos.position,
		                   firingPos.position + firingDir*detectionRange,
		                   targets);
		Color lineCol = Color.white;

		if(hit.collider != null) {
			if(canFire)
				FireProjectile(firingDir,projSpeed);
			lineCol = Color.red;
		}
		Debug.DrawLine(firingPos.position,
		               firingPos.position + firingDir*detectionRange, lineCol);
	}

	void FireProjectile(Vector3 firingDir, float speed)
	{
		GameObject proj = projPool.GetPooled();
		proj.SetActive(true);
		Physics2D.IgnoreCollision(collider2D,proj.collider2D);
		proj.transform.position = firingPos.position;
		proj.transform.rotation = Quaternion.FromToRotation(Vector3.right, firingDir);
		proj.rigidbody2D.velocity = firingDir*speed;

		canFire = false;
		StartCoroutine(Timers.Countdown(cooldown,EnableFire));
	}

	void EnableFire()
	{
		canFire = true;
	}


}

