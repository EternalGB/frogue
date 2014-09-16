using UnityEngine;
using System.Collections;

public class MouthProjController : MouthAction
{

	public GameObject projPrefab;
	ObjectPool projPool;
	public float fireForce;

	void Awake()
	{
		projPool = PoolManager.Instance.GetPoolByRepresentative(projPrefab);
	}

	public override void Action ()
	{
		Fire (Util.MouseWorldPos(FrogController.Instance.transform.position.z));
	}

	void Fire(Vector3 target)
	{
		GameObject proj = projPool.GetPooled();
		proj.transform.position = transform.position;
		proj.SetActive(true);
		proj.rigidbody2D.velocity = (target - proj.transform.position).normalized*fireForce;

	}

}

