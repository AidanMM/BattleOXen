using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSelectManager : MonoBehaviour {
	PlayerSelect[] players;
	List<int> pid;
	List<int> oxColors;

	// Use this for initialization
	void Start () {
		players = new PlayerSelect[4];
		pid = new List<int> ();
		oxColors = new List<int> ();

		for (int i = 1; i <= 4; i++) {
			PlayerSelect go = GameObject.Find (i.ToString()).GetComponent<PlayerSelect>();
			go.id = i;
			players[i-1] = go;
		}
	}
	
	// Update is called once per frame
	void Update () {
		PlayerSelect player;
		int joined = 0;
		int ready = 0;

		pid = new List<int> ();
		oxColors = new List<int> ();
		for (int i = 0; i < this.players.Length; i++) {
			player = players[i];
			if (player.joined) {
				joined++;
				pid.Add(player.id);
			}
			if (player.ready) {
				ready++;
				oxColors.Add (player.index);
			}
		}
		
		if (joined > 0 && joined == ready) {
			UpdatePlayerSelectObject();
			Application.LoadLevel("init");
		}
	}

	void UpdatePlayerSelectObject() {
		PlayerSelectObject p = GameObject.Find ("PlayerSelectObject").GetComponent<PlayerSelectObject> ();
		p.pid = pid;
		p.oxColors = oxColors;
	}
}
