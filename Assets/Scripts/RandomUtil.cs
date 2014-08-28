using UnityEngine;
using System.Collections.Generic;

public class RandomUtil
{

	public static Vector3 RandomInsideBounds(Bounds bounds)
	{
		return bounds.center + new Vector3(Random.Range (-bounds.extents.x,bounds.extents.x),
		                                   Random.Range (-bounds.extents.y,bounds.extents.y),
		                                   Random.Range (-bounds.extents.z,bounds.extents.z));
	}
	
	public static T GetRandomElement<T>(IList<T> list)
	{
		return list[Random.Range (0,list.Count)];
	}

	public static T GetRandomElement<T>(T[] array)
	{
		return array[Random.Range (0,array.Length)];
	}
}

