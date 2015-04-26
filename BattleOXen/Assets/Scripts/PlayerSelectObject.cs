using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSelectObject : MonoBehaviour {
	public List<int> pid;
	public List<int> oxColors;

	// Use this for initialization
	void Start () {
		pid = new List<int> ();
		oxColors = new List<int> ();
		Object.DontDestroyOnLoad (this);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
