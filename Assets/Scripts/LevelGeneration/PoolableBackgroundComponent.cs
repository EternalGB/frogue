using UnityEngine;
using System.Collections;

public class PoolableBackgroundComponent : PoolableSprite
{

	public delegate void DestroyHandler(GameObject destroyed);
	public static event DestroyHandler ObjDestroyed;


	void Destroy()
	{
		ObjDestroyed(gameObject);
		base.Destroy();
	}

}

