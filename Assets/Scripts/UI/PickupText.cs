using UnityEngine;
using System.Collections;

public class PickupText : PoolableObject
{

	public GUIText text;
	public float lifetime;
	public Vector3 movementDirection;
	public float movementSpeed;
	Vector3 startPos;
	float startTime;
	Color startColor;
	Color color;

	void OnEnable()
	{
		startPos = transform.position;
		startTime = Time.time;
		startColor = text.color;
		color = startColor;
		StartCoroutine(Timers.Countdown(lifetime,Destroy));
	}

	void Update()
	{
		transform.position += movementDirection.normalized*movementSpeed*Time.deltaTime;
		color.a = Mathf.Lerp(startColor.a,0,(Time.time - startTime)/lifetime);
		text.color = color;
		text.transform.position = Camera.main.WorldToViewportPoint(transform.position);
	}

	void OnDisable()
	{
		text.color = startColor;
	}



}

