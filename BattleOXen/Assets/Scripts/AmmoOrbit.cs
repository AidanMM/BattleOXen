using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AmmoOrbit : MonoBehaviour {

	List<GameObject> OrbitList= new List<GameObject> ();
	float timer = 0;
	public float OrbitDistance;
	public int InitialOrbitCount;
	public float OrbitSpeed;
	int dir = 1;

	// Use this for initialization
	void Start () {
		GameObject ammoBlock = GameObject.FindGameObjectWithTag ("ammo");
		for(int i = 0; i < InitialOrbitCount; i++)
		{
			var tempObject = Instantiate(ammoBlock);
			OrbitList.Add(tempObject);
		}
	}
	
	// Update is called once per frame
	void Update () {

		Orbit ();
		GetInput ();
	}

	void GetInput()
	{
		if (Input.GetKeyDown (KeyCode.Space)) {
			Throw();

		}
	}

	void Throw()
	{
		if (OrbitList.Count != 0) {
			OrbitList[0].transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			OrbitList[0].transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(1000 * dir, 0));
			OrbitList[0].GetComponent<SpriteRenderer>().color = Color.green;
			OrbitList[0].GetComponent<BoxCollider2D>().enabled = true;
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
			OrbitList.Add(collidedObject.gameObject);
		}
	}
}
