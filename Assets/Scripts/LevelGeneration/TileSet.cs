using UnityEngine;
using System.Collections.Generic;

public class TileSet : MonoBehaviour
{

	public List<List<string>> test;
	public List<TileList> tileReps;
	Dictionary<string, List<Sprite>> tiles;
	public bool mixedTiles;
	public float tileWidth, tileHeight;
	public List<Sprite> platformBases; 
	public List<Sprite> platformDecorations;
	public List<Material> backdrops;
	public List<GameObject> backgroundObj;
	public List<GameObject> middlegroundObj;
	public List<GameObject> airObstacles;
	public List<GameObject> platformObstacles;
	public PhysicsMaterial2D physicsMaterial;
	public int difficulty;
	int currentGroup = 0;


	public bool HasAirObst
	{
		get {return airObstacles != null && airObstacles.Count > 0;}
	}

	public bool HasPlatformObst
	{
		get {return platformObstacles != null && platformObstacles.Count > 0;}
	}

	public bool HasObst
	{
		get {return HasAirObst || HasPlatformObst;}
	}

	public bool HasPlatformDecorations
	{
		get{return platformDecorations != null && platformDecorations.Count > 0;}
	}

	void Awake()
	{
		tiles = new Dictionary<string, List<Sprite>>();
		foreach(TileList rep in tileReps) {
			tiles.Add(rep.tileName,rep.tiles);
		}
	}
	

	public Sprite GetTile(string type, int i)
	{
		return tiles[type][i];
	}

	public void SetGroup(int i)
	{
		currentGroup = i;
	}

	public void SetGroupRandom()
	{
		int maxNum = int.MaxValue;
		foreach(List<Sprite> sprites in tiles.Values) {
			if(sprites.Count < maxNum)
				maxNum = sprites.Count;
		}
		currentGroup = Random.Range (0,maxNum);
	}
	
	public Sprite GetTile(string type)
	{
		if(mixedTiles)
			return RandomUtil.GetRandomElement(tiles[type]);
		else
			return GetTile(type,currentGroup);
	}

	
}

