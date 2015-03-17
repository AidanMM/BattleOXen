﻿using UnityEngine;
using System.Collections;

public class Ammo : MonoBehaviour {

	public int playerID;
	public int state = 0;
	public Vector2 goalPoint = new Vector2(0,0);
	private Vector2 dir = new Vector2(0,0);
	//0 -> idle
	//1 -> orbiting
	//2 -> thrown

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (state == 1) {
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
		if ((collidedObject.gameObject.tag == "stageHorizontal" || collidedObject.gameObject.tag == "stageVertical" 
		    || collidedObject.gameObject.tag =="stageTop") && state == 2) {
			playerID = -1;
			state = 0;
		}
		if (collidedObject.gameObject.tag == "ammo" && 
		    	collidedObject.gameObject.GetComponent<Ammo>().playerID != playerID) {
			state = 0;
			playerID = -1;
			collidedObject.gameObject.GetComponent<Ammo>().state = 0;

		}

	}

}
