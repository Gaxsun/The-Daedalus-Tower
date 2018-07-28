using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public string weaponName;
    public int baseDamage;
    public float speed;
    public float knockback;
    public float knockbackModdable;
    public bool attackActive = false;
    public GameObject effectObject;
    public AudioSource weaponHit;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (attackActive) {
            effectObject.GetComponent<TrailRenderer>().enabled = true;
            print("weapon active");
        } else {
            effectObject.GetComponent<TrailRenderer>().enabled = false;
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

    public void playHitEffects() {
        weaponHit.Play();

        //instantiate spark and flash

    }

}
