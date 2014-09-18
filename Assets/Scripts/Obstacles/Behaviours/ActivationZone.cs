using UnityEngine;
using System.Collections;

public class ActivationZone : MonoBehaviour
{

	public Bounds area;
	public Transform parent;
	public LayerMask targets;

	void Update()
	{
		if(Physics2D.OverlapArea(transform.position + area.center + area.extents,
		                         transform.position + area.center - area.extents,
		                         targets))
			parent.SendMessage("Activate");
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(Util.InLayerMask(targets,col.gameObject.layer))
			parent.SendMessage("Activate");
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireCube(transform.position + area.center, area.size);
	}
	
}

