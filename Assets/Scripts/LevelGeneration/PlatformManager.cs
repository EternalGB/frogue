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
	}

	void Update()
	{
		if(platforms.ObjectAvailable())
			GenPlatform();

		//TODO change tileset
		//probably based on distance milestones?
		difficulty = (int)tileSetDifficultyCurve.Evaluate(FrogController.Instance.distanceTraveled/distanceScaling);

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

}

