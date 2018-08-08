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
    public GameObject effectObject2;
    public AudioSource weaponHit;
    public GameObject hitEffectObject;
    Vector3 currentCollisionPoint;

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

    void OnCollisionEnter(Collision other) {
        currentCollisionPoint = other.contacts[0].point;
    }

    public void playHitEffects() {
        weaponHit.Play();
        Instantiate(hitEffectObject, currentCollisionPoint, Quaternion.identity);
        //instantiate spark and flash
    }

}
