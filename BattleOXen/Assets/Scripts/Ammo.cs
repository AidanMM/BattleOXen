using UnityEngine;
using System.Collections;

public class Ammo : MonoBehaviour {

	public int playerID;
	public int state = 0;
	//0 -> idle
	//1 -> orbiting
	//2 -> thrown

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		switch (playerID) {
		case 0: 
			GetComponent<SpriteRenderer>().color = Color.green;
			break;
		case 2:
			GetComponent<SpriteRenderer>().color = Color.blue;
			break;
		case 3:
			GetComponent<SpriteRenderer>().color = Color.yellow;
			break;
		case 4:
			GetComponent<SpriteRenderer>().color = Color.red;
			break;
		case -1:
			GetComponent<SpriteRenderer>().color = Color.gray;
			break;
		default:
			GetComponent<SpriteRenderer>().color = Color.white;
			break;
		}
	}

	void OnCollisionEnter2D(Collision2D collidedObject)
	{
		if (collidedObject.gameObject.tag == "stage" && state == 2) {
			playerID = -1;
			state = 0;
		}
		if (collidedObject.gameObject.tag == "ammo" && state == 2 && collidedObject.gameObject.GetComponent<Ammo>().state == 1) {
			state = 0;
			playerID = -1;
			collidedObject.gameObject.GetComponent<Ammo>().state = 0;

		}

	}

}
