using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AmmoOrbit : MonoBehaviour {

	public GameObject AmmoPrefab;
	List<GameObject> OrbitList= new List<GameObject> ();
	float timer = 0;
	public float OrbitDistance;
	public int InitialOrbitCount;
	public float OrbitSpeed;
	int dir = 1;
	bool throwing = false;
	public int playerID { get; set; }
	float deadzone = 0.2f;
	int throwScale = 5000;
	List<GameObject> RemoveList = new List<GameObject>();

	// Use this for initialization
	void Start () {
		SetupOrbit ();
	}
	
	// Update is called once per frame
	void Update () {
		Orbit ();
		HandleThrown ();
		if (OrbitList.Count > 0) {
			if (Input.GetJoystickNames ().Length > 0) {
				GetJoystickThrow ();
			} else {
				if (playerID == 1) {
					GetKeyboardThrow ();
				}
			}
		}
	}

	void SetupOrbit() {
		for(int i = 0; i < InitialOrbitCount; i++)
		{
			GameObject ammo = (GameObject)Instantiate(AmmoPrefab);
			ammo.GetComponent<Ammo>().playerID = playerID;
			ammo.GetComponent<Ammo>().state = 1;
			ammo.GetComponent<BoxCollider2D>().enabled = false;
			OrbitList.Add(ammo);
		}
	}

	void GetJoystickThrow() {
		string joystickRHorizontalAxis = "J" + playerID + "RHorizontal";
		string joystickRVerticalAxis = "J" + playerID + "RVertical";
		float rHorizontal = Input.GetAxis (joystickRHorizontalAxis);
		float rVertical = Input.GetAxis (joystickRVerticalAxis);
		if (OutsideDeadzone(rHorizontal, rVertical)) {
			if (!throwing) {
				throwing = true;
				Throw (new Vector2(rHorizontal, rVertical), true);
			}
		} else {
			throwing = false;
		}
	}

	void GetKeyboardThrow() {
		if (Input.GetMouseButtonDown (0)) {
			Vector2 input = (Vector2)Camera.main.ScreenToWorldPoint (Input.mousePosition);
			Throw (input, false);
		}
	}

	bool OutsideDeadzone(float rHorizontal, float rVertical) {
		return (rHorizontal > deadzone || rHorizontal < -deadzone) || (rVertical > deadzone || rVertical < -deadzone);
	}

	void Throw(Vector2 input, bool fromJoystick) {
		GameObject ammo = OrbitList[0];
		Rigidbody2D ammoRigidBody = ammo.transform.GetComponent<Rigidbody2D>();
		ammoRigidBody.velocity = Vector2.zero;
		
		if (fromJoystick) {
			input.Normalize();
			ammoRigidBody.AddForce(input * throwScale);
		} else {
			Vector2 ammoPos = (Vector2)ammo.transform.position;
			Vector2 force = input - ammoPos;
			ammoRigidBody.AddForce(force * 30);
		}
		RemoveList.Add (ammo);
		ammo.GetComponent<SpriteRenderer>().color = Color.green;
		OrbitList[0].GetComponent<Ammo>().state = 2;
		OrbitList.RemoveAt(0);
	}

	void Orbit()
	{
		for (int i = 0; i < OrbitList.Count; i++) {
			if(i == 0)
			{
				OrbitList[i].GetComponent<SpriteRenderer>().color = Color.cyan;
			}
			else
			{
				OrbitList[i].GetComponent<SpriteRenderer>().color = Color.white;
			}
			OrbitList[i].transform.position = 
				new Vector2(gameObject.transform.position.x + Mathf.Cos (((360/OrbitList.Count) * (i + timer / 360) ) * Mathf.Deg2Rad ) * OrbitDistance, 
				            gameObject.transform.position.y + Mathf.Sin (((360/OrbitList.Count) * (i + timer / 360) ) * Mathf.Deg2Rad ) * OrbitDistance);
			if(OrbitList[i].GetComponent<Ammo>().state == 0)
			{
				OrbitList.RemoveAt(i);
				i--;
				continue;
			}
			
		}
		
		if (gameObject.GetComponent<Rigidbody2D> ().velocity.x > 1) {
			dir = 1;
		}
		else if(gameObject.GetComponent<Rigidbody2D> ().velocity.x < -1) {
			dir = -1;
		}

		timer += gameObject.GetComponent<Rigidbody2D>().angularVelocity / 25 
			+ (OrbitSpeed * -dir);

	}

	void HandleThrown()
	{
		for (int i = 0; i < RemoveList.Count; i++) {
			if(Vector2.Distance(gameObject.transform.position, RemoveList[i].transform.position) > OrbitDistance)
			{
				RemoveList[i].GetComponent<BoxCollider2D>().enabled = true;
				RemoveList.RemoveAt(i);
				i++;
				continue;
			}
		}
	}


	void OnCollisionEnter2D(Collision2D collidedObject)
	{
		if (collidedObject.gameObject.tag == "ammo") {
			int ammoState = collidedObject.gameObject.GetComponent<Ammo>().state;

			if(ammoState == 0 || collidedObject.gameObject.GetComponent<Ammo>().playerID == playerID)
			{
				collidedObject.gameObject.GetComponent<BoxCollider2D>().enabled = false;
				OrbitList.Add(collidedObject.gameObject);
				collidedObject.gameObject.GetComponent<Ammo>().state = 1;
				collidedObject.gameObject.GetComponent<Ammo>().playerID = playerID;
			}

		}
	}
}
