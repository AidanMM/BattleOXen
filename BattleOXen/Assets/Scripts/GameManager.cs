using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	public GameObject PlayerPrefab;
	public GameObject DispenserPrefab;
	public int numPlayers;

	//private List<GameObject> players;

	// Use this for initialization
	void Start () {
		LimitNumPlayers ();
		CreatePlayers ();
		CreateDispenser ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	void LimitNumPlayers() {
		if (numPlayers > 3) {
			numPlayers = 3;
		} else if (numPlayers < 1) {
			numPlayers = 1;
		}
	}

	void CreatePlayers() {
		GameObject player;
		for (int i = 0; i < numPlayers; i++) {
			player = (GameObject)Instantiate (PlayerPrefab, new Vector2 ((i+1)*50, 10), Quaternion.identity);
			player.name = i.ToString();
			player.GetComponent<PlayerMovement>().playerID = i;
			print (i);
		}
	}

	void CreateDispenser() {
		Instantiate (DispenserPrefab, new Vector2 (-1, 20), Quaternion.identity);
	}
}
