using UnityEngine;
using System.Collections.Generic;

public class MouthController : MonoBehaviour
{

	Queue<MouthAction> actions;


	void Start()
	{
		actions = new Queue<MouthAction>();
		MouthAction[] children = GetComponentsInChildren<MouthAction>();
		foreach(MouthAction tc in children)
			actions.Enqueue(tc);
	}

	public bool CanActivate()
	{
		foreach(MouthAction ma in actions)
			if(ma.CanActivate())
				return true;
		return false;
	}

	public void DoAvailableMouthAction()
	{
		if(actions.Peek().CanActivate()) {
			MouthAction action = actions.Dequeue();
			action.Activate();
			actions.Enqueue(action);
		}
	}

	/*
	public void AddTongue()
	{
		MouthAction tc = tongues.Peek();
		GameObject newTongue = (GameObject)GameObject.Instantiate(tc.gameObject,transform.position,Quaternion.identity);
		newTongue.transform.parent = transform;
		newTongue.transform.localScale = Vector3.one;
		MouthAction newTc = newTongue.GetComponent<MouthAction>();
		newTc.onCooldown = false;
		newTc.isLicking = false;
		tongues.Enqueue(newTongue.GetComponent<MouthAction>());
	}
	*/

	public void IncreaseTongueLength(float amount)
	{
		//TODO
		//foreach(MouthAction tongue in tongues)
			//tongue.maxDist += amount;
	}

}

