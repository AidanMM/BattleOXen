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
	double dir = 1;
	bool throwing = false;
	public int playerID { get; set; }
	float deadzone = 0.2f;
	int throwScale = 5000;
	List<GameObject> RemoveList = new List<GameObject>();
	public float secondsTimer = 0;
	public int oxColor = -1;
	private bool supered = false;

	// Use this for initialization
	void Start () {
		if (RulesScript.useCustomRules == true) {
			OrbitDistance = RulesScript.orbitDistance;
			InitialOrbitCount = (int)Mathf.Floor(RulesScript.initialOrbitCount);
			OrbitSpeed = RulesScript.orbitSpeed;
		}
		//print (RulesScript.initialOrbitCount);
		SetupOrbit ();

	}
	
	// Update is called once per frame
	void Update () {
		Orbit ();
		HandleThrown ();
		secondsTimer += 1 * Time.deltaTime;

		if (OrbitList.Count > 0) {
			if (Input.GetJoystickNames ().Length > 0) {
				GetJoystickSuper();
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
			ammo.GetComponent<Ammo>().type = Ammo.Type.Bomb;
            print(ammo.GetComponent<Ammo>().bombSprite);
            ammo.GetComponent<SpriteRenderer>().sprite = ammo.GetComponent<Ammo>().bombSprite;
			AddAmmoToOrbit(ammo);
		}
	}

	public void AddAmmoToOrbit(GameObject ammo) {
		ChangeAmmoState(ammo, Ammo.State.Orbiting);
		OrbitList.Add(ammo);
	}

	public void RemoveAmmoFromOrbit(GameObject ammo, int index, Ammo.State state) {
		if (state == Ammo.State.Orbiting) { // Something went wrong and this should never happen, so don't do anything.
			return;
		}

		ChangeAmmoState (ammo, state); // Could be thrown or idle
		if (index != -1) {
			OrbitList.RemoveAt (index);
		} else {
			OrbitList.Remove (ammo);
		}

	}

	public void RemoveAllAmmo() {
		Ammo ammo;
		for (int i = 0; i < OrbitList.Count; i++) {
			ammo = OrbitList[i].GetComponent<Ammo>();
			ammo.DeactivateSelf();
		}
		OrbitList = new List<GameObject> ();
	}

	void GetJoystickSuper() {
		string joystickLB = "J" + playerID + "LB";
		string joystickRB = "J" + playerID + "RB";
		if (Input.GetButton (joystickLB) && Input.GetButton (joystickRB) && !supered) {
			print ("SUPER");
			supered = true;
			SuperMove();
		} else {
			supered = false;
		}
	}

	void SuperMove() {
		Rigidbody2D ammoRigidBody;

		GameObject ammo;
		Vector3 ammoPos;
		Vector3 playerPos;
		Vector3 force;
		for (int i = 0; i < OrbitList.Count; i++) {
			ammo = OrbitList[i];
			ammoRigidBody = ammo.transform.GetComponent<Rigidbody2D>();
			ammoRigidBody.velocity = Vector2.zero;

			ammoPos = ammo.transform.position;
			playerPos = gameObject.transform.position;
			force = ammoPos - playerPos;
			force.Normalize();
			ammoRigidBody.AddForce(force * throwScale);

			RemoveList.Add (ammo);
			
			RemoveAmmoFromOrbit (ammo, i, Ammo.State.Thrown);
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
		int indexOf = 0;
		//Find the position that we want to put a block closest to for finding
		Vector2 targetVec = new Vector2(input.normalized.x * OrbitDistance, input.normalized.y * OrbitDistance);
		//Create a distance value so we can find the closest orbiter to our desired location
		GameObject ammo = OrbitList[0];
		float dist = Mathf.Abs(Vector2.Distance(targetVec, ammo.transform.position ));
		for (int i = 1; i < OrbitList.Count; i++) {
			float newDist = Mathf.Abs(Vector2.Distance(targetVec, OrbitList[i].transform.position));
			if(newDist < dist)
			{
				dist = newDist;
				ammo = OrbitList[i];
				indexOf = i;
			}

		}
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

		RemoveAmmoFromOrbit (ammo, indexOf, Ammo.State.Thrown);

		//ChangeAmmoState (ammo, Ammo.State.Thrown); 
		//OrbitList.RemoveAt(indexOf);
	}

	void ChangeAmmoState(GameObject ammo, Ammo.State state) {
		ammo.GetComponent<Ammo>().state = state;
		BoxCollider2D ammoColl = ammo.GetComponent<BoxCollider2D> ();

		switch (state) {
		case Ammo.State.Idle:
			ammoColl.isTrigger = false;
			ammo.GetComponent<Ammo>().playerID = -1;
			ammo.GetComponent<Ammo>().oxColor = -1;
			ammo.GetComponent<Rigidbody2D>().gravityScale = 3.0f;
			//GameManager.IgnoreCollisionWithAllPlayers(ammoColl, true);
			//Physics2D.IgnoreCollision(ammoColl, gameObject.GetComponent<BoxCollider2D>(), false);
			break;
		case Ammo.State.Orbiting:
			ammoColl.isTrigger = true;
			GameManager.IgnoreCollisionWithAllPlayers(ammoColl, false);
			ammo.GetComponent<Ammo>().playerID = playerID;
			ammo.GetComponent<Ammo>().oxColor = oxColor;
			ammo.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
			ammo.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			ammo.transform.position = gameObject.transform.position;
			break;
		case Ammo.State.Thrown:
			ammoColl.isTrigger = false;
			//Physics2D.IgnoreCollision(ammoColl, gameObject.GetComponent<BoxCollider2D>()); // Ignore collision with current player
			ammo.GetComponent<SpriteRenderer>().color = Color.green;
			ammo.GetComponent<Rigidbody2D>().gravityScale = 3.0f;
			break;
		}
	}

	void Orbit()
	{
		for (int i = 0; i < OrbitList.Count; i++) {
			/*if(i == 0)
			{
				OrbitList[i].GetComponent<SpriteRenderer>().color = Color.cyan;
			}
			else
			{
				OrbitList[i].GetComponent<SpriteRenderer>().color = Color.white;
			}*/
			if(secondsTimer >= 5)
			{
			
			OrbitList[i].GetComponent<Ammo>().goalPoint = 
				new Vector2(gameObject.transform.position.x + Mathf.Cos (((360/(float)OrbitList.Count) * ((float)i + (float)timer / 360) ) * Mathf.Deg2Rad ) * OrbitDistance, 
				            gameObject.transform.position.y + Mathf.Sin (((360/(float)OrbitList.Count) * ((float)i + (float)timer / 360) ) * Mathf.Deg2Rad ) * OrbitDistance);
			}
			else
			{
				OrbitList[i].GetComponent<Ammo>().goalPoint = 
					new Vector2(gameObject.transform.position.x + Mathf.Cos (((360/(float)OrbitList.Count) * ((float)i + (float)timer / 360) ) * Mathf.Deg2Rad ) * OrbitDistance, 
					            gameObject.transform.position.y + Mathf.Sin (((360/(float)OrbitList.Count) * ((float)i + (float)timer / 360) ) * Mathf.Deg2Rad ) * OrbitDistance);
				/*OrbitList[i].GetComponent<Ammo>().goalPoint = 
					new Vector2(gameObject.transform.position.x + Mathf.Cos (( (float)timer / 360 + i ) * Mathf.Deg2Rad ) * OrbitDistance, 
					            gameObject.transform.position.y + Mathf.Sin (( (float)timer / 360 + i ) * Mathf.Deg2Rad ) * OrbitDistance);*/
			}
			if(OrbitList[i].GetComponent<Ammo>().state == Ammo.State.Idle)
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

		if (secondsTimer >= 5) {
			timer += gameObject.GetComponent<Rigidbody2D> ().angularVelocity / 25 
				+ (OrbitSpeed * -(float)dir);
		}

	}

	void HandleThrown()
	{
		for (int i = 0; i < RemoveList.Count; i++) {
			if(Vector2.Distance(gameObject.transform.position, RemoveList[i].transform.position) > OrbitDistance)
			{
				//RemoveList[i].GetComponent<BoxCollider2D>().enabled = true;
				RemoveList.RemoveAt(i);
				i++;
				continue;
			}
		}
	}

	void AmmoKnockback(GameObject ammo) {
		gameObject.GetComponent<Rigidbody2D> ().AddForce (ammo.GetComponent<Rigidbody2D> ().velocity * 100);
		timer = 0;
	}

	void PickupAmmo(GameObject ammo) {
		// Could potentially do more things when picking up.
		AddAmmoToOrbit (ammo);
	}

	void OnCollisionEnter2D(Collision2D collidedObject)
	{
		if (collidedObject.gameObject.tag == "ammo") {
			Ammo ammo = collidedObject.gameObject.GetComponent<Ammo>();
			if((ammo.state == Ammo.State.Idle || ammo.playerID == playerID) && secondsTimer > 2.0f)
			{
				PickupAmmo(collidedObject.gameObject);
			}

		}
	}

	void OnTriggerEnter2D(Collider2D colliderObject) {
		if (colliderObject.gameObject.tag == "ammoGhost") {
			GameObject ammo = colliderObject.gameObject.GetComponent<AmmoGhost>().AmmoParent;
			//print (ammo.GetComponent<Ammo>().state);
			switch (ammo.GetComponent<Ammo> ().state) {
			case Ammo.State.Idle:
				//PickupAmmo(ammo);
				break;
			case Ammo.State.Orbiting:
				// Nothing for now.
				break;
			case Ammo.State.Thrown:
				if (ammo.GetComponent<Ammo>().playerID != playerID) {
					//AmmoKnockback(ammo);
				}
				break;
			}
		}
	}
}
