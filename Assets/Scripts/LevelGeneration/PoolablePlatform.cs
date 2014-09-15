using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PoolablePlatform : PoolableObject
{


	public int width;
	public TileSet tileSet;
	public float actualWidth = 0;
	float recycleOffset = 20;

	BoxCollider2D boxCollider;

	Rect tileRect;
	List<SpriteRenderer> tiles;
	List<SpriteRenderer> decorations;

	void Awake()
	{
		boxCollider = GetComponent<BoxCollider2D>();
		tiles = new List<SpriteRenderer>();
		decorations = new List<SpriteRenderer>();
	}

	public void CreatePlatform(int width, Vector3 position, TileSet tileSet)
	{
		this.width = width;
		transform.position = position;
		this.tileSet = tileSet;
		actualWidth = width*tileSet.tileWidth;
		boxCollider.size = new Vector2(actualWidth,tileSet.tileHeight);
		boxCollider.sharedMaterial = tileSet.physicsMaterial;
		SetSprites();
		if(tileSet.HasPlatformDecorations)
			Decorate();
	}

	void SetSprites()
	{
		if(tiles.Count < width) {
			AddSpriteRenderers(tiles,width - tiles.Count);
		}
		Sprite nextTile;
		tileSet.SetGroupRandom();
		if(width == 1)
			nextTile = tileSet.GetTile("Block");
		else
			nextTile = tileSet.GetTile ("Left");
		for(int i = 0; i < width; i++) {
			tiles[i].enabled = true;
			tiles[i].sprite = nextTile;
			tiles[i].transform.position = transform.position - 
				new Vector3(actualWidth/2 - tileSet.tileWidth/2 - i*tileSet.tileWidth,0);
			if(i == width-2)
				nextTile = tileSet.GetTile("Right");
			else
				nextTile = tileSet.GetTile("Mid");
		}
		//create a base for the platform, if it exists
		int baseHeight = 15;
		if(tileSet.platformBases != null && tileSet.platformBases.Count > 0) {
			AddSpriteRenderers(tiles,(baseHeight + width) - tiles.Count);
			for(int i = width; i < width + baseHeight; i++) {
				tiles[i].sprite = RandomUtil.GetRandomElement(tileSet.platformBases);
				tiles[i].enabled = true;
				tiles[i].transform.position = transform.position - new Vector3(0,boxCollider.size.y/2 + tileSet.tileWidth*(i-width));
				tiles[i].sortingOrder = -2;
			}
		}
	} 

	void Decorate()
	{
		int numDecorations = Random.Range (0,width);
		if(decorations.Count < numDecorations) {
			AddSpriteRenderers(decorations,numDecorations - decorations.Count);
		}
		Sprite nextDec;
		Vector3 nextLoc;
		for(int i = 0; i < numDecorations; i++) {
			nextDec = tileSet.platformDecorations[Random.Range(0,tileSet.platformDecorations.Count)];
			//TODO add a little variation to the positions
			nextLoc = transform.position - 
				new Vector3(actualWidth/2 - tileSet.tileWidth/2 - Random.Range (0,width)*tileSet.tileWidth,-tileSet.tileHeight/2);
			decorations[i].enabled = true;
			decorations[i].sprite = nextDec;
			decorations[i].transform.position = nextLoc;
			decorations[i].sortingOrder = -10;
		}
	}

	void OnDisable()
	{
		foreach(SpriteRenderer sr in tiles)
			if(sr != null)
				sr.enabled = false;
		foreach(SpriteRenderer sr in decorations)
			if(sr != null)
				sr.enabled = false;
	}

	void AddSpriteRenderers(List<SpriteRenderer> renderers, int amount)
	{
		for(int i = 0; i < amount; i++) {
			SpriteRenderer sr = (new GameObject()).AddComponent<SpriteRenderer>();
			sr.enabled = false;
			sr.sortingLayerName = "Foreground";
			sr.sortingOrder = -1;
			renderers.Add(sr);
		}
	}

	void Update()
	{
		//recycle the platform if we go too far
		if(FrogController.Instance.distanceTraveled > transform.position.x + recycleOffset + actualWidth/2)
			Destroy();
	}




}

