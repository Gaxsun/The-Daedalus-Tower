using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resspawnTracker : MonoBehaviour {

    public bool hasDiedBefore = false;
    public bool lvRestarted = false;
    public bool hasReachedBossRoom = false;
    public Vector3 respawnLocation;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this.gameObject);
    }
	
	// Update is called once per frame
	void Update () {
        DontDestroyOnLoad(this.gameObject);
        if (lvRestarted && hasDiedBefore && respawnLocation != null && hasReachedBossRoom) {
            print("(.)(.)");
            GameObject.FindWithTag("Player").transform.position = respawnLocation;
            lvRestarted = false;
        }
    }
}
