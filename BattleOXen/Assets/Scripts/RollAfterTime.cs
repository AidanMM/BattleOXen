using UnityEngine;
using System.Collections;

public class RollAfterTime : MonoBehaviour {

	float timer = 0;
	public float RollTime;
	public float Direction;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		timer++;
		if (timer > RollTime) {
			gameObject.GetComponent<Rigidbody2D>().AddTorque( 100 * Direction);
		}
	}
}
