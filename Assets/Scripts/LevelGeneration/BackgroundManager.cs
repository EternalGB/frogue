using UnityEngine;
using System.Collections.Generic;


public class BackgroundManager : MonoBehaviour
{

	public float panelWidth;
	public float recycleOffset = 10;
	float frontLinePos;

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
		if(panelQueue.Peek().transform.position.x + recycleOffset < FrogController.Instance.transform.position.x)
			Recycle();
	}

	void Recycle()
	{
		GameObject p = panelQueue.Dequeue();
		frontLinePos += panelWidth;
		p.transform.localPosition = new Vector3(frontLinePos,p.transform.localPosition.y);
		panelQueue.Enqueue(p);
	}


}

