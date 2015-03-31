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
		Color oldColor = gameObject.GetComponent<SpriteRenderer> ().color;
		gameObject.GetComponent<SpriteRenderer> ().color = new Color (oldColor.r, oldColor.g, oldColor.b,alpha);

	}

	public void Reset()
	{
		gameObject.GetComponent<SpriteRenderer> ().enabled = true;
		alpha = 1;
	}
}
