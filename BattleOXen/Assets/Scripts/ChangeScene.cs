using UnityEngine;
using System.Collections;

public class ChangeScene : MonoBehaviour {

	public float TransitionTime;
	public string DestinationLevel;

	private float timePast = 0;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		timePast++;

		if (timePast > TransitionTime) {
			Application.LoadLevel(DestinationLevel);
		}
	
	}
}
