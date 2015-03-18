using UnityEngine;
using System.Collections;

public class AmmoGhost : MonoBehaviour {
	public GameObject AmmoGhostPrefab;
	public GameObject AmmoParent { get; set; }

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.position = AmmoParent.transform.position;
	}
}
