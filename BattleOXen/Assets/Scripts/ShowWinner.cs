using UnityEngine;
using System.Collections;

public class ShowWinner : MonoBehaviour {

	GameObject Winner;
	float timer = 0;
	bool begin = false;
	public string DestinationLevel;
	private float scaleX = 0;
    private Sprite[] winnerFrames;

	// Use this for initialization
	void Start () {
		scaleX = transform.localScale.x;
        winnerFrames = Resources.LoadAll<Sprite>("winner/");
	}
	
	// Update is called once per frame
	void Update () {
		if (begin == true) {
			timer += 1 * Time.deltaTime;
			if(timer > 4)
			{
				Application.LoadLevel(DestinationLevel);
			}
		}

	}

	public void Begin(GameObject playerThatWon){
		GetComponent<SpriteRenderer> ().enabled = true;
		begin = true;
		Winner = playerThatWon;
		Winner.GetComponent<PlayerMovement> ().gameover = true;
        GetComponent<SpriteRenderer>().sprite = winnerFrames[Winner.GetComponent<PlayerMovement>().oxColor];

	}
}
