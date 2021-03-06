﻿using UnityEngine;
using System.Collections;

public class InstructionsScript : MonoBehaviour {

    Sprite[] instructionFrames;
    private int currentFrame = 0;
    string joystickButton;
    string joystickAxis;
    string joystickBack;
    float oldAxis;
    float axis;

	// Use this for initialization
	void Start () {

        instructionFrames = Resources.LoadAll<Sprite>("Instructions/");
        joystickButton = "J1Jump";
        joystickAxis = "J1LHorizontal";
        joystickBack = "J1Back";

	}
	
	// Update is called once per frame
	void Update () {
        axis = Input.GetAxis(joystickAxis);
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.Space) ||
            Input.GetButtonDown(joystickButton) || (oldAxis < .5f && axis > .5f) )
        {
            currentFrame++;
            if (currentFrame > instructionFrames.Length - 1)
            {
                Application.LoadLevel("ModeSelect");
            }
            gameObject.GetComponent<SpriteRenderer>().sprite = instructionFrames[currentFrame];
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Backspace) ||
            Input.GetButtonDown(joystickBack) || ( axis < -.5f && oldAxis > -.5f ))
        {
            currentFrame--;
            if (currentFrame < 0)
            {
                Application.LoadLevel("ModeSelect");
            }
            gameObject.GetComponent<SpriteRenderer>().sprite = instructionFrames[currentFrame];
        }
        print(oldAxis + "   :   " +  axis);
        oldAxis = axis;
	
	}
}
