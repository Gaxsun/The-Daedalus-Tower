using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class claw : MonoBehaviour {

    public GameObject mistwalker;

    public int clawDamage = 10;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay(Collider other) {
        if (other.tag == "Player" && mistwalker.GetComponent<mistwalker>().clawsActive) {
            other.GetComponent<playerManager>().takeDamage(clawDamage);
        }
    }

}
