using UnityEngine;
using System.Collections;

public class Platform 
{


	public int width;
	public Vector3 position;
	public TileSet tileSet;
	//GameObject parent;
	PoolableTile[] tiles;


	public Platform(TileSet tileSet)
	{
		SetValues (0,Vector3.zero,tileSet);
	}

	public Platform(int width, Vector3 position, TileSet tileSet)
	{
		SetValues (width,position,tileSet);
	}

	public void SetValues(int width, Vector3 position, TileSet tileSet)
	{
		this.width = width;
		this.position = position;
		this.tileSet = tileSet;
	}

	public void Create()
	{
		tiles = new PoolableTile[width];
		PoolableTile nextTile;
		if(width == 1)
			nextTile = tileSet.GetTile("Block");
		else
			nextTile = tileSet.GetTile("Left");
		for(int i = 0; i < width; i++) {
			nextTile.gameObject.SetActive(true);
			nextTile.transform.position = position + new Vector3(i*tileSet.tileSize,0,0);
			tiles[i] = nextTile;
			if(i == width-2)
				nextTile = tileSet.GetTile("Right");
			else
				nextTile = tileSet.GetTile("Mid");
		}
	}

	public void RecycleTiles()
	{
		if(tiles != null)
			foreach(PoolableTile tile in tiles) {
				tile.Destroy();
			}
	}

	public override string ToString()
	{
		return "Platform(width = " + width + ", position = " + position.ToString() + ", tileSet = " + tileSet.name + ")";
	}

	public static Platform Create(int width, Vector3 position, TileSet tileSet)
	{
		Platform plat = new Platform(width,position,tileSet);
		plat.Create();
		return plat;
	}
}

