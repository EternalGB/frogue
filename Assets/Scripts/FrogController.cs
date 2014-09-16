using UnityEngine;
using System.Collections.Generic;

public class FrogController : MonoBehaviour 
{

	public Transform groundCheck;
	public LayerMask groundLayer;
	public Transform dragBall;
	public float maxPower;
	public float distPowerRatio;
	public float aimSensitivity;
	public LineRenderer predictor;
	public int predictionResolution;

	public MouthController mouth;
	float maxDist;

	public LayerMask killLayer;

	Vector3 ballMove = Vector3.zero;

	bool onGround;
	bool canJump = false;
	bool checkGround = true;
	public int numJumps = 1;
	public bool canGlide;
	int timesJumped = 0;
	Animator anim;

	public float distanceTraveled = 0;
	public float foodAmount = 1;
	public float hungerRate;

	//public GameObject deathParticles;

	public delegate void PlayerDeathHandler();
	public static event PlayerDeathHandler Die;

	public float score;

	public static FrogController Instance 
	{
		get; private set;
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
		Die += HandleDie;
	}



	// Update is called once per frame
	void Update () 
	{
		foodAmount -= hungerRate*Time.deltaTime;
		if(foodAmount <= 0)
			Die();
		distanceTraveled = Mathf.Max(distanceTraveled,transform.position.x);
		maxDist = maxPower/distPowerRatio;
		if(checkGround)
			onGround = Physics2D.OverlapCircle(groundCheck.position,0.1f,groundLayer);
		if(timesJumped > 0 && onGround)
			timesJumped = 0;
		anim.SetBool("onGround",onGround);
		canJump = timesJumped < numJumps;
/*
		#if UNITY_EDITOR
			canJump = true;
		#endif
*/
		//start jump calc
		if(Input.GetMouseButton(0) && canJump) {
			dragBall.GetComponent<SpriteRenderer>().enabled = true;
			ballMove.x = Input.GetAxis("Mouse X");
			ballMove.y = Input.GetAxis("Mouse Y");
			dragBall.transform.localPosition += ballMove*aimSensitivity;
			dragBall.transform.localPosition = Vector3.ClampMagnitude(dragBall.transform.localPosition,maxDist);
			Vector3 nextVel = ((transform.position - dragBall.position)*distPowerRatio);
			predictor.enabled = true;
			UpdatePredictions(transform.position,nextVel,5, predictionResolution);
		//perform jump
		} else if(Input.GetMouseButtonUp(0)) {
			if(canJump) {
				dragBall.GetComponent<SpriteRenderer>().enabled = false;
				//fire the frog
				rigidbody2D.velocity = ((transform.position - dragBall.position)*distPowerRatio);
				dragBall.transform.localPosition = Vector3.zero;
				predictor.enabled = false;
				if(timesJumped == 0) {
					onGround = false;
					checkGround = false;
					StartCoroutine(Timers.Countdown(0.1f, delegate() {checkGround = true;}));
				}
				timesJumped++;
			} else if(!onGround && canGlide) {
				rigidbody2D.gravityScale = 1;
			}
		//gliding
		} else if(canGlide && Input.GetMouseButton(0) && !onGround) {
			rigidbody2D.gravityScale = 0;
		//mouth stuff
		} else if(Input.GetMouseButtonDown(1) && mouth.CanActivate()) {
			mouth.DoAvailableMouthAction();
		}
	}

	public void UpdatePredictions(Vector3 initPos, Vector3 initVel, float maxTime, int numSlices)
	{
		predictor.SetVertexCount(numSlices);
		predictor.SetPosition(0,initPos);
		float timeDiff = maxTime/(numSlices-1);
		for(int i = 1; i < numSlices; i++) {
			Vector3 prediction = Util.KinematicPrediction2D(initPos,initVel,Physics2D.gravity,timeDiff*i);
			prediction.z = predictor.transform.position.z;
			predictor.SetPosition(i,prediction);
		}

	}

	public List<Vector3> TrajectoryPredictions(Vector3 initPos, Vector3 initVel, float maxTime, int numSlices)
	{
		List<Vector3> positions = new List<Vector3>(numSlices);
		float timeDiff = maxTime/numSlices;
		for(int i = 0; i < numSlices; i++) {
			Vector3 next = Util.KinematicPrediction2D(initPos,initVel,Physics2D.gravity,timeDiff*i);
			positions.Add(next);
		}
		return positions;
	}



	void OnCollisionEnter2D(Collision2D col)
	{
		//if in the kill layer
		if((killLayer.value &1 << col.gameObject.layer) != 0) {
			Die();
		} else if(col.gameObject.layer == LayerMask.NameToLayer("Pickup")) {
			PoolablePickup pickup = col.gameObject.GetComponent<PoolablePickup>();
			pickup.ApplyEffect();
			pickup.transform.parent = null;
			pickup.Destroy();
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		//if in the kill layer
		if((killLayer.value &1 << col.gameObject.layer) != 0) {
			Die();
		} else if(col.gameObject.layer == LayerMask.NameToLayer("Pickup")) {
			PoolablePickup pickup = col.gameObject.GetComponent<PoolablePickup>();
			pickup.ApplyEffect();
			pickup.transform.parent = null;
			pickup.Destroy();
		}
	}

	void HandleDie ()
	{
		gameObject.SetActive(false);
		//deathParticles.transform.position = transform.position;
		//deathParticles.SetActive(true);
		Die -= HandleDie;
	}

	void AddJump()
	{
		numJumps++;
	}
	

}
