using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PoolablePlatform : PoolableObject
{


	public int width;
	public TileSet tileSet;
	float actualWidth = 0;
	float recycleOffset = 20;

	BoxCollider2D boxCollider;

	Rect tileRect;
	List<SpriteRenderer> tiles;

	void Awake()
	{
		boxCollider = GetComponent<BoxCollider2D>();
		tiles = new List<SpriteRenderer>();
	}

	public void CreatePlatform(int width, Vector3 position, TileSet tileSet)
	{
		this.width = width;
		transform.position = position;
		this.tileSet = tileSet;
		actualWidth = width*tileSet.tileSize;
		boxCollider.size = new Vector2(actualWidth,tileSet.tileSize);
		boxCollider.sharedMaterial = tileSet.physicsMaterial;
		SetSprites();
	}

	void SetSprites()
	{
		if(tiles.Count < width) {
			AddTiles(width - tiles.Count);
		}
		Sprite nextTile;
		if(width == 1)
			nextTile = tileSet.GetTile("Block");
		else
			nextTile = tileSet.GetTile ("Left");
		for(int i = 0; i < width; i++) {
			tiles[i].enabled = true;
			tiles[i].sprite = nextTile;
			tiles[i].transform.position = transform.position - 
				new Vector3(actualWidth/2 - tileSet.tileSize/2 - i*tileSet.tileSize,0);
			if(i == width-2)
				nextTile = tileSet.GetTile("Right");
			else
				nextTile = tileSet.GetTile("Mid");
		}
	} 

	void OnDisable()
	{
		foreach(SpriteRenderer sr in tiles)
			if(sr != null)
				sr.enabled = false;
	}

	void AddTiles(int amount)
	{
		for(int i = 0; i < amount; i++) {
			SpriteRenderer tile = (new GameObject()).AddComponent<SpriteRenderer>();
			tile.enabled = false;
			tiles.Add(tile);
		}
	}

	void Update()
	{
		//recycle the platform if we go too far
		if(FrogController.Instance.distanceTraveled > transform.position.x + recycleOffset + actualWidth/2)
			Destroy();
	}

}

