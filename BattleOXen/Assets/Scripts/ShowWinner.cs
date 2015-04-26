using UnityEngine;
using System.Collections;

public class ShowWinner : MonoBehaviour {

	GameObject Winner;
	float timer = 0;
	bool begin = false;
	public string DestinationLevel;
	private float scaleX = 0;

	// Use this for initialization
	void Start () {
		scaleX = transform.localScale.x;
	}
	
	// Update is called once per frame
	void Update () {
		if (begin == true) {
			timer += 1 * Time.deltaTime;
			if(timer % 2 < 1)
				transform.localScale = new Vector3(scaleX * (1 + timer % 1), transform.localScale.y, transform.localScale.z);
			else
				transform.localScale = new Vector3(scaleX * (2 - timer % 1), transform.localScale.y, transform.localScale.z);
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

		switch (Winner.GetComponent<PlayerMovement>().oxColor) {
		case 0: 
			GetComponent<SpriteRenderer>().color =  Color.red;
			break;
		case 1:
			GetComponent<SpriteRenderer>().color =  Color.green;
			break;
		case 2:
			GetComponent<SpriteRenderer>().color =  Color.blue;
			break;
		case 3:
			GetComponent<SpriteRenderer>().color =  Color.yellow;
			break;
		case -1:
			GetComponent<SpriteRenderer>().color =  Color.gray;
			break;
		default:
			GetComponent<SpriteRenderer>().color =  Color.white;
			break;
		}
	}
}
