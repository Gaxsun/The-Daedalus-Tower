using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hellhoundBite : MonoBehaviour {
    private Animator anim;
    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
    }

    private void OnTriggerStay(Collider other) {
        anim = GetComponentInParent<hellhound>().anim;
        print(other.tag + " " + anim.GetCurrentAnimatorStateInfo(0));
        if ((other.tag == "Player" || (other.tag == "Weapon" && !GetComponentInParent<hellhound>().player.GetComponent<PlayerMovement>().isAttacking()))) {
            //(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack(1)") || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack(2)") || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack(3)")) && 
            print("Net");
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
