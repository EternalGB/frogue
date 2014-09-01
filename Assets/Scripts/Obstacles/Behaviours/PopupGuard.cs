using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class PopupGuard : MonoBehaviour
{

	Animator anim;
	public float detectionRadius;
	public LayerMask targets;
	bool enemyDetected;

	void OnEnable()
	{
		anim = GetComponent<Animator>();
	}

	void Update()
	{
		enemyDetected = Physics2D.OverlapCircle(transform.position,detectionRadius,targets);
		anim.SetBool("enemyDetected",enemyDetected);
	}

}

