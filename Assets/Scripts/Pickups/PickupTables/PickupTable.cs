using UnityEngine;
using System.Collections.Generic;

public class PickupTable : MonoBehaviour
{

	public List<PickupTableItem> pickups;

	public GameObject GetRandom()
	{
		float total = 0;
		foreach(PickupTableItem item in pickups) {
			total += item.relativeDropChance;
		}
		float selector = Random.Range (0,total);
		total = 0;
		foreach(PickupTableItem item in pickups) {
			total += item.relativeDropChance;
			if(selector <= total)
				return item.pickup;
		}
		return null;
	}

}

