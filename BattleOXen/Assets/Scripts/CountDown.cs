using UnityEngine;
using System.Collections;

public class CountDown : MonoBehaviour {

	public Sprite Three, Two, One, Go;
	int state = 0;
	private float timer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		timer += 1 * Time.deltaTime;
		gameObject.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 1 - timer % 1);

		if (Mathf.Floor (timer) == 1 && state == 0) {
			gameObject.GetComponent<SpriteRenderer>().sprite = Two;
			state++;
		}
		if (Mathf.Floor (timer) == 2 && state == 1) {
			gameObject.GetComponent<SpriteRenderer>().sprite = One;
			state++;
		}
		if (Mathf.Floor (timer) == 3 && state == 2) {
			gameObject.GetComponent<SpriteRenderer>().sprite = Go;
			state++;
		}
		if (Mathf.Floor (timer) == 4 && state == 3) {
			Destroy(gameObject);
		}


	
	}
}
