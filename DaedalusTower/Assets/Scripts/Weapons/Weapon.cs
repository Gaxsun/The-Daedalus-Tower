using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public string weaponName;
    public int baseDamage;
    public float speed;
    public int knockback;
    public int knockbackModdable;
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
        if (other.tag == "enemy" && attackActive) {
            other.GetComponent<Enemy>().takeDamage(this.gameObject,baseDamage, knockbackModdable);
        }
        if (other.tag == "destTerrain" && attackActive) {
            Destroy(other.gameObject);
        }
        if (other.tag == "mistwalker" && attackActive) {
            other.GetComponent<mistwalker>().takeDamage(baseDamage);
        }
    }
}
