using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hellhoundBite : MonoBehaviour {
    private Animator anim;
    private Vector3 startPos;

    // Use this for initialization
    void Start () {
        startPos = transform.localPosition;
    }
	
	// Update is called once per frame
	void Update () {
        if (GetComponentInParent<hellhound>()) {
            if (!GetComponentInParent<hellhound>().stage2) {
                transform.localPosition = new Vector3(startPos.x, startPos.y, startPos.z);
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if (GetComponentInParent<hellhound>()) {
            anim = GetComponentInParent<hellhound>().anim;
            if (other.tag == "Player") {
                if (GetComponentInParent<hellhound>().attackFix) {
                    GetComponentInParent<hellhound>().player.GetComponent<playerManager>().takeDamage(GetComponentInParent<hellhound>().damage);
                    GetComponentInParent<hellhound>().playHitEffects();
                    GetComponentInParent<hellhound>().attackTimer = Time.time;
                    GetComponent<BoxCollider>().enabled = false;
                    GetComponentInParent<hellhound>().attackFix = false;
                }
            }
        }
    }
}
