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
    public bool heavyAttack;
    public GameObject effectObject;
    public GameObject effectObject2;
    public GameObject hitEffectObject;
    Vector3 currentCollisionPoint;

    public AudioClip[] weaponSound;
    public AudioSource weaponSoundSource;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {



        if (attackActive) {
            if (heavyAttack) {
                effectObject2.GetComponent<TrailRenderer>().enabled = true;
                effectObject.GetComponent<TrailRenderer>().enabled = false;
            } else {
                effectObject.GetComponent<TrailRenderer>().enabled = true;
                effectObject2.GetComponent<TrailRenderer>().enabled = false;
            }
        } else {
            effectObject.GetComponent<TrailRenderer>().enabled = false;
            effectObject2.GetComponent<TrailRenderer>().enabled = false;
        }
	}

    void OnTriggerStay(Collider other) {
        if (other.tag == "enemy" && attackActive) {
            other.GetComponent<Enemy>().takeDamage(this.gameObject,baseDamage, knockbackModdable);

            if (weaponSoundSource.isPlaying == false)
            {
                weaponSoundSource.Stop();
                weaponSoundSource.clip = weaponSound[Mathf.RoundToInt(Random.Range(0, 2))];
                weaponSoundSource.loop = false;
                weaponSoundSource.Play();
            }

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
        Instantiate(hitEffectObject, currentCollisionPoint, Quaternion.identity);
        //instantiate spark and flash
    }

}
