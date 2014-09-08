using UnityEngine;
using System.Collections.Generic;

public class PlatformManager : MonoBehaviour
{

	public BackgroundManager bm;
	public int platformPoolSize;
	public Transform start;
	public int minWidth, maxWidth;
	public Vector2 minGap, maxGap;
	public float minHeight;
	public float maxHeight;

	public GameObject platformRep;
	public TileSet tileSet;

	public AnimationCurve minGapCurve;
	public AnimationCurve maxGapCurve;
	public List<TileSet> tileSets;
	Dictionary<int,List<TileSet>> tsDict;
	public AnimationCurve tileSetDifficultyCurve;

	Vector2 nextPos;
	ObjectPool platforms;
	float distanceScaling = 1;
	public int difficulty = 1;

	public AnimationCurve numObstCurve;
	public int numObst = 0;
	public int activeObst = 0;
	float obstChance = 0.5f;


	int numLevelChanges = 0;
	float levelThreshold = 50;

	public float pickupChance;
	public GameObject foodPickup;


	void Start()
	{

		platforms = PoolManager.Instance.GetNewUntrackedPool(platformRep,platformPoolSize,false);

		nextPos = start.position;

		tsDict = new Dictionary<int,List<TileSet>>();
		foreach(TileSet ts in tileSets) {
			if(!tsDict.ContainsKey(ts.difficulty))
				tsDict.Add(ts.difficulty,new List<TileSet>());
			tsDict[ts.difficulty].Add(ts);
		}

		PoolableObstacle.ObjDestroyed += HandleDestroyedObstacle;
		bm.InitBackground(tileSet);
	}


	void Update()
	{


		if(bm.PanelNeedsRecycling)
			bm.RecyclePanel(bm.frontLinePos + bm.panelWidth,tileSet);

		if(platforms.ObjectAvailable())
			GenPlatform();

				//TODO change tileset
		//probably based on distance milestones?
		difficulty = (int)tileSetDifficultyCurve.Evaluate(FrogController.Instance.distanceTraveled/distanceScaling);

		numObst = (int)numObstCurve.Evaluate(FrogController.Instance.distanceTraveled/distanceScaling);

		//TODO also change gap and tile width constraints over time
		minGap.x = minGapCurve.Evaluate(FrogController.Instance.distanceTraveled/distanceScaling);
		maxGap.x = maxGapCurve.Evaluate(FrogController.Instance.distanceTraveled/distanceScaling);

		if(FrogController.Instance.distanceTraveled - (numLevelChanges+1)*levelThreshold > 0) {
			//change tileset
			numLevelChanges++;
			ChangeLevel();
		}
	}

	void GenPlatform()
	{
		GameObject platform = platforms.GetPooled();
		if(platform != null) {
			PoolablePlatform pp = platform.GetComponent<PoolablePlatform>();
			int width = Random.Range(minWidth,maxWidth);
			pp.CreatePlatform(width,nextPos,tileSet);
			platform.SetActive(true);

			if(activeObst < numObst && tileSet.HasObst && Random.value < obstChance) {
				if(tileSet.HasAirObst && tileSet.HasPlatformObst) {
					if(Random.value < 0.5)
						GenAirObstacle(pp.actualWidth);
					else
						GenPlatformObstacle(pp.actualWidth);
				} else if(tileSet.HasAirObst)
					GenAirObstacle(pp.actualWidth);
				else
					GenPlatformObstacle(pp.actualWidth);
			}

			//potentially add a pickup
			if(Random.value < pickupChance)
				GenPickup(foodPickup,nextPos,pp.actualWidth);

			//add some background decorations
			if(bm.ObjAvailable && Random.value < bm.spawnChance)
				bm.CreateBackgroundObj(tileSet, nextPos);

			//generate a new position, clamped inside the height bounds
			nextPos += new Vector2(
				Random.Range (minGap.x, maxGap.x) + width*tileSet.tileSize,
				Mathf.Clamp(Random.Range (minGap.y, maxGap.y),minHeight,maxHeight));
		}

	}

	void GenPickup(GameObject pickupRep, Vector2 platPos, float platWidth)
	{
		GameObject pickup = PoolManager.Instance.GetPoolByRepresentative(pickupRep).GetPooled ();
		pickup.SetActive(true);
		pickup.transform.position = platPos + RandomUtil.RandomVector(new Vector2(-platWidth,2),new Vector2(platWidth,6));
	}

	void GenAirObstacle(float width)
	{
		if(tileSet.airObstacles.Count > 0) {

			GameObject obstRep = RandomUtil.GetRandomElement(tileSet.airObstacles);
			Vector2 pos = nextPos + new Vector2(Random.Range (-width/2,width/2),Random.Range (4f,6f));
			GameObject obst = PoolManager.Instance.GetPoolByRepresentative(obstRep).GetPooled();
			if(obst != null) {
				obst.transform.position = pos;
				obst.SetActive(true);
				activeObst++;
			}
		}
	}

	void GenPlatformObstacle(float width)
	{
		if(tileSet.platformObstacles.Count > 0) {
			GameObject obstRep = RandomUtil.GetRandomElement(tileSet.platformObstacles);
			Vector2 pos = nextPos + new Vector2(Random.Range (-width/2,width/2),3);
			GameObject obst = PoolManager.Instance.GetPoolByRepresentative(obstRep).GetPooled();
			if(obst != null) {
				obst.transform.position = pos;
				//TODO properly move the obst up so it's above the platform
				obst.SetActive(true);
				activeObst++;
			}
		}
	}

	void ChangeLevel()
	{
		Debug.Log ("CHANGING LEVEL");
		if(tsDict.ContainsKey(difficulty)) {
			List<TileSet> potentialTileSets = tsDict[difficulty];
			tileSet = potentialTileSets[Random.Range(0,potentialTileSets.Count)];
			bm.RecyclePanel(nextPos.x,tileSet);
		}

	}

	void HandleDestroyedObstacle(GameObject destroy)
	{
		activeObst--;
	}

}

