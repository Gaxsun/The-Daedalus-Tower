using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class claw : MonoBehaviour {

    public GameObject mistwalker;

    public int clawDamage = 10;
    public float clawKnockback;
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<BoxCollider>().enabled = mistwalker.GetComponent<mistwalker>().clawsActive;
	}

    void OnTriggerStay(Collider other) {
        if (other.tag == "Player" && mistwalker.GetComponent<mistwalker>().clawsActive) {
            other.GetComponent<playerManager>().takeDamage(clawDamage);
            other.GetComponent<Rigidbody>().AddForce((other.transform.position - transform.position).normalized * clawKnockback);
            mistwalker.GetComponent<mistwalker>().clawsActive = false;
        }
    }

}
