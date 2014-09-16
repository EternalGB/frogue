using UnityEngine;
using System.Collections;

public class TongueLength : PoolablePickup
{

	public override void PickupEffect ()
	{
		FrogController.Instance.mouth.IncreaseTongueLength(1);
	}
			
}

