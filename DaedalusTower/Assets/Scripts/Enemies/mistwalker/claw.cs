using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class claw : MonoBehaviour {

    public GameObject mistwalker;

    public int clawDamage = 10;
    public float clawKnockback;

    public GameObject hitEffectObject;
    private Vector3 currentCollisionPoint;

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
            currentCollisionPoint = other.GetComponent<PlayerMovement>().playerCam.GetComponent<CameraFollow>().playerLocation;
            playHitEffects();
            foreach (Renderer rend in other.GetComponentsInChildren<Renderer>()) {
                rend.material.color = new Color(other.GetComponent<playerManager>().health / other.GetComponent<playerManager>().healthBar.maxValue, other.GetComponent<playerManager>().health / other.GetComponent<playerManager>().healthBar.maxValue, other.GetComponent<playerManager>().health / other.GetComponent<playerManager>().healthBar.maxValue);
            }
            mistwalker.GetComponent<mistwalker>().clawsActive = false;
        }
    }


    public void playHitEffects() {
        Instantiate(hitEffectObject, currentCollisionPoint, Quaternion.identity);
        //instantiate spark and flash
    }

}
