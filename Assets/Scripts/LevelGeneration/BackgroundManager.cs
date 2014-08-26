using UnityEngine;
using System.Collections.Generic;


public class BackgroundManager : MonoBehaviour
{
	
	public float panelWidth;
	public float recycleOffset = 10;
	public float frontLinePos;
	public Transform backLayer;
	public Transform middleLayer;
	public int numBackgroundObj;
	int numBackgroundActive;
	int numMiddlegroundActive = 0;
	public int numMiddlegroundObj = 0;
	public float spawnChance;
	public Bounds spawnArea;

	class PanelSorter : IComparer<GameObject>
	{

		public int Compare(GameObject go1, GameObject go2)
		{
			return (int)(go1.transform.position.x - go2.transform.position.x);
		}

		public static IComparer<GameObject> panelSort()
		{
			return (IComparer<GameObject>) new PanelSorter();
		}

	}

	public List<GameObject> panels;

	public bool ObjAvailable
	{
		get
		{
			return numBackgroundActive < numBackgroundObj || numMiddlegroundActive < numMiddlegroundObj;
		}
	}

	public bool PanelNeedsRecycling
	{
		get
		{
			return panels[0].transform.position.x + recycleOffset < FrogController.Instance.transform.position.x;
		}
	}
	

	void Start()
	{
		panels.Sort(PanelSorter.panelSort());
		frontLinePos = panels[panels.Count-1].transform.position.x;
		PoolableBackgroundComponent.ObjDestroyed += HandleDestroyedBackgroundObj;
	}

	public void CreateBackgroundObj(TileSet tileSet, Vector3 center)
	{
		GameObject rep = null;
		Transform parent = null;
		Vector2 pos = center + RandomisationUtilities.RandomInsideBounds(spawnArea);
		if(numBackgroundActive < numBackgroundObj && numMiddlegroundActive < numMiddlegroundObj) {
			if(Random.value < 0.5) {
				rep = RandomisationUtilities.GetRandomElement(tileSet.backgroundObj);
				parent = backLayer;
			} else {
				rep = RandomisationUtilities.GetRandomElement(tileSet.middlegroundObj);
				parent = middleLayer;
			}
		} else if (numBackgroundActive < numBackgroundObj) {
			rep = RandomisationUtilities.GetRandomElement(tileSet.backgroundObj);
			parent = backLayer;
		} else if(numMiddlegroundActive < numMiddlegroundObj) {
			rep = RandomisationUtilities.GetRandomElement(tileSet.middlegroundObj);
			parent = middleLayer;
		}
		if(rep != null && parent != null) {
			GameObject obj = PoolManager.Instance.GetPoolByRepresentative(rep).GetPooled();
			obj.transform.parent = parent;
			obj.transform.position = new Vector3(pos.x,pos.y,parent.position.z);
			obj.transform.localScale = rep.transform.localScale;
			if(parent.GetInstanceID() == middleLayer.GetInstanceID()) {
				obj.GetComponent<SpriteRenderer>().sortingLayerName = "Middleground";
				numMiddlegroundActive++;
			} else {
				obj.GetComponent<SpriteRenderer>().sortingLayerName = "Background";
				numBackgroundActive++;
			}
			obj.SetActive(true);
			print ("In tileset " + tileSet.name + " creating " + rep.name);
		}
	}

	public void RecyclePanel(float nextPos, TileSet tileSet)
	{
		GameObject p = panels[0];
		panels.RemoveAt(0);
		frontLinePos = nextPos;
		p.transform.localPosition = new Vector3(frontLinePos,p.transform.localPosition.y);
		p.renderer.material = tileSet.backdrop;
		p.renderer.material.mainTextureOffset = 
			new Vector2(Random.Range (-p.renderer.material.mainTexture.width,p.renderer.material.mainTexture.width),
			            Random.Range (-p.renderer.material.mainTexture.height,p.renderer.material.mainTexture.height));
		//Find where the new panel should go and update the panels after it to the new tileSet
		int position = -1;
		for(int i = 0; i < panels.Count; i++) {
			if(p.transform.localPosition.x < panels[i].transform.localPosition.x) {
				panels.Insert(i,p);
				position = i;
				break;
			}
		}
		if(position < 0) {
			panels.Add(p);
			position = panels.Count-1;
		}
		for(int i = position; i < panels.Count; i++) {
			panels[i].renderer.material = tileSet.backdrop;
			panels[i].transform.localPosition = p.transform.localPosition
				+ new Vector3((i-position)*panelWidth,0);
		}
		frontLinePos = panels[panels.Count-1].transform.localPosition.x;
	}

	void HandleDestroyedBackgroundObj(GameObject destroyed)
	{
		Transform parent = destroyed.transform.parent;
		if(parent != null) {

			if(parent.GetInstanceID() == middleLayer.GetInstanceID())
				numMiddlegroundActive--;
			else
				numBackgroundActive--;
		} else {
			print ("Background obj destroyed with no parent");
		}
	}




}

