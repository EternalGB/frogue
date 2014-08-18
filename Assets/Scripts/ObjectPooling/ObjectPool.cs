using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{



	public GameObject pooledObject;
	public int pooledAmount = 20;
	public bool growable = true;

	List<GameObject> pool;
	
	public void Init(GameObject pooledObject, int pooledAmount, bool growable) 
	{
		pool = new List<GameObject>();
		this.pooledObject = pooledObject;
		this.pooledAmount = pooledAmount;
		this.growable = growable;
		for(int i = 0; i < pooledAmount; i++) {
			GameObject obj = (GameObject)Instantiate(pooledObject);
			obj.SetActive(false);
			pool.Add(obj);
		}
	}

	public GameObject GetPooled()
	{
		for(int i = 0; i < pool.Count; i++) {
			if(pool[i] && !pool[i].activeInHierarchy) {
				return pool[i];
			}
		}

		if(growable) {
			GameObject obj = (GameObject)Instantiate(pooledObject);
			pool.Add(obj);
			return obj;
		}

		return null;
	}

	public bool ObjectAvailable()
	{
		for(int i = 0; i < pool.Count; i++) {
			if(pool[i] && !pool[i].activeInHierarchy) {
				return true;
			}
		}
		return false;
	}

}

