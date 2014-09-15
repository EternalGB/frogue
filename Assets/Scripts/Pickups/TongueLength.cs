using UnityEngine;
using System.Collections;

public class TongueLength : PoolablePickup
{

	public override void ApplyEffect ()
	{
		FrogController.Instance.tongues.IncreaseTongueLength(1);
	}
			
}

