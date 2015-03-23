using UnityEngine;
using System.Collections;

public class NextSceneOnPress : MonoBehaviour {
	
	public string NextScene;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.Space) ) {
			Application.LoadLevel(NextScene);
		}
		
	}
}
