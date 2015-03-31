using UnityEngine;
using System.Collections;

public class FadeOut : MonoBehaviour {

	float alpha = 1;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		alpha -= .01f;
		gameObject.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, alpha);
	}
}
