using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
public class BeamTurret : MonoBehaviour
{

	public Sprite notFiringSprite;
	public Sprite firingSprite;
	SpriteRenderer sr;
	public Transform firingPosition;
	public bool verticalFire;
	public GameObject laserBeam;
	public float laserPieceLength;
	public GameObject laserEnd;
	ObjectPool beamPool;
	ObjectPool endPool;
	List<PoolableSprite> beams;
	PoolableSprite end;
	public int minLength,maxLength;
	int laserLength;
	public bool isFiring;
	public float onTime;
	public float offTime;


	void OnEnable()
	{
		beamPool = PoolManager.Instance.GetPoolByRepresentative(laserBeam);
		endPool = PoolManager.Instance.GetPoolByRepresentative(laserEnd);
		sr = GetComponent<SpriteRenderer>();
		laserLength = Random.Range (minLength,maxLength);
		SetFiring(isFiring);
	}

	void SetFiring(bool firing)
	{
		if(firing) {
			isFiring = true;
			sr.sprite = firingSprite;
			beams = new List<PoolableSprite>(laserLength);
			for(int i = 0; i < laserLength; i++) {
				PoolableSprite beam = beamPool.GetPooled().GetComponent<PoolableSprite>();

				if(verticalFire)
					beam.transform.position = firingPosition.position + new Vector3(0,i*laserPieceLength);
				else
					beam.transform.position = firingPosition.position + new Vector3(i*laserPieceLength,0);

				beam.gameObject.SetActive(true);
				beam.SendMessage("SetTurret",transform);
				beams.Add(beam);
			}
			end = endPool.GetPooled().GetComponent<PoolableSprite>();
			if(verticalFire)
				end.transform.position = firingPosition.position + new Vector3(0,(laserLength)*laserPieceLength);
			else
				end.transform.position = firingPosition.position + new Vector3((laserLength)*laserPieceLength,0);

			end.gameObject.SetActive(true);
			end.SendMessage("SetTurret",transform);
			StartCoroutine(Timers.Countdown<bool>(onTime,SetFiring,false));
		} else {
			isFiring = false;
			sr.sprite = notFiringSprite;
			if(beams != null)
				foreach(PoolableSprite beam in beams)
					beam.Destroy();
			if(end != null)
				end.SendMessage ("Destroy");
			StartCoroutine(Timers.Countdown<bool>(offTime,SetFiring,true));
		}
	}

			
}

