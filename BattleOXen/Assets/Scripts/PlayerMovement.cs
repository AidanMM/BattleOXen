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
		if (gameObject.name == "0") {
			GetInput ();
		}
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
		if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
			acceleration.x = -50;
			//acceleration.x = -(50 + (-acceleration.normalized.x * acceleration.magnitude));

		} else if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
			acceleration.x = 50;
			//acceleration.x = (50 + (-acceleration.normalized.x * acceleration.magnitude));
		} else {
			acceleration.x = 0;
		}
	}

	void CheckJump() {
		if (Input.GetKeyUp (KeyCode.UpArrow) || Input.GetKeyUp (KeyCode.W)) {
			upToggle = false;
		}
		
		if ((Input.GetKeyDown (KeyCode.UpArrow) && !upToggle) || (Input.GetKeyDown (KeyCode.W) && !upToggle)) {
			upToggle = true;
			gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,1000));
		}
	}

	//for debugging
	void OnGUI()
	{
		GUI.Label (new Rect (10, 10, 100, 20), "X Acceleration: " + (-acceleration.normalized.x * acceleration.magnitude).ToString ());
	}

}
