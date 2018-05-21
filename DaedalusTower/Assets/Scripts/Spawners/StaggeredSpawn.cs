using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaggeredSpawn : MonoBehaviour {
    public GameObject[] preSpawners;
    private bool triggered;

	// Use this for initialization
	void Start () {
        triggered = false;	
	}
	
	// Update is called once per frame
	void Update () {
        triggered = true;
		foreach(GameObject spawner in preSpawners) {
            if (spawner.GetComponent<BasicSpawner>().corpsePile == false) {
                triggered = false;
            }
            
        }
	}
}
