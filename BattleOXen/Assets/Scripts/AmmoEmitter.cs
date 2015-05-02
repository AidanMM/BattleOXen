using UnityEngine;
using System.Collections;

public class AmmoEmitter : MonoBehaviour {


	public GameObject AmmoPrefab;
	public Vector2 dir;
	public Vector2 varying;
    private Animator anim;
	public int mag;
	public int frequency;
	public int max = 10;
	public int tourque;
	//-1 for count will make it spawn infinetly
	private int count = 0;
	private int timer = 0;
    private int frameCount = 0;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        anim.speed = 0;
        
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		timer++;
		if ((count < max || max == -1) && timer % frequency == 0) {
            anim.speed = 2;
            frameCount = timer;
			GameObject ammo = (GameObject)Instantiate(AmmoPrefab);
			ammo.transform.position = gameObject.transform.position;
			Vector2 randomDir = new Vector2(Random.Range(0, varying.x), Random.Range(0,varying.y));
			ammo.gameObject.GetComponent<Rigidbody2D>().AddForce((dir + randomDir) * mag);
			ammo.gameObject.GetComponent<Rigidbody2D>().AddTorque(tourque);
			if(ammo.tag == "ammo")
			{
                ammo.gameObject.GetComponent<SpriteRenderer>().sortingOrder = -5;
				ammo.gameObject.GetComponent<Ammo>().state = Ammo.State.Idle;
                if (Random.Range(0, 5) == 4)
                {
                    ammo.gameObject.GetComponent<Ammo>().type = Ammo.Type.Bomb;
                    ammo.gameObject.GetComponent<SpriteRenderer>().sprite = ammo.gameObject.GetComponent<Ammo>().bombSprite;
                }
				//GameManager.IgnoreCollisionWithAllPlayers(ammo.GetComponent<BoxCollider2D>(), true);
			}
			count++;

			if (AmmoPrefab.tag == "emptyPlayer") {
				int i = Random.Range(0,7);
				AmmoPrefab.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Oxen/" + i);

				if(Random.Range(0,2) == 1)
				{
					RollAfterTime RAT = AmmoPrefab.GetComponent<RollAfterTime>();
					RAT.enabled = true;
					RAT.RollTime = Random.Range(75,200	);
					RAT.Direction = Random.Range(0,2) * 2 - 1;
				}
			}
		}
        if (timer > frameCount + 8 )
        {
            anim.speed = 0;
        }
	}
}
