using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	private bool upToggle = false;
	private bool jumpToggle = true;
	private const int MAXJUMPS = 2;
	private int numJumps = MAXJUMPS;
	private Vector2 acceleration;
	public int playerID { get; set; }
	public int oxColor = -1;
	public Vector2 initPos;
	public bool gameover = true;
	public int lives = 3;

	// Use this for initialization
	void Start () {
		Invoke ("Init", 3);
	}
	
	// Update is called once per frame
	void Update () {
		if (!gameover) {
			if (Input.GetJoystickNames ().Length > 0) {
				GetJoystickInput ();
			} else {
				if (playerID == 1) {
					GetKeyboardInput ();
				}
			}
			acceleration.y = -(float)25;

			AddAccelerationForce ();

			if (transform.position.magnitude > 200) {
				GameObject.FindGameObjectWithTag ("KO").GetComponent<FadeOut> ().Reset ();
				switch (oxColor) {
				case 0: 
					GameObject.FindGameObjectWithTag ("KO").GetComponent<SpriteRenderer> ().color = Color.red;
					break;
				case 1:
					GameObject.FindGameObjectWithTag ("KO").GetComponent<SpriteRenderer> ().color = Color.green;
					break;
				case 2:
					GameObject.FindGameObjectWithTag ("KO").GetComponent<SpriteRenderer> ().color = Color.blue;
					break;
				case 3:
					GameObject.FindGameObjectWithTag ("KO").GetComponent<SpriteRenderer> ().color = Color.yellow;
					break;
				case -1:
					GameObject.FindGameObjectWithTag ("KO").GetComponent<SpriteRenderer> ().color = Color.gray;
					break;
				default:
					GameObject.FindGameObjectWithTag ("KO").GetComponent<SpriteRenderer> ().color = Color.white;
					break;
				}

				lives--;
				gameObject.SetActive (false);
				gameObject.GetComponent<AmmoOrbit> ().RemoveAllAmmo ();
				if (lives > 0) {
					Invoke("Respawn", 2);
				} else {
					GameObject.FindGameObjectWithTag ("MainGO").GetComponent<GameManager> ().LastManStanding ();
				}
			}
		}
	}

	void Respawn() {
		gameObject.transform.position = initPos;
		gameObject.SetActive (true);
	}

	void Init() {
		acceleration = new Vector2 (0, 0);
		initPos = gameObject.transform.position;
		gameover = false;
	}

	void GetJoystickInput() {
		JoystickJump ();
		JoystickMove ();
		//JoyStickDashLeft ();
		//JoyStickDashRight ();
	}

	void GetKeyboardInput() {
		KeyboardJump ();
		KeyboardMove ();
	}

	void JoystickJump() {
		string joystickButton = "J" + playerID + "Jump";
		if (Input.GetButtonDown (joystickButton) && jumpToggle) {
			gameObject.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, 4000));
			numJumps++;
			if(numJumps >= MAXJUMPS)
			{
				jumpToggle = false;
			}
		}
	}

	void JoystickMove() {
		string joystickAxis = "J" + playerID + "LHorizontal";
		float axis = Input.GetAxis (joystickAxis);
		acceleration.x = axis * (150 + (-axis * gameObject.GetComponent<Rigidbody2D> ().velocity.x));

	}

	/*void JoyStickDashLeft(){
		string joystickButton = "J" + playerID + "DashLeft";
		if (Input.GetButtonDown (joystickButton)) {
			gameObject.GetComponent<Rigidbody2D> ().AddForce (new Vector2(4000,0));
		}
	}
	
	void JoyStickDashRight(){
		string joystickButton = "J" + playerID + "DashRight";
		if (Input.GetButtonDown (joystickButton)) {
			gameObject.GetComponent<Rigidbody2D> ().AddForce (new Vector2(4000,0));
		}
	}*/

	void KeyboardJump() {
		if (Input.GetKeyUp (KeyCode.UpArrow) || Input.GetKeyUp (KeyCode.W)) {
			upToggle = false;
		}
		
		if (((Input.GetKeyDown (KeyCode.UpArrow) && !upToggle) || (Input.GetKeyDown (KeyCode.W) && !upToggle)) && jumpToggle) {
			upToggle = true;
			gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,4000));
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
				gameObject.GetComponent<AmmoOrbit>().secondsTimer = 0;
			}
		}

		if (collidedObject.gameObject.tag == "stageHorizontal") {	// resets jump counter and reenables jumping if you touch a horizontal platform
			numJumps = 0;
			jumpToggle = true;
		}

		if (collidedObject.gameObject.tag == "player") {		// collides with player
			if (numJumps > 0)
			{
				numJumps -= 1;
			}
			jumpToggle = true;
		}

		if (collidedObject.gameObject.tag == "stageVertical") { // Wall jumping mechanic, commenting out for now
			numJumps = 1;
			jumpToggle = true;
		}
	}
}