using UnityEngine;
using System.Collections;

public class PoolableBackgroundComponent : PoolableSprite
{

	public delegate void DestroyHandler(GameObject destroyed);
	public static event DestroyHandler ObjDestroyed;


	public override void Destroy()
	{
		print ("Destroying " + transform.parent.name);
		ObjDestroyed(gameObject);
		base.Destroy();
	}

}

