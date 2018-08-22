﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour {
    public bool triggered;
    public bool bossFight;
	// Use this for initialization
	void Start () {
        triggered = false;
        bossFight = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (bossFight) {
            triggered = true;
        }
	}

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            triggered = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            triggered = false;
        }
    }
}
