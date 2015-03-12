using UnityEngine;
using System.Collections;

public class AmmoEmitter : MonoBehaviour {


	public GameObject particle;
	public Vector2 dir;
	public int mag;
	public int frequency;
	public int max = 10;
	private int count = 0;
	private int timer = 0;

	// Use this for initialization
	void Start () {
		dir *= mag;

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
