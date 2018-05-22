using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mistwalker : MonoBehaviour {

    public bool clawsActive = false;
    public int health = 1000;

    public Animator anim;

    public GameObject player;

    // Use this for initialization
    void Start () {
        player.GetComponent<PlayerMovement>().playerCam.GetComponent<CameraFollow>().bossFight = true;
        RenderSettings.fog = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 2")) {
            clawsActive = true;
        } else {
            clawsActive = false;
        }
        if (/* not moving &&*/ !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 2")) {
            anim.Play("Take 001", 0, 0f);
        }
    }

    void move() {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 0")) {
            anim.Play("Take 001 0", 0, 0f);
        }
    }

    void attack() {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 2")) {
            anim.Play("Take 001 2", 0, 0f);
        }
        print("Mistwalker Attacking");
    }

    public void takeDamage(int damage) {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 2")) {
            anim.Play("Take 001 1", 0, 0f);
        }
        health = health - damage;
        print("Mistwalker Took Damage");
    }

}
