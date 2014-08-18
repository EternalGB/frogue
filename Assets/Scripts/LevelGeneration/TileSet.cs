using UnityEngine;
using System.Collections.Generic;

public class TileSet : MonoBehaviour
{


	public List<Sprite> tileReps;
	Dictionary<string, Sprite> tiles;
	public float tileSize;
	public PhysicsMaterial2D physicsMaterial;

	void Awake()
	{
		tiles = new Dictionary<string, Sprite>();
		foreach(Sprite rep in tileReps) {
			tiles.Add(rep.name,rep);
			print ("Added " + rep.name + " to tileset " + name);
		}
	}

	public Sprite GetTile(string type)
	{
		return tiles[type];
	}

}

