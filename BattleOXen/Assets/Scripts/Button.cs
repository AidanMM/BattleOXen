using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {

	public Sprite Idle;
	public Sprite Hovered;
	public GameObject projectile;
	public string DestinationLevel;
	private float redCounter = 0.0f;
	bool idle = true;
	

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<SpriteRenderer>().sprite = Idle;
	}
	
	// Update is called once per frame
	void Update () {
		if (idle == true && redCounter > 0) {
			redCounter -= .3f;
		}
		else if (idle == false) {
			redCounter -= .02f;
		}
		if(redCounter < 0)
		{
			redCounter = 0.0f;
		}


	}

	void Fire(Vector2 target, Vector2 startPos)
	{
		GameObject ammo = (GameObject)Instantiate(projectile);
		ammo.transform.position = startPos;
		target -= (Vector2)ammo.transform.position ;
		ammo.gameObject.GetComponent<Rigidbody2D>().AddForce(target.normalized * 3000);
		ammo.GetComponent<Ammo> ().enabled = false;

	}


	void OnCollisionEnter2D(Collision2D collidedObject)
	{
		if (collidedObject.gameObject.tag == "ammo") {
			gameObject.GetComponent<Rigidbody2D> ().AddForce(collidedObject.gameObject.GetComponent<Rigidbody2D> ().velocity * 10);
			if(idle == false)
			{
			redCounter++;
				if(redCounter > 10)
				{
					Application.LoadLevel(DestinationLevel);
				}
			}
		}
		

	}

	void OnMouseOver()
	{
		idle = false;
		gameObject.GetComponent<SpriteRenderer> ().sprite = Hovered;
		gameObject.GetComponent<SpriteRenderer> ().color = new Color (1 - redCounter * .2f, 1, 1 - redCounter * .2f, 1.0f);
		if(Input.GetMouseButtonDown(1))
		{
			Fire (new Vector2(transform.position.x, transform.position.y), new Vector2(transform.position.x + (40 * (Random.Range(0,2) * 2 - 1)) , transform.position.y + Random.Range(-5,10)));
		}
		if (Input.GetMouseButtonDown (0)) {
			Application.LoadLevel(DestinationLevel);
		}
	}

	void OnMouseExit()
	{
		gameObject.GetComponent<SpriteRenderer> ().color = new Color (1, 1 , 1, 1);
		gameObject.GetComponent<SpriteRenderer> ().sprite = Idle;
		idle = true;
	}

	public void NextLevel()
	{
		Application.LoadLevel(DestinationLevel);
	}
	
}
