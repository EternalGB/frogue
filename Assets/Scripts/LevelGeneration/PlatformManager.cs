using UnityEngine;
using System.Collections.Generic;

public class PlatformManager : MonoBehaviour
{

	public int queueSize;
	public float recycleOffset;
	public Transform start;
	public int minWidth, maxWidth;
	public Vector2 minGap, maxGap;
	public int numPlats;

	public TileSet tileSet;

	public LayerMask platformLayer;

	public AnimationCurve minGapCurve;
	public AnimationCurve maxGapCurve;
	public int numTracks;
	//public AnimationCurve numPlatsCurve;

	const string collectablesPath = "Collectables/";
	public GameObject flyPrefab;
	ObjectPool flyPool;

	Vector2 nextPos;
	Queue<Platform> platforms;
	float distanceScaling = 1000;

	void Start()
	{
		flyPool = PoolManager.Instance.GetPoolByRepresentative(flyPrefab);
		platforms = new Queue<Platform>(queueSize);
		for(int i = 0; i < queueSize; i++) {
			platforms.Enqueue(new Platform(tileSet));
		}
		nextPos = start.position;
		for(int i = 0; i < queueSize; i++) {
			Recycle();
		}


	}

	void Update()
	{
		if(platforms.Peek().position.x + recycleOffset < FrogController.Instance.distanceTraveled) {
			Recycle();
		}
		//TODO change tileset
		//probably based on distance milestones?

		//TODO also change gap and tile width constraints over time
		minGap.x = minGapCurve.Evaluate(FrogController.Instance.distanceTraveled/distanceScaling);
		maxGap.x = maxGapCurve.Evaluate(FrogController.Instance.distanceTraveled/distanceScaling);
		//numPlats = (int)numPlatsCurve.Evaluate(frog.distanceTraveled/distanceScaling);
	}

	int maxTries = 10;

	void Recycle()
	{
		Platform platform = platforms.Dequeue();


		platform.tileSet = tileSet;
		int width = Random.Range(minWidth,maxWidth);
		platform.width = width;
		platform.position = nextPos;
		platform.RecycleTiles();
		platform.Create();
		platforms.Enqueue(platform);

		//sometimes add a fly
		if(Random.value < 0.5) {
			GameObject fly = flyPool.GetPooled();
			fly.transform.position = nextPos + new Vector2(width/2,5);
			fly.SetActive(true);
		}

		//generate a new position but max sure there isn't another platform there already
		//in case we're running multiple platform generators
		Vector2 tmpPos;
		int tries = 0;
		do {
			tmpPos = nextPos + new Vector2(
				Random.Range (minGap.x, maxGap.x) + width*tileSet.tileSize,
				Random.Range (minGap.y, maxGap.y));
			tries++;
		} while(tries < maxTries &&
		        Physics2D.OverlapArea(tmpPos,tmpPos + new Vector2(width*tileSet.tileSize,tileSet.tileSize),platformLayer));
		nextPos = tmpPos;
	}
}

