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
	bool throwToggle = false;
	int playerID = -1;

	// Use this for initialization
	void Start () {
		playerID = gameObject.GetComponent<PlayerMovement> ().playerID;
		for(int i = 0; i < InitialOrbitCount; i++)
		{
			GameObject ammo = (GameObject)Instantiate(AmmoPrefab);
			ammo.GetComponent<Ammo>().playerID = playerID;
			ammo.GetComponent<Ammo>().state = 1;
			OrbitList.Add(ammo);
		}
	}
	
	// Update is called once per frame
	void Update () {
		Orbit ();
		GetInput ();
	}

	void GetInput()
	{
		float rHorizontal = Input.GetAxis ("RHorizontal");
		float rVertical = Input.GetAxis ("RVertical");
		//print (rHorizontal + " " + rVertical);
		Vector2 pos;
		if (Input.GetMouseButtonDown (0)) {
			pos = (Vector2)Camera.main.ScreenToWorldPoint (Input.mousePosition);
			ThrowAmmoFromMouse (pos);
		} else if ((rHorizontal > 0.2 || rHorizontal < -0.2) || (rVertical > 0.2 || rVertical < -0.2)) {
			pos = new Vector2 (rHorizontal, -rVertical);
			ThrowAmmoFromJoystick (pos);
		} else {
			throwToggle = false;
		}
	}

	void Throw()
	{
		if (OrbitList.Count != 0) {
			OrbitList[0].transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			OrbitList[0].transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(1000 * dir, 0));
			OrbitList[0].GetComponent<SpriteRenderer>().color = Color.green;
			OrbitList[0].GetComponent<BoxCollider2D>().enabled = true;
			OrbitList[0].GetComponent<Ammo>().state = 2;
			OrbitList.RemoveAt(0);

		}
	}

	//TODO: Refactor both of these to one method. Didn't have time.
	void ThrowAmmoFromMouse(Vector2 mousePos) {
		if (OrbitList.Count != 0) {
			GameObject ammo = OrbitList[0];
			Rigidbody2D ammoRigidBody = ammo.transform.GetComponent<Rigidbody2D>();
			Vector2 ammoPos = (Vector2)ammo.transform.position;
			Vector2 force = mousePos - ammoPos;

			ammoRigidBody.velocity = Vector2.zero;
			ammoRigidBody.AddForce(force * 30);
			ammo.GetComponent<SpriteRenderer>().color = Color.green;
			//ammo.GetComponent<BoxCollider2D>().enabled = true;

			OrbitList[0].GetComponent<Ammo>().state = 2;
			OrbitList.RemoveAt(0);
		}
	}

	void ThrowAmmoFromJoystick(Vector2 direction) {
		if (OrbitList.Count != 0 && !throwToggle) {
			GameObject ammo = OrbitList[0];
			Rigidbody2D ammoRigidBody = ammo.transform.GetComponent<Rigidbody2D>();
			Vector2 ammoPos = (Vector2)ammo.transform.position;

			//TODO: Get this calculation correct. Didn't have time.
			direction.Normalize();
			
			ammoRigidBody.velocity = Vector2.zero;
			ammoRigidBody.AddForce(direction * 5000);
			ammo.GetComponent<SpriteRenderer>().color = Color.green;
			//ammo.GetComponent<BoxCollider2D>().enabled = true;

			throwToggle = true;
			OrbitList[0].GetComponent<Ammo>().state = 2;
			OrbitList.RemoveAt(0);
		}
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


	void OnCollisionEnter2D(Collision2D collidedObject)
	{
		if (collidedObject.gameObject.tag == "ammo") {
			int ammoState = collidedObject.gameObject.GetComponent<Ammo>().state;
			//collidedObject.gameObject.GetComponent<BoxCollider2D>().enabled = false;
			if(ammoState == 0)
			{
			
				OrbitList.Add(collidedObject.gameObject);
				collidedObject.gameObject.GetComponent<Ammo>().state = 1;
				collidedObject.gameObject.GetComponent<Ammo>().playerID = playerID;
			}

		}
	}
}
