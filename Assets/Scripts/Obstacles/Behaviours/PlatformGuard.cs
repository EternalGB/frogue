using UnityEngine;
using System.Collections;

public class PlatformGuard : MonoBehaviour
{

	public float speed;
	public Transform groundCheck;
	public Transform forwardGroundCheck;
	public LayerMask groundLayer;
	bool onGround = false;
	bool groundInFront = false;
	float travellingDir = 1;

	void Update()
	{
		onGround = Physics2D.OverlapCircle(groundCheck.position,0.1f,groundLayer);
		groundInFront = Physics2D.OverlapCircle(forwardGroundCheck.position,0.1f,groundLayer);

		if(onGround) {
			rigidbody2D.velocity = new Vector2(speed*travellingDir,rigidbody2D.velocity.y);
			if(!groundInFront) {
				travellingDir = -travellingDir;
				Flip ();
			}
		}
	}

	void Flip()
	{
		transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y,1);
	}

}

