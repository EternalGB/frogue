using UnityEngine;
using System.Collections.Generic;

public class PlatformManager : MonoBehaviour
{

	public int platformPoolSize;
	public Transform start;
	public int minWidth, maxWidth;
	public Vector2 minGap, maxGap;

	public GameObject platformRep;
	public TileSet tileSet;

	public AnimationCurve minGapCurve;
	public AnimationCurve maxGapCurve;
	public List<TileSet> tileSets;
	Dictionary<int,List<TileSet>> tsDict;
	public AnimationCurve tileSetDifficultyCurve;

	Vector2 nextPos;
	ObjectPool platforms;
	float distanceScaling = 1000;
	public int difficulty = 1;

	public AnimationCurve numObstCurve;
	public int numObst = 0;
	public int activeObst = 0;
	float obstChance = 0.2f;

	int numLevelChanges = 0;
	float levelThreshold = 100;

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
	}

	void Update()
	{
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
			nextPos += new Vector2(
				Random.Range (minGap.x, maxGap.x) + width*tileSet.tileSize,
				Random.Range (minGap.y, maxGap.y));

			if(activeObst < numObst && tileSet.HasObst && Random.value < 0.1) {
				if(tileSet.HasAirObst && tileSet.HasPlatformObst) {
					if(Random.value < 0.5)
						GenAirObstacle(width);
					else
						GenPlatformObstacle(width);
				} else if(tileSet.HasAirObst)
					GenAirObstacle(width);
				else
					GenPlatformObstacle(width);
			}

		}

	}

	void GenAirObstacle(int width)
	{
		if(tileSet.airObstacles.Count > 0) {
			float widthf = (float)width;
			GameObject obstRep = tileSet.airObstacles[Random.Range(0,tileSet.airObstacles.Count-1)];
			Vector2 pos = nextPos + new Vector2(Random.Range (-widthf,widthf),Random.Range (4f,6f));
			GameObject obst = PoolManager.Instance.GetPoolByRepresentative(obstRep).GetPooled();
			if(obst != null) {
				obst.transform.position = pos;
				obst.SetActive(true);
				activeObst++;
			}
		}
	}

	void GenPlatformObstacle(int width)
	{
		if(tileSet.platformObstacles.Count > 0) {
			float widthf = (float)width;
			GameObject obstRep = tileSet.platformObstacles[Random.Range(0,tileSet.airObstacles.Count-1)];
			Vector2 pos = nextPos + new Vector2(Random.Range (-widthf,widthf),3);
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
		print ("CHANGING LEVEL");
		if(tsDict.ContainsKey(difficulty)) {
			List<TileSet> potentialTileSets = tsDict[difficulty];
			tileSet = potentialTileSets[Random.Range(0,potentialTileSets.Count-1)];
		}

	}

	void HandleDestroyedObstacle(GameObject destroy)
	{
		activeObst--;
	}

}

