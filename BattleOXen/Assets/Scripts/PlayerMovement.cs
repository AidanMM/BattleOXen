using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	private bool spaceToggle = false;
	private Vector2 acceleration;
	public int maxVelocity;

	// Use this for initialization
	void Start () {
		acceleration = new Vector2 (0, 0);

	}
	
	// Update is called once per frame
	void Update () {

		GetInput ();

	}


	void GetInput()
	{
		CheckJump ();
		LeftRight ();
		acceleration = Vector2.ClampMagnitude (acceleration, maxVelocity);
		gameObject.GetComponent<Rigidbody2D>().AddForce(acceleration);
	}

	void LeftRight()
	{
		if (Input.GetKey (KeyCode.LeftArrow)) {
			acceleration.x -= 50;
		} else if (Input.GetKey (KeyCode.RightArrow)) {
			acceleration.x += 50;
		} else {
			acceleration /= 1.5f;
		}

	


	}

	void CheckJump() {
		if (Input.GetKeyUp (KeyCode.Space)) {
			spaceToggle = false;
		}


		if (Input.GetKeyDown (KeyCode.Space) && !spaceToggle) {
			spaceToggle = true;
			gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,1000));
		}
	}

}
