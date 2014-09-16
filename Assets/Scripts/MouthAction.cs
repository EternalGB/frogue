using UnityEngine;
using System.Collections;

public abstract class MouthAction : MonoBehaviour
{
	
	public float cooldown;
	public bool onCooldown = false;
	
	public virtual bool CanActivate()
	{
		return !onCooldown;
	}
	
	public void Activate()
	{
		onCooldown = true;
		StartCoroutine(Timers.Countdown(cooldown,ResetCooldown));
		Action();
	}

	public abstract void Action();

	void ResetCooldown()
	{
		onCooldown = false;
	}

}

