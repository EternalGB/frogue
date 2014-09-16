using UnityEngine;
using System.Collections;

public abstract class PoolablePickup : PoolableObject
{

	public GameObject textPrefab;
	public string pickupText;
	float recycleOffset = 25;
	public float pointValue;


	public void ApplyEffect()
	{
		FrogController.Instance.score += pointValue;
		PickupText textObj = PoolManager.Instance.GetPoolByRepresentative(textPrefab).GetPooled().GetComponent<PickupText>();
		textObj.text.text = pickupText;
		textObj.transform.position = FrogController.Instance.transform.position + new Vector3(0,1);
		textObj.gameObject.SetActive(true);
		PickupEffect();
	}

	public abstract void PickupEffect();

	void Update()
	{
		if(enabled) {
			if(transform.position.x + recycleOffset < FrogController.Instance.distanceTraveled) {
				Debug.Log(name + " pickup being destroyed");
				Destroy();
			}
		}
	}

}

