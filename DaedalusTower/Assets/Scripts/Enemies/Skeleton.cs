using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Skeleton : MonoBehaviour {

    private GameObject player;

    public Animator anim;

    public GameObject boxCollider;
    public GameObject fireEffect;

    public float minDistance;

    public int damage;
    public float attackDelay;
    private float attackTimer;
    private int previousAnimationState = 0;

    public float deathTimerStartTime;

    // Use this for initialization
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        attackTimer = 0;
    }

    // Update is called once per frame
    void Update() {

        if (anim.GetInteger("currentAnimationState") != 5) {
            GetComponent<NavMeshAgent>().destination = player.transform.position;
        } else {
            GetComponent<NavMeshAgent>().destination = transform.position;
        }

        if (Vector3.Distance(player.transform.position, transform.position) <= minDistance && anim.GetInteger("currentAnimationState") != 5) {
            GetComponent<NavMeshAgent>().destination = transform.position;
            if (Vector3.Distance(player.transform.position, this.gameObject.transform.position) <= minDistance && !anim.GetCurrentAnimatorStateInfo(0).IsName("idle") && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack") && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack2")) {
                if (anim.GetInteger("currentAnimationState") != 5) {
                    anim.SetInteger("currentAnimationState", 0);
                }
                if (Time.time > attackTimer + attackDelay) {
                    boxCollider.GetComponent<BoxCollider>().enabled = true;
                    attack();
                }
            }
        } else if (Vector3.Distance(player.transform.position, this.gameObject.transform.position) > minDistance && !anim.GetCurrentAnimatorStateInfo(0).IsName("walk") && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack") && !anim.GetCurrentAnimatorStateInfo(0).IsName("walk2") && anim.GetInteger("currentAnimationState") != 5) {
            if (Mathf.RoundToInt(Random.Range(0, 100)) >= 90) {
                if (anim.GetInteger("currentAnimationState") != 5) {
                    anim.SetInteger("currentAnimationState", 4);
                }
            } else {
                if (anim.GetInteger("currentAnimationState") != 5) {
                    anim.SetInteger("currentAnimationState", 1);
                }
            }  
        }

        if (previousAnimationState == 0) {
            if ((anim.GetInteger("currentAnimationState") == 3 || anim.GetInteger("currentAnimationState") == 2) && anim.GetInteger("currentAnimationState") != 5) {
                anim.SetInteger("currentAnimationState",0);
            }
        }

        if (anim.GetInteger("currentAnimationState") != 0 && anim.GetInteger("currentAnimationState") != 1 && anim.GetInteger("currentAnimationState") != 2 && anim.GetInteger("currentAnimationState") != 3 && anim.GetInteger("currentAnimationState") != 4 && anim.GetInteger("currentAnimationState") != 5) {
            Destroy(this);
        }
        
        if (previousAnimationState != 5 && anim.GetInteger("currentAnimationState") == 5) {
            deathTimerStartTime = Time.time;
        }

        if (Time.time >= deathTimerStartTime + 5 && anim.GetInteger("currentAnimationState") == 5) {
            // put sick fire here
            fireEffect.GetComponent<ParticleSystem>().Play();
        }

        if (Time.time >= deathTimerStartTime + 7 && anim.GetInteger("currentAnimationState") == 5) {
            Destroy(this);
        }

        previousAnimationState = anim.GetInteger("currentAnimationState");
    }

    public void playDamaged() {
        if (anim.GetInteger("currentAnimationState") != 5) {
            anim.Play("walk", 0);
        }
    }

    void attack() {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("attack") && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack2") && anim.GetInteger("currentAnimationState") != 5) {
            Random.InitState(Mathf.RoundToInt(Time.time)  * Mathf.RoundToInt(transform.position.x * transform.position.y * transform.position.z));
            if (Mathf.RoundToInt(Random.Range(0,10)) >= 5) {
                anim.Play("attack", 0);
            } else {
                anim.Play("attack2", 0);
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if ((anim.GetCurrentAnimatorStateInfo(0).IsName("attack") || anim.GetCurrentAnimatorStateInfo(0).IsName("attack2")) && other.tag == "Player") {
            player.GetComponent<playerManager>().takeDamage(damage);
            boxCollider.GetComponent<BoxCollider>().enabled = false;
        }
    }

    public void playDeath() {
        anim.SetInteger("currentAnimationState", 5);
    }

}
