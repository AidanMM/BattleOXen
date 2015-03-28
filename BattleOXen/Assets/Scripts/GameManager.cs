﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	public GameObject PlayerPrefab;
	public GameObject DispenserPrefab;
	public static GameObject[] Players;
	public int numPlayers;
	public static int PlayerLayer;

	//private List<GameObject> players;

	// Use this for initialization
	void Start () {
		PlayerLayer = PlayerPrefab.layer;
		LimitNumPlayers ();
		Players = new GameObject[numPlayers];
		CreatePlayers ();
		CreateDispenser ();
	}
	
	// Update is called once per frame
	void Update () {

		ResetLevelOnInput ();
	}

	void LimitNumPlayers() {
		if (numPlayers > 4) {
			numPlayers = 4;
		} else if (numPlayers < 1) {
			numPlayers = 1;
		}
	}

	void CreatePlayers() {
		GameObject player;
		for (int i = 1; i <= numPlayers; i++) {
			player = (GameObject)Instantiate (PlayerPrefab, new Vector2 ((i-1) * 50, 10), Quaternion.identity);
			player.name = i.ToString();
			player.GetComponent<PlayerMovement>().playerID = i;
			player.GetComponent<AmmoOrbit>().playerID = i;
			player.GetComponent<Rigidbody2D>().mass = (float)1.2;
			Players[i-1] = player;
		}
	}

	void CreateDispenser() {
		Instantiate (DispenserPrefab, new Vector2 (-1, 20), Quaternion.identity);
	}

	void ResetLevelOnInput()
	{
		if (Input.GetKey (KeyCode.R)) {
			Application.LoadLevel(0);
		}
	}

	public static void IgnoreCollisionWithAllPlayers(Collider2D collider, bool ignore) {
		GameObject player;
		for (int i = 0; i < Players.Length; i++) {
			player = Players[i];
			Physics2D.IgnoreCollision(collider, player.GetComponent<BoxCollider2D>(), ignore);
		}
	}
}
