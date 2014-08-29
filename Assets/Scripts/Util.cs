using UnityEngine;
using System.Collections;

public class Util
{

	public static bool InLayerMask(LayerMask mask, int layer)
	{
		return (mask.value &1 << layer) != 0;
	}

}

