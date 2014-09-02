using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class SpookyGhost : MonoBehaviour
{
	
	public float speed;
	public Transform forward;
	Animator anim;

	void OnEnable()
	{
		anim = GetComponent<Animator>();
	}

	void Activate()
	{
		anim.SetBool("activated",true);
		rigidbody2D.velocity = (forward.position - transform.position).normalized*speed;
	}

}



