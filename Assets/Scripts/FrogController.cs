﻿using UnityEngine;
using System.Collections.Generic;

public class FrogController : MonoBehaviour 
{

	public Transform groundCheck;
	public LayerMask groundLayer;
	public Transform dragBall;
	public float maxPower;
	public float distPowerRatio;
	public float mouseSensitivity;
	public LineRenderer predictor;
	public int predictionResolution;

	public TongueController tongue;
	float maxDist;
	
	Vector3 ballMove = Vector3.zero;

	bool onGround;
	Animator anim;

	public float distanceTraveled = 0;
	public int fliesCollected = 0;

	public static FrogController Instance 
	{
		get; private set;
	}

	public Vector3 MouseWorldPos 
	{
		get 
		{
			Vector3 mousePos = Input.mousePosition;
			return Camera.main.ScreenToWorldPoint(mousePos);
		}
	}

	void Awake()
	{
		if(Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}

		Instance = this;
	}


	void Start()
	{
		anim = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update () 
	{
		distanceTraveled = Mathf.Max(distanceTraveled,transform.position.x);
		maxDist = maxPower/distPowerRatio;
		onGround = Physics2D.OverlapCircle(groundCheck.position,0.2f,groundLayer);
		anim.SetBool("onGround",onGround);
		if(Input.GetMouseButton(0)) {
			dragBall.GetComponent<SpriteRenderer>().enabled = true;
			ballMove.x = Input.GetAxis("Mouse X");
			ballMove.y = Input.GetAxis("Mouse Y");
			dragBall.transform.localPosition += ballMove*mouseSensitivity;
			dragBall.transform.localPosition = Vector3.ClampMagnitude(dragBall.transform.localPosition,maxDist);
			Vector3 nextVel = ((transform.position - dragBall.position)*distPowerRatio);
			predictor.enabled = true;
			UpdatePredictions(transform.position,nextVel,5, predictionResolution);
		} else if(Input.GetMouseButtonUp(0)) {
			dragBall.GetComponent<SpriteRenderer>().enabled = false;
			//fire the frog
			rigidbody2D.velocity = ((transform.position - dragBall.position)*distPowerRatio);
			dragBall.transform.localPosition = Vector3.zero;
			predictor.enabled = false;
		} else if(Input.GetMouseButton(1) && tongue.CanLick()) {
			tongue.Lick(MouseWorldPos);
		}
	}

	public void UpdatePredictions(Vector3 initPos, Vector3 initVel, float maxTime, int numSlices)
	{
		predictor.SetVertexCount(numSlices);
		predictor.SetPosition(0,initPos);
		float timeDiff = maxTime/(numSlices-1);
		for(int i = 1; i < numSlices; i++) {
			predictor.SetPosition(i,KinematicPrediction2D(initPos,initVel,Physics2D.gravity,timeDiff*i));
		}

	}

	public List<Vector3> TrajectoryPredictions(Vector3 initPos, Vector3 initVel, float maxTime, int numSlices)
	{
		List<Vector3> positions = new List<Vector3>(numSlices);
		float timeDiff = maxTime/numSlices;
		for(int i = 0; i < numSlices; i++) {
			Vector3 next = KinematicPrediction2D(initPos,initVel,Physics2D.gravity,timeDiff*i);
			positions.Add(next);
		}
		return positions;
	}

	public static Vector3 KinematicPrediction2D(Vector3 p, Vector3 v, Vector3 a, float t)
	{
		Vector3 res = new Vector3();
		res.x = p.x + v.x*t;
		res.y = p.y + v.y*t + 0.5f*a.y*Mathf.Pow(t,2);
		return res;
	}



}
