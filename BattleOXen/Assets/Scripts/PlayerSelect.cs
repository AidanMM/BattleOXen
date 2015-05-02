using UnityEngine;
using System.Collections;

public class PlayerSelect : MonoBehaviour {
	public bool joined = false;
	public bool ready = false;
	public int id;
	string joystickButton;
	string joystickAxis;
	string joystickBack;
	Sprite[] oxen;
	public int index = 0;
	float deadzone = 0.2f;
	bool changing = false;
	public GameObject PlayerStatusPrefab;
	private GameObject playerStatus;
	private Sprite joinedSprite;
	private Sprite readySprite;
	private Sprite pressSprite;
	private GameObject arrows;


	// Use this for initialization
	void Start () {
		joystickButton = "J" + gameObject.name + "Jump";
		joystickAxis = "J" + gameObject.name + "LHorizontal";
		joystickBack = "J" + gameObject.name + "Back";
		oxen = Resources.LoadAll<Sprite>("Oxen/");

		playerStatus = (GameObject)Instantiate (PlayerStatusPrefab, new Vector3 (
			gameObject.transform.position.x, 
			gameObject.transform.position.y + 4.8f, 
			gameObject.transform.position.z), Quaternion.identity);

		joinedSprite = Resources.Load<Sprite> ("PlayerSelect/joined");
		readySprite = Resources.Load<Sprite> ("PlayerSelect/ready");
		pressSprite = gameObject.GetComponent<SpriteRenderer> ().sprite;

		arrows = GameObject.Find ("a" + id);
		arrows.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (ready && Input.GetButtonDown(joystickBack)) {
			ready = false;
			UpdateStatus();
		}
		else if (joined) {
			if (Input.GetButtonDown(joystickButton) || Input.GetKeyDown(KeyCode.Space)) {
				ready = true;
				UpdateStatus();
			} else if (Input.GetButtonDown(joystickBack)) {
				joined = false;
				gameObject.transform.localScale = new Vector2(2.0f, 2.0f);
				UpdateSprite();
				UpdateStatus();
			} else if (!ready) {
				float axis = Input.GetAxis (joystickAxis);
				if (OutsideDeadzone(axis)) {
					if (!changing) {
						changing = true;
						if (axis < 0) {
							index--;
						} else if (axis > 0) {
							index++;
						}
						UpdateSprite(); 
					}
				} else {
					changing = false;
				}
			}
		} else {
			if (Input.GetButtonDown(joystickButton) || Input.GetKeyDown(KeyCode.Space)) {
				gameObject.transform.localScale = new Vector2(0.75f, 0.75f);
				UpdateSprite ();
				joined = true;
				UpdateStatus();
			} else if (Input.GetButtonDown(joystickBack)) { 
				Application.LoadLevel ("ModeSelect");
			}
		}
	}

	// Check if joystick has a new valid input
	bool OutsideDeadzone(float axis) {
		return axis > this.deadzone || axis < -this.deadzone;
	}

	// Update to the new sprite
	void UpdateSprite() {
		if (index < 0) {
			index = oxen.Length - 1;
		} else if (index >= oxen.Length) {
			index = 0;
		}

		gameObject.GetComponent<SpriteRenderer>().sprite = oxen[index];
	}

	void UpdateStatus() {
		if (ready) {
			arrows.SetActive(false);
			playerStatus.GetComponent<SpriteRenderer>().sprite = readySprite;
		} else if (joined) {
			playerStatus.GetComponent<SpriteRenderer>().sprite = joinedSprite;
			arrows.SetActive(true);
		} else {
			arrows.SetActive(false);
			playerStatus.GetComponent<SpriteRenderer>().sprite = null;
			gameObject.GetComponent<SpriteRenderer>().sprite = pressSprite;

		}
	}
}
