using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavigation : MonoBehaviour {

    private GameObject player;

    public Animator anim;

    public float minDistance;

    public int damage;
    public float attackDelay;
    private float attackTimer;
    // Use this for initialization
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        attackTimer = 0;
    }

    // Update is called once per frame
    void Update() {
        GetComponent<NavMeshAgent>().destination = player.transform.position;
        if (Vector3.Distance(player.transform.position, transform.position) <= minDistance) {
            GetComponent<NavMeshAgent>().destination = transform.position;
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 0")) {
                anim.Play("Take 001", 0, 0f);
                if (Time.time > attackTimer + attackDelay) {
                    GetComponent<BoxCollider>().enabled = true;
                    attack();
                }
            }
        } else if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 0")) {
            anim.Play("Take 001 1", 0, 0f);
        }
    }

void attack() {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 0")) {
            anim.Play("Take 001 0", 0, 0f);
        }
    }

    private void OnTriggerStay(Collider other) {
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 0") && other.tag == "Player") {
            player.GetComponent<playerManager>().takeDamage(damage);
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}
