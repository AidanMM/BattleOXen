using UnityEngine;
using System.Collections;

public class InstructionsScript : MonoBehaviour {

    Sprite[] instructionFrames;
    private int currentFrame = 0;
    string joystickButton;
    string joystickAxis;
    string joystickBack;
    float oldAxis;

	// Use this for initialization
	void Start () {

        instructionFrames = Resources.LoadAll<Sprite>("Instructions/");
        joystickButton = "J1Jump";
        joystickAxis = "J1LHorizontal";
        joystickBack = "J1Back";

	}
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.Space) ||
            Input.GetButtonDown(joystickButton) || (oldAxis < .5f && Input.GetAxis(joystickAxis) > .5f) )
        {
            currentFrame++;
            if (currentFrame > instructionFrames.Length)
            {
                Application.LoadLevel("ModeSelect");
            }
            gameObject.GetComponent<SpriteRenderer>().sprite = instructionFrames[currentFrame];
        }
        oldAxis = Input.GetAxis(joystickAxis);

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Backspace) ||
            Input.GetButtonDown(joystickBack) || (oldAxis > -0.5f && Input.GetAxis(joystickAxis) < -.5f))
        {
            currentFrame--;
            if (currentFrame < 0)
            {
                Application.LoadLevel("ModeSelect");
            }
            gameObject.GetComponent<SpriteRenderer>().sprite = instructionFrames[currentFrame];
        }
        oldAxis = Input.GetAxis(joystickAxis);
	
	}
}
