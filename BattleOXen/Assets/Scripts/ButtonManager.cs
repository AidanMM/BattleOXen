using UnityEngine;
using System.Collections;
using System.Linq;

public class ButtonManager : MonoBehaviour {

	IOrderedEnumerable<GameObject> ButtonList;
	int index = 0;
	string joystickAxis = "J1LVertical";
	float axis;
	float oldAxis;
	// Use this for initialization
	void Start () {
		GameObject[] buttonListGet = GameObject.FindGameObjectsWithTag ("Button");
		ButtonList = buttonListGet.OrderBy(button => -button.transform.position.y);
	}
	
	// Update is called once per frame
	void Update () {

		axis = Input.GetAxis (joystickAxis);
		PlayerInput ();
		oldAxis = axis;
		for(int i = 0; i < ButtonList.Count(); i++)
		{
			if( i == index)
			{
				ButtonList.ElementAt(i).GetComponent<SpriteRenderer>().color = new Color(0,1,0,1);
			}
			else
			{
				ButtonList.ElementAt(i).GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);
			}
		}
	
	}

	void PlayerInput()
	{


		if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) ||
		   (axis < -.5f && oldAxis > -.5f))
		{
			index++;
			index %= ButtonList.Count ();
		}
		else if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)||
		        (axis > .5f && oldAxis < .5f))
		{
			index--;
			if(index < 0)
			{
				index = ButtonList.Count() - 1;
			}
		}
		if (Input.GetKeyDown (KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) {
			ButtonList.ElementAt(index).GetComponent<Button>().NextLevel();
		}

	}
}
