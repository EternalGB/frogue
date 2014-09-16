using UnityEngine;
using System.Collections;

public class DeathFloor : MonoBehaviour 
{

	public Transform target;

	void Start()
	{
		target = FrogController.Instance.transform;
	}

	void Update()
	{
		transform.position = new Vector3(target.position.x,transform.position.y);
	}

}
