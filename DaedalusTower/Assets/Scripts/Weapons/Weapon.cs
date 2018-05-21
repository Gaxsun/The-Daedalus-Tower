using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public string weaponName;
    public float baseDamage;
    public float speed;
    public float knockback;
    public bool attackActive = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (attackActive) {
            print("weapon active");
        }
	}

    void OnTriggerStay(Collider other) {
        if (other.tag == "enemy" && attackActive|| other.tag == "destTerrain" && attackActive) {
            Destroy(other.gameObject);
        }
    }
}
