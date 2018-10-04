using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resspawnTracker : MonoBehaviour {

    public bool hasDiedBefore;
    public bool lvRestarted;
    public bool hasReachedBossRoom;
    public Vector3 respawnLocation;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this.gameObject);
        print("noot noot");
    }
	
	// Update is called once per frame
	void Update () {
        DontDestroyOnLoad(this.gameObject);
        print(hasDiedBefore);
        if (lvRestarted && hasDiedBefore && respawnLocation != null) {
            GameObject.FindWithTag("Player").transform.position = respawnLocation;
            lvRestarted = false;
        }
    }
}
