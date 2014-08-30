using UnityEngine;
using System.Collections;

public class Beam : MonoBehaviour
{

	public Transform turret;
	public bool isVertical;
	Vector3 offset;

	void Update()
	{
		if(turret != null) {
			if(isVertical)
				transform.position = new Vector3(turret.position.x,turret.position.y + offset.y);
			else
				transform.position = new Vector3(turret.position.x + offset.x,turret.position.y);
		}
	}

	void SetTurret(Transform turret)
	{
		this.turret = turret;
		offset = transform.position - turret.position;
	}

	void SetVertical(bool isVertical)
	{
		this.isVertical = isVertical;
	}

}

