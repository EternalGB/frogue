using UnityEngine;
using System.Collections.Generic;


public class BackgroundManager : MonoBehaviour
{

	public PlatformManager pm;
	public float panelWidth;
	public float recycleOffset = 10;
	float frontLinePos;
	public Transform backLayer;
	public Transform middleLayer;
	public int numBackgroundObj;
	public int numMiddlegroundObj;
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
	Queue<GameObject> panelQueue;

	void Start()
	{
		panels.Sort(PanelSorter.panelSort());
		panelQueue = new Queue<GameObject>();
		foreach(GameObject go in panels) {
			if(go.transform.localPosition.x > frontLinePos)
				frontLinePos = go.transform.localPosition.x;
			panelQueue.Enqueue(go);
		}
	}

	void Update()
	{
		//spawn some background objects
		if(Random.value < spawnChance*Time.deltaTime) {
			Vector2 pos = FrogController.Instance.transform.position + RandomisationUtilities.RandomInsideBounds(spawnArea);
			GameObject rep;
			Transform parent;
			if(Random.value < 0.5) {
				rep = RandomisationUtilities.GetRandomElement(pm.tileSet.backgroundObj);
				parent = backLayer;
			} else {
				rep = RandomisationUtilities.GetRandomElement(pm.tileSet.middlegroundObj);
				parent = middleLayer;
			}
			GameObject obj = PoolManager.Instance.GetPoolByRepresentative(rep).GetPooled();
			obj.transform.parent = parent;
			obj.transform.position = Vector3.zero;
			obj.transform.localPosition = pos;
			obj.SetActive(true);
		}

		//recycle the panels
		if(panelQueue.Peek().transform.position.x + recycleOffset < FrogController.Instance.transform.position.x)
			Recycle();
	}



	void Recycle()
	{
		GameObject p = panelQueue.Dequeue();
		frontLinePos += panelWidth;
		p.transform.localPosition = new Vector3(frontLinePos,p.transform.localPosition.y);
		p.renderer.material.mainTexture = pm.tileSet.backdrop;
		panelQueue.Enqueue(p);
	}


}

