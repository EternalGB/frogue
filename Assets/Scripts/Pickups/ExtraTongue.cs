using UnityEngine;
using System.Collections;

public class ExtraTongue : PoolablePickup
{

	public override void ApplyEffect ()
	{
		FrogController.Instance.tongues.AddTongue();
	}
			
}

