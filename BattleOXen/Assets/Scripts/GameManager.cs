using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	public GameObject PlayerPrefab;
	public static GameObject[] Players;
	private int numPlayers;
	private PlayerSelectObject p;

	//private List<GameObject> players;

	// Use this for initialization
	void Start () {
		p = GameObject.Find ("PlayerSelectObject").GetComponent<PlayerSelectObject>();

		GetNumPlayers ();
		Players = new GameObject[numPlayers];
		CreatePlayers ();
	}
	
	// Update is called once per frame
	void Update () {
		ResetLevelOnInput ();
	}

	public void LastManStanding()
	{
		int aliveIndex = 0;
		int deadPlayers = 0;
		for(int i = 0; i < numPlayers; i++)
		{
			if(Players[i].GetComponent<PlayerMovement>().enabled == true)
			{
				aliveIndex = i;
			}
			else
			{
				deadPlayers++;
			}
		}

		if (deadPlayers == numPlayers - 1) {
			GameObject.FindGameObjectWithTag("Winner").GetComponent<ShowWinner>().Begin(Players[aliveIndex]);
		}
	}

	void GetNumPlayers() {
		if (Input.GetJoystickNames ().Length > 0) {
			numPlayers = p.pid.Count;
		} else {
			numPlayers = 4;
		}
	}

	void CreatePlayers() {
		GameObject player;
		for (int i = 0; i < numPlayers; i++) {
			int id = p.pid[i];
			int offset = (i+1) % 2 == 0 ? 1 : -1;
			float x = offset * 60 * ((int)(i+2)/2);
			player = (GameObject)Instantiate (PlayerPrefab, new Vector2 (x, -80), Quaternion.identity);
			player.name = id.ToString();
			player.GetComponent<PlayerMovement>().playerID = id;
			player.GetComponent<PlayerMovement>().oxColor = p.oxColors[i];
			switch(p.oxColors[i])
			{
			case 0:
				player.GetComponent<SpriteRenderer>().sprite = Resources.Load("Oxen/redOx", typeof(Sprite)) as Sprite;
				break;
			case 1:
				player.GetComponent<SpriteRenderer>().sprite = Resources.Load("Oxen/greenOx", typeof(Sprite)) as Sprite;
				break;
			case 2:
				player.GetComponent<SpriteRenderer>().sprite = Resources.Load("Oxen/blueOx", typeof(Sprite)) as Sprite;
				break;
			case 3:
				player.GetComponent<SpriteRenderer>().sprite = Resources.Load("Oxen/yellowOx", typeof(Sprite)) as Sprite;
				break;
			case 4:
				player.GetComponent<SpriteRenderer>().sprite = Resources.Load("Oxen/greenOx", typeof(Sprite)) as Sprite;
				break;
		   default:
				player.GetComponent<SpriteRenderer>().sprite = Resources.Load("Oxen/redOx", typeof(Sprite)) as Sprite;
				break;
			}
			player.GetComponent<AmmoOrbit>().playerID = id;
			player.GetComponent<AmmoOrbit>().oxColor = p.oxColors[i];
			player.GetComponent<Rigidbody2D>().mass = (float)1.2;
			Players[i] = player;
		}
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
