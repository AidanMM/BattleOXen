using UnityEngine;
using System.Collections;

public class PlayerSelect : MonoBehaviour {
	public bool joined = false;
	public bool ready = false;
	string joystickButton;
	string joystickAxis;
	Sprite[] oxen;
	int index = 0;
	float deadzone = 0.2f;
	bool changing = false;

	// Use this for initialization
	void Start () {
		joystickButton = "J" + gameObject.name + "Jump";
		joystickAxis = "J" + gameObject.name + "LHorizontal";
		oxen = Resources.LoadAll<Sprite>("Oxen/");
		Reorder ();
	}
	
	// Update is called once per frame
	void Update () {
		if (joined) {
			if (Input.GetButtonDown(joystickButton)) {
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
			if (Input.GetButtonDown(joystickButton)) {
				UpdateSprite ();
				joined = true;
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
}
