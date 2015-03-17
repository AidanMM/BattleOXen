using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	private bool upToggle = false;
	private bool jumpToggle = true;
	private const int MAXJUMPS = 2;
	private int numJumps = MAXJUMPS;
	private Vector2 acceleration;
	public int playerID { get; set; }

	// Use this for initialization
	void Start () {
		acceleration = new Vector2 (0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetJoystickNames ().Length > 0) {
			GetJoystickInput();
		} else {
			if (playerID == 1) {
				GetKeyboardInput();
			}
		}

		//add gravity
		acceleration.y = -(float)9.8;

		AddAccelerationForce();

		switch (playerID) {
		case 1: 
			GetComponent<SpriteRenderer>().color = Color.red;
			break;
		case 2:
			GetComponent<SpriteRenderer>().color = Color.blue;
			break;
		case 3:
			GetComponent<SpriteRenderer>().color = Color.yellow;
			break;
		case 4:
			GetComponent<SpriteRenderer>().color = Color.green;
			break;
		case -1:
			GetComponent<SpriteRenderer>().color = Color.gray;
			break;
		default:
			GetComponent<SpriteRenderer>().color = Color.white;
			break;
		}
	}

	void GetJoystickInput() {
		JoystickJump ();
		JoystickMove ();
	}

	void GetKeyboardInput() {
		KeyboardJump ();
		KeyboardMove ();
	}

	void JoystickJump() {
		string joystickButton = "J" + playerID + "Jump";
		if (Input.GetButtonDown (joystickButton) && jumpToggle) {
			gameObject.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, 5000));
			numJumps++;
			if(numJumps >= MAXJUMPS)
			{
				jumpToggle = false;
			}
		}
	}

	void JoystickMove() {
		string joystickAxis = "J" + playerID + "LHorizontal";
		//acceleration.x = Input.GetAxis (joystickAxis) * (50 + ((-gameObject.GetComponent<Rigidbody2D>().velocity.x)/3));
		acceleration.x = Input.GetAxis (joystickAxis) * 50;
	}

	void KeyboardJump() {
		if (Input.GetKeyUp (KeyCode.UpArrow) || Input.GetKeyUp (KeyCode.W)) {
			upToggle = false;
		}
		
		if (((Input.GetKeyDown (KeyCode.UpArrow) && !upToggle) || (Input.GetKeyDown (KeyCode.W) && !upToggle)) && jumpToggle) {
			upToggle = true;
			gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,3000));
			numJumps++;
			if(numJumps >= MAXJUMPS)
			{
				jumpToggle = false;
			}
		}
	}

	void KeyboardMove() {
		if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
			acceleration.x = -50;
		} else if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
			acceleration.x = 50;
		} else {
			acceleration.x = 0;
		}
	}

	void AddAccelerationForce() {
		gameObject.GetComponent<Rigidbody2D>().AddForce(acceleration);
		gameObject.GetComponent<Rigidbody2D> ().AddTorque (-acceleration.normalized.x * acceleration.magnitude * 3);
	}

	//for debugging
	void OnGUI()
	{
		//Commented out becase this is a permanent change to the gui. not a debug setting
		//GUI.Label (new Rect (10, 10, 100, 20), "X Acceleration: " + (-acceleration.normalized.x * acceleration.magnitude).ToString ());
	}

	void OnCollisionEnter2D(Collision2D collidedObject)
	{
		if (collidedObject.gameObject.tag == "ammo") {
			int objID = collidedObject.gameObject.GetComponent<Ammo> ().playerID;
			if (objID >= 0 && objID != playerID) {
				gameObject.GetComponent<Rigidbody2D> ().AddForce (collidedObject.gameObject.GetComponent<Rigidbody2D> ().velocity * 100);
			}
		}

		if (collidedObject.gameObject.tag == "stageHorizontal") {	// resets jump counter and reenables jumping if you touch a horizontal platform
			numJumps = 0;
			jumpToggle = true;
		}

		if (collidedObject.gameObject.tag == "player") {		// collides with player
		}

		if (collidedObject.gameObject.tag == "stageVertical") { // Wall jumping mechanic, commenting out for now
			numJumps = 1;
			jumpToggle = true;
		}
	}
}