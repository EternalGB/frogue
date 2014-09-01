using UnityEngine;
using System.Collections;

public class Beam : MonoBehaviour
{

	public Transform turret;
	Vector3 offset;
	

	void Update()
	{
		if(turret != null) {
			Vector3 final = turret.rotation*offset;
			transform.position = turret.position + final;
			transform.rotation = turret.rotation;
		}

	}

	void SetTurret(Transform turret)
	{
		this.turret = turret;
		offset = transform.position - turret.position;
	}
	

}

