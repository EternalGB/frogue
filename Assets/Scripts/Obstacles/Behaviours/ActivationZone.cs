using UnityEngine;
using System.Collections;

public class ActivationZone : MonoBehaviour
{
	
	public Transform parent;
	public LayerMask targets;
	
	void OnTriggerEnter2D(Collider2D col)
	{
		if(Util.InLayerMask(targets,col.gameObject.layer))
			parent.SendMessage("Activate");
	}
	
}

