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

	public LayerMask platformLayer;
	
	Vector2 nextPos;
	ObjectPool platforms;
	float distanceScaling = 1000;

	void Start()
	{

		platforms = PoolManager.Instance.GetNewUntrackedPool(platformRep,platformPoolSize,false);

		nextPos = start.position;

	}

	void Update()
	{
		if(platforms.ObjectAvailable())
			GenPlatform();
		//TODO change tileset
		//probably based on distance milestones?

		//TODO also change gap and tile width constraints over time
		//minGap.x = minGapCurve.Evaluate(FrogController.Instance.distanceTraveled/distanceScaling);
		//maxGap.x = maxGapCurve.Evaluate(FrogController.Instance.distanceTraveled/distanceScaling);
		//numPlats = (int)numPlatsCurve.Evaluate(frog.distanceTraveled/distanceScaling);
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
}

