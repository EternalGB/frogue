using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour
{

	public Transform target;

	void Start()
	{
		target = FrogController.Instance.transform;
	}

	void Update()
	{
		transform.position = new Vector3(target.transform.position.x,target.transform.position.y,-10);
	}

}

