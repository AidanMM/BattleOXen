using UnityEngine;
using System.Collections;

public class FadeIn : MonoBehaviour {
	float alpha = 0;

	// Use this for initialization
	void Start () {
		this.GetComponent<SpriteRenderer> ().color = new Color (255, 255, 255, alpha);
	}
	
	// Update is called once per frame
	void Update () {
		alpha += .01f;
		this.GetComponent<SpriteRenderer> ().color = new Color (255, 255, 255, alpha);
	
	}
}
