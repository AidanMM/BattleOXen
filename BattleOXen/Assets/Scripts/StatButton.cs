using UnityEngine;
using System.Collections;

public class StatButton : Button {
	public string StatToMod  = "";
	public float amountToMod  = 0;


	// Use this for initialization
	public new void Start () {
		gameObject.GetComponent<SpriteRenderer>().sprite = Idle;
	}


	public new void OnMouseOver()
	{
		idle = false;
		gameObject.GetComponent<SpriteRenderer> ().sprite = Hovered;
		gameObject.GetComponent<SpriteRenderer> ().color = new Color (1 - redCounter * .2f, 1, 1 - redCounter * .2f, 1.0f);
		if (Input.GetMouseButtonDown (0)) {
			OnPress();
		}
	}
	
	public new void OnMouseExit()
	{
		gameObject.GetComponent<SpriteRenderer> ().color = new Color (1, 1 , 1, 1);
		gameObject.GetComponent<SpriteRenderer> ().sprite = Idle;
		idle = true;
	}

	public void ModStat(string stat, float amount)
	{
		/*
		public static float orbitDistance = 15;
		public static float initialOrbitCount = 3;
		public static float orbitSpeed = 3;
		public static bool useCostumRules = false;*/
		switch (stat) {
		case "distance":
			RulesScript.orbitDistance += amount;
			break;
		case "count":
			RulesScript.initialOrbitCount += amount;
			break;
		case "speed":
			RulesScript.orbitSpeed += amount;
			break;
		case "custom":
			RulesScript.useCustomRules = !RulesScript.useCustomRules;
			break;
		default:
			//Do nothing
			break;
		}


	}
	
	public override void OnPress()
	{
		ModStat (StatToMod, amountToMod);
	}
}
