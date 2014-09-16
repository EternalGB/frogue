using UnityEngine;
using System.Collections;

public class MedusaHead : MonoBehaviour
{

	public float horiVelocity;
	public float oscillationRate;
	public float oscillationSize;
	Vector2 pos;
	Vector2 startingPos;

	void OnEnable()
	{
		startingPos = transform.position;
	}

	void Update()
	{
		pos.x = transform.position.x + horiVelocity*Time.deltaTime;
		pos.y = startingPos.y + oscillationSize*Mathf.Sin(Mathf.Repeat(oscillationRate*Time.time,2*Mathf.PI));
		rigidbody2D.MovePosition(pos);
	}

}

