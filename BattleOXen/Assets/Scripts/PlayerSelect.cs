﻿using UnityEngine;
using System.Collections;

public class PlayerSelect : MonoBehaviour {
	public bool joined = false;
	public bool ready = false;
	public int id;
	string joystickButton;
	string joystickAxis;
	Sprite[] oxen;
	public int index = 0;
	float deadzone = 0.2f;
	bool changing = false;
	public GameObject PlayerStatusPrefab;
	private GameObject playerStatus;
	private Sprite joinedSprite;
	private Sprite readySprite;


	// Use this for initialization
	void Start () {
		joystickButton = "J" + gameObject.name + "Jump";
		joystickAxis = "J" + gameObject.name + "LHorizontal";
		oxen = Resources.LoadAll<Sprite>("Oxen/");
		Reorder ();

		playerStatus = (GameObject)Instantiate (PlayerStatusPrefab, new Vector3 (
			gameObject.transform.position.x, 
			gameObject.transform.position.y + 4.8f, 
			gameObject.transform.position.z), Quaternion.identity);

		joinedSprite = Resources.Load<Sprite> ("PlayerSelect/joined");
		readySprite = Resources.Load<Sprite> ("PlayerSelect/ready");
	}
	
	// Update is called once per frame
	void Update () {
		if (joined) {
			if (Input.GetButtonDown(joystickButton) || Input.GetKeyDown(KeyCode.Space)) {
				ready = !ready;
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
				UpdateSprite ();
				joined = true;
				UpdateStatus();
			}
		}
	}

	// Reorder the oxen array
	void Reorder() {
		Sprite r = oxen [2];
		oxen [2] = oxen[0];
		oxen [0] = r;
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
			playerStatus.GetComponent<SpriteRenderer>().sprite = readySprite;
		} else if (joined) {
			playerStatus.GetComponent<SpriteRenderer>().sprite = joinedSprite;
		} else {
			playerStatus.GetComponent<SpriteRenderer>().sprite = null;
		}
	}
}
