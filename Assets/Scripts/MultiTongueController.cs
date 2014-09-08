using UnityEngine;
using System.Collections.Generic;

public class MultiTongueController : MonoBehaviour
{

	Queue<TongueController> tongues;


	void Start()
	{
		tongues = new Queue<TongueController>();
		TongueController[] childTongues = GetComponentsInChildren<TongueController>();
		foreach(TongueController tc in childTongues)
			tongues.Enqueue(tc);
	}

	public bool CanLick()
	{
		foreach(TongueController tc in tongues)
			if(tc.CanLick())
				return true;
		return false;
	}

	public void Lick(Vector2 pos)
	{
		if(tongues.Peek().CanLick()) {
			TongueController tongue = tongues.Dequeue();
			tongue.Lick(pos);
			tongues.Enqueue(tongue);
		}
	}

}

