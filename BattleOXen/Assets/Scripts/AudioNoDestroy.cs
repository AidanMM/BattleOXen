using UnityEngine;
using System.Collections;

public class AudioNoDestroy : MonoBehaviour {

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
        if (Application.loadedLevelName == "ModeSelect")
        {
            this.GetComponent<AudioSource>().Stop();
            Destroy(this);
            print("Happened");
        }
        print("This is running");
	}
}
