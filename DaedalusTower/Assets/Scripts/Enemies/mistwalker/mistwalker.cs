using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class mistwalker : MonoBehaviour {

    public bool clawsActive = false;
    public int health = 1000;

    public Animator anim;

    private GameObject player;
    public float minDistance;

    private float fogDensity;
    public float maxFogDensity;
    public float fogGrowthRate;

    private float normSpeed;
    private float normAccel;
    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerMovement>().playerCam.GetComponent<CameraFollow>().bossFight = true;
        RenderSettings.fog = true;
        fogDensity = 0;
        normSpeed = GetComponent<NavMeshAgent>().speed;
        normAccel = GetComponent<NavMeshAgent>().acceleration;
	}
	
	// Update is called once per frame
	void Update () {
        float playerDistance = Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.z), new Vector2(transform.position.x, transform.position.z));
        transform.GetComponent<NavMeshAgent>().destination = player.transform.position;
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 2")) {
            clawsActive = true;
        } else {
            clawsActive = false;
        }
        if (GetComponent<NavMeshAgent>().speed == 0 && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 2") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 1")) {
            //anim.Play("Take 001", 0, 0f);
            anim.SetBool("moving", false);
        }
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001")) {
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
        }

        if(playerDistance <= minDistance) {
            GetComponent<NavMeshAgent>().speed = 0;
            GetComponent<NavMeshAgent>().acceleration = 1000;
        }else {
            GetComponent<NavMeshAgent>().speed = normSpeed;
            GetComponent<NavMeshAgent>().acceleration = normAccel;
            move();
        }


        if(fogDensity < maxFogDensity) {
            fogDensity += maxFogDensity * Time.deltaTime * fogGrowthRate/100;
        }else if(fogDensity > maxFogDensity) {
            fogDensity = maxFogDensity;
        }
        RenderSettings.fogDensity = fogDensity;
    }

    void move() {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 0")) {
            //anim.Play("Take 001 0", 0, 0f);
            anim.SetBool("moving", true);
        }
    }

    void attack() {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 2")) {
            //anim.Play("Take 001 2", 0, 0f);
            anim.SetTrigger("attack");
        }
        print("Mistwalker Attacking");
    }

    public void takeDamage(int damage) {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 2")) {
            anim.Play("Take 001 1", 0, 0f);
            print(!anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 2"));
        }
        health = health - damage;
        print("Mistwalker Took Damage");
    }

}
