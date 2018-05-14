using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public string weaponName;
    public float baseDamage;
    public bool attackActive = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (attackActive) {
            print("Q");
        }
	}

    void OnTriggerStay(Collider other) {
        if (other.tag == "enemy" && attackActive|| other.tag == "destTerrain" && attackActive) {
            Destroy(other.gameObject);
        }
    }
}
