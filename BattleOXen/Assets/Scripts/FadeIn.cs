using UnityEngine;
using System.Collections;

public class FadeIn : MonoBehaviour {
	float alpha = 0;

	// Use this for initialization
	void Start () {
		this.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, alpha);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		alpha += .008f;
		this.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, alpha);
	
	}
}
