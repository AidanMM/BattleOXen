using UnityEngine;
using System.Collections;

public class Ammo : MonoBehaviour {

	public int playerID;
	public enum State {Idle, Orbiting, Thrown};
	public State state { get; set; }
	public Vector2 goalPoint = new Vector2(0,0);
	private Vector2 dir = new Vector2(0,0);
	public GameObject AmmoGhostPrefab;
	private GameObject AmmoGhost;
	public enum Type {Default, Bomb};
	public Type type { get; set; }
	private bool effectActivated;
	private int effectTimer;
	private Vector3 bombScale = new Vector3(100, 100, 1);
	private Vector3 effectPosition;
	public int oxColor = -1;
    public Sprite bombSprite;


	// Use this for initialization
	void Start () {
		AmmoGhost = (GameObject)Instantiate (AmmoGhostPrefab, gameObject.transform.position, Quaternion.identity);
		AmmoGhost.GetComponent<AmmoGhost> ().AmmoParent = gameObject;
              
	}
	
	// Update is called once per frame
	void Update () {
		if (state == State.Orbiting) {
			interpolateToGoal ();
		}

		if (effectActivated) {
			gameObject.transform.position = effectPosition;
			gameObject.transform.rotation = Quaternion.identity;
			Color oldColor = gameObject.GetComponent<SpriteRenderer> ().color;
			gameObject.GetComponent<SpriteRenderer> ().color = new Color (oldColor.r, oldColor.g, oldColor.b,  oldColor.a - .016f);
			if (effectTimer > 60) {
				DeactivateSelf();
			} else {
				effectTimer++;
			}
			return;
		}

		UpdateColor ();
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

	void UpdateColor() {
		switch (oxColor) {
		case 0: 
			GetComponent<SpriteRenderer>().color = Color.red;
			break;
		case 1:
			GetComponent<SpriteRenderer>().color = Color.green;
			break;
		case 2:
			GetComponent<SpriteRenderer>().color = Color.blue;
			break;
		case 3:
			GetComponent<SpriteRenderer>().color = Color.yellow;
			break;
		case -1:
			GetComponent<SpriteRenderer>().color = Color.gray;
			break;
		default:
			GetComponent<SpriteRenderer>().color = Color.white;
			break;
		}
	}

	void OnCollisionEnter2D(Collision2D collidedObject)
	{
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = -1;
		if (IsStage(collidedObject.gameObject) && state == State.Thrown &&
		    ( (collidedObject.gameObject.GetComponent<PlatformEffector2D>().oneWay == true && 
		 	gameObject.GetComponent<Rigidbody2D>().velocity.y <= 0.1f) ||
		    collidedObject.gameObject.GetComponent<PlatformEffector2D>().oneWay == false)) { // If a thrown ammo hits a wall, activate any effect unless it travels through a one way platform
			ActivateEffect();
		}

		if (collidedObject.gameObject.tag == "player" && state == State.Thrown && type == Type.Bomb && collidedObject.gameObject.name != playerID.ToString()) { 
			// If a thrown ammo hits a different player and its a bomb, activate bomb effect
			ActivateBombEffect();
		}
    }

	void OnTriggerEnter2D(Collider2D colliderObject) 
	{
		if (IsEnemyThrownAmmo (colliderObject.gameObject) && state == State.Orbiting) { // If an enemy thrown ammo hits an orbiting ammo
			ShieldAmmo(colliderObject);
		}

		if (colliderObject.tag == "player" && IsThrownAmmoWithActivatedEffect()){
			if (type == Type.Bomb) {
				Explode(colliderObject);
			}
		}
	}

	public void ActivateEffect() {
		switch (type) {
		case Type.Default:
			setIdle ();
			break;
		case Type.Bomb:
			ActivateBombEffect();
			break;
		}
	}

	private void ActivateBombEffect() {
		effectPosition = gameObject.transform.position;
		gameObject.GetComponent<BoxCollider2D> ().isTrigger = true;
		gameObject.transform.localScale = bombScale;
		effectActivated = true;
	}

	private void Explode(Collider2D colliderObject) {
		colliderObject.gameObject.GetComponent<Rigidbody2D> ().AddForce ((colliderObject.gameObject.transform.position - effectPosition).normalized * 10000);
	}

	private bool IsThrownAmmoWithActivatedEffect() {
		return state == State.Thrown && effectActivated;
	}

	private void ShieldAmmo(Collider2D colliderObject) {
		GameObject player = GameObject.Find (playerID.ToString());
		player.GetComponent<AmmoOrbit>().RemoveAmmoFromOrbit(gameObject, -1, State.Idle);
		DeactivateSelf ();

		AmmoGhost ghost = colliderObject.gameObject.GetComponent<AmmoGhost>();
		Deactivate (ghost);
	}

	private bool IsStage(GameObject obj) {
		return obj.tag == "stageHorizontal" || obj.tag == "stageVertical" || obj.tag == "stageTop";
	}

	private bool IsEnemyThrownAmmo(GameObject obj) {
		return obj.tag == "ammoGhost" &&
			obj.GetComponent<AmmoGhost> ().AmmoParent.GetComponent<Ammo> ().playerID != playerID &&
			obj.GetComponent<AmmoGhost> ().AmmoParent.GetComponent<Ammo> ().state == State.Thrown &&
			!obj.GetComponent<AmmoGhost> ().AmmoParent.GetComponent<Ammo> ().effectActivated;
	}
	public void DeactivateSelf() {
		AmmoGhost.SetActive(false);
		gameObject.SetActive(false);
	}

	private void Deactivate(AmmoGhost ghost) {
		ghost.AmmoParent.SetActive(false);
		ghost.gameObject.SetActive(false);
	}

	public void setIdle() {
		gameObject.GetComponent<BoxCollider2D> ().isTrigger = false;
		gameObject.GetComponent<Rigidbody2D>().gravityScale = 3.0f;
		state = State.Idle;
		playerID = -1;
		oxColor = -1;
	}

}
