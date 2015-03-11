using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	private bool upToggle = false;
	private Vector2 acceleration;
	public int maxVelocity;

	// Use this for initialization
	void Start () {
		acceleration = new Vector2 (0, 0);

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		GetInput ();
	}


	void GetInput()
	{
		CheckJump ();
		LeftRight ();
		//acceleration = Vector2.ClampMagnitude (acceleration, maxVelocity);
		gameObject.GetComponent<Rigidbody2D>().AddForce(acceleration);
		gameObject.GetComponent<Rigidbody2D> ().AddTorque (-acceleration.normalized.x * acceleration.magnitude);
	}

	void LeftRight()
	{
		if (Input.GetKey (KeyCode.LeftArrow)) {
			acceleration.x = -50;

		} else if (Input.GetKey (KeyCode.RightArrow)) {
			acceleration.x = 50;
		} else {
			acceleration.x = 0;
		}
	}

	void CheckJump() {
		if (Input.GetKeyUp (KeyCode.UpArrow)) {
			upToggle = false;
		}
		
		if (Input.GetKeyDown (KeyCode.UpArrow) && !upToggle) {
			upToggle = true;
			gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,1000));
		}
	}

}
