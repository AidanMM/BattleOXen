using UnityEngine;
using System.Collections;

public class AmmoEmitter : MonoBehaviour {


	public GameObject AmmoPrefab;
	public Vector2 dir;
	public Vector2 varying;
	public int mag;
	public int frequency;
	public int max = 10;
	public int tourque;
	//-1 for count will make it spawn infinetly
	private int count = 0;
	private int timer = 0;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		timer++;
		if ((count < max || max == -1) && timer % frequency == 0) {
			GameObject ammo = (GameObject)Instantiate(AmmoPrefab);
			ammo.transform.position = gameObject.transform.position;
			Vector2 randomDir = new Vector2(Random.Range(0, varying.x), Random.Range(0,varying.y));
			ammo.gameObject.GetComponent<Rigidbody2D>().AddForce((dir + randomDir) * mag);
			ammo.gameObject.GetComponent<Rigidbody2D>().AddTorque(tourque);
			if(ammo.tag == "ammo")
			{
				ammo.gameObject.GetComponent<Ammo>().state = Ammo.State.Idle;
				//GameManager.IgnoreCollisionWithAllPlayers(ammo.GetComponent<BoxCollider2D>(), true);
			}
			count++;

			if (AmmoPrefab.tag == "emptyPlayer") {
				int i = Random.Range(1,6);
				print(i);
				switch(i)
				{
				case 1:
					AmmoPrefab.GetComponent<SpriteRenderer>().sprite = Resources.Load("redOx", typeof(Sprite)) as Sprite;
					break;
				case 2:
					AmmoPrefab.GetComponent<SpriteRenderer>().sprite = Resources.Load("greenOx", typeof(Sprite)) as Sprite;
					break;
				case 3:
					AmmoPrefab.GetComponent<SpriteRenderer>().sprite = Resources.Load("blueOx", typeof(Sprite)) as Sprite;
					break;
				case 4:
					AmmoPrefab.GetComponent<SpriteRenderer>().sprite = Resources.Load("yellowOx", typeof(Sprite)) as Sprite;
					break;
				case 5:
					AmmoPrefab.GetComponent<SpriteRenderer>().sprite = Resources.Load("greenOx", typeof(Sprite)) as Sprite;
					break;
				default:
					AmmoPrefab.GetComponent<SpriteRenderer>().sprite = Resources.Load("redOx", typeof(Sprite)) as Sprite;
					break;
				}
				if(Random.Range(0,2) == 1)
				{
					RollAfterTime RAT = AmmoPrefab.GetComponent<RollAfterTime>();
					RAT.enabled = true;
					RAT.RollTime = Random.Range(75,200	);
					RAT.Direction = Random.Range(0,2) * 2 - 1;
				}
			}
		}
	}
}
