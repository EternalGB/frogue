using UnityEngine;
using System.Collections;

public class MedusaHead : MonoBehaviour
{

	public float horiVelocity;
	public float oscillationRate;
	Vector2 pos;

	void Update()
	{
		pos.x = transform.position.x + horiVelocity*Time.deltaTime;
		pos.y = Mathf.Sin(Mathf.Repeat(oscillationRate*Time.time,2*Mathf.PI));
		rigidbody2D.MovePosition(pos);
	}

}

