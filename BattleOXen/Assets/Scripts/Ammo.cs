using UnityEngine;
using System.Collections;

public class Ammo : MonoBehaviour {

	public int playerID;
	public enum State {Idle, Orbiting, Thrown};
	public State state { get; set; }
	public Vector2 goalPoint = new Vector2(0,0);
	private Vector2 dir = new Vector2(0,0);
	public GameObject AmmoGhostPrefab;

	// Use this for initialization
	void Start () {
		GameObject ghost = (GameObject)Instantiate (AmmoGhostPrefab, gameObject.transform.position, Quaternion.identity);
		ghost.GetComponent<AmmoGhost> ().AmmoParent = gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (state == State.Orbiting) {
			interpolateToGoal ();
		}

		switch (playerID) {
		case 1: 
			GetComponent<SpriteRenderer>().color = Color.red;
			break;
		case 2:
			GetComponent<SpriteRenderer>().color = Color.blue;
			break;
		case 3:
			GetComponent<SpriteRenderer>().color = Color.yellow;
			break;
		case 4:
			GetComponent<SpriteRenderer>().color = Color.green;
			break;
		case -1:
			GetComponent<SpriteRenderer>().color = Color.gray;
			break;
		default:
			GetComponent<SpriteRenderer>().color = Color.white;
			break;
		}

	}

	void interpolateToGoal()
	{
		float dist = Vector2.Distance (goalPoint, (Vector2)transform.position);
		if (Mathf.Abs (dist) > 2) {
			dir = goalPoint - (Vector2)transform.position;
			dir /= dir.magnitude / 4.0f;
			transform.position += (Vector3)dir;
		} else {
			transform.position = goalPoint;
		}
	}

	void OnCollisionEnter2D(Collision2D collidedObject)
	{
		if (IsStage(collidedObject.gameObject) && state == State.Thrown) {
			setIdle();
		}
	}

	void OnTriggerEnter2D(Collider2D colliderObject) {
		/*if (IsEnemyThrownAmmo(colliderObject.gameObject)) { // Need to work out some kinks first, like how they are still a part of the ammo orbit list.
			GameObject ammo = colliderObject.gameObject.GetComponent<AmmoGhost>().AmmoParent;
			setIdle();
			ammo.GetComponent<Ammo>().setIdle();
			gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			ammo.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			ammo.GetComponent<Rigidbody2D>().gravityScale = 3.0f;
		}*/ 
	}

	private bool IsStage(GameObject obj) {
		return obj.tag == "stageHorizontal" || obj.tag == "stageVertical" || obj.tag == "stageTop";
	}

	private bool IsEnemyThrownAmmo(GameObject obj) {
		return obj.tag == "ammoGhost" &&
			obj.GetComponent<AmmoGhost> ().AmmoParent.GetComponent<Ammo> ().playerID != playerID &&
				obj.GetComponent<AmmoGhost> ().AmmoParent.GetComponent<Ammo> ().state == State.Thrown;
	}

	public void setIdle() {
		gameObject.GetComponent<BoxCollider2D> ().isTrigger = false;
		state = State.Idle;
		playerID = -1;
	}

}
