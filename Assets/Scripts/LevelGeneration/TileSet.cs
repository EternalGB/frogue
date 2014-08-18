using UnityEngine;
using System.Collections.Generic;

public class TileSet : MonoBehaviour
{


	public List<GameObject> tileReps;
	Dictionary<string, ObjectPool> tiles;
	public float tileSize;

	void Start()
	{
		tiles = new Dictionary<string, ObjectPool>();
		foreach(GameObject rep in tileReps) {
			tiles.Add(rep.name,PoolManager.Instance.GetPoolByRepresentative(rep));
		}
	}

	public PoolableTile GetTile(string type)
	{
		ObjectPool pool = tiles[type];
		GameObject tile = pool.GetPooled();
		return tile.GetComponent<PoolableTile>();
	}

}

