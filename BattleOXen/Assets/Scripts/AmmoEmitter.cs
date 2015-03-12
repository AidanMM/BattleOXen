using UnityEngine;
using System.Collections;

public class AmmoEmitter : MonoBehaviour {


	public GameObject particle;
	public Vector2 dir;
	public int mag;
	public int frequency;
	public int max = 10;
	//-1 for count will make it spawn infinetly
	private int count = 0;
	private int timer = 0;

	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {
		timer++;
		if ((count < max || max == -1) && timer % frequency == 0) {
			var tempObject = Instantiate(particle);
			tempObject.transform.position = gameObject.transform.position;
			tempObject.GetComponent<Rigidbody2D>().AddForce(dir * mag);
			count++;
		}
	}
}
