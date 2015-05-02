using UnityEngine;
using System.Collections;

public class OffScreenDeath : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 toCamera = Camera.main.WorldToScreenPoint (transform.position);
		if (toCamera.x < -gameObject.GetComponent<SpriteRenderer>().bounds.size.x / 3 || toCamera.x > Screen.width + gameObject.GetComponent<SpriteRenderer>().bounds.size.x / 3
		    || toCamera.y < -100 || toCamera.y > 1000) {
			gameObject.SetActive(false);
		}
	}
}
