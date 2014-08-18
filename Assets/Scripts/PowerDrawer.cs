using UnityEngine;
using System.Collections;

public class PowerDrawer : MonoBehaviour
{

	public LineRenderer lr;
	public Transform frogue;

	void Update()
	{
		lr.SetPosition(0,transform.position);
		lr.SetPosition(1,frogue.position);

	}

}

