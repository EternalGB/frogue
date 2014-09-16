using UnityEngine;
using System.Collections;

public class Util
{

	public static Vector3 MouseWorldPos(float zPos) 
	{
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = zPos - Camera.main.transform.position.z;
		return Camera.main.ScreenToWorldPoint(mousePos);
	}

	public static bool InLayerMask(LayerMask mask, int layer)
	{
		return (mask.value &1 << layer) != 0;
	}

	public static Vector3 KinematicPrediction2D(Vector3 p, Vector3 v, Vector3 a, float t)
	{
		Vector3 res = new Vector3();
		res.x = p.x + v.x*t;
		res.y = p.y + v.y*t + 0.5f*a.y*Mathf.Pow(t,2);
		return res;
	}

	public static Vector2 GetLaunchPower(Vector2 initVelDir, Vector2 initPos, Vector2 finalPos, float travelTime)
	{
		return new Vector2((finalPos.x - initPos.x)/(initVelDir.x*travelTime),
		                   (finalPos.y - initPos.y - 0.5f*Physics2D.gravity.y*Mathf.Pow(travelTime,2))/(initVelDir.y*travelTime));
	}

}

