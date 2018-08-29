using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class hellhound : MonoBehaviour {

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

    public GameObject hitEffectObject;
    private Vector3 currentCollisionPoint;
    private bool attackFix = true;
    // Use this for initialization
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        attackTimer = 0;
    }

    // Update is called once per frame
    void Update() {

        if (anim.GetInteger("currentAnimationState") != 8 && anim.GetInteger("currentAnimationState") != 9) {
            GetComponent<NavMeshAgent>().destination = player.transform.position;
        } else {
            GetComponent<NavMeshAgent>().destination = transform.position;
        }

        if (Vector3.Distance(player.transform.position, transform.position) <= minDistance && anim.GetInteger("currentAnimationState") != 8 && anim.GetInteger("currentAnimationState") != 9) {
            GetComponent<NavMeshAgent>().destination = transform.position;
            if (Vector3.Distance(player.transform.position, this.gameObject.transform.position) <= minDistance && !anim.GetCurrentAnimatorStateInfo(0).IsName("idle") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack(1)") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack(2)") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack(3)")) {
                if (anim.GetInteger("currentAnimationState") != 8 && anim.GetInteger("currentAnimationState") != 9) {
                    anim.SetInteger("currentAnimationState", 0);
                }
                if (Time.time > attackTimer + attackDelay) {
                    boxCollider.GetComponent<BoxCollider>().enabled = true;
                    attackFix = true;
                    attack();
                }
            }
        } else if (Vector3.Distance(player.transform.position, this.gameObject.transform.position) > minDistance && !anim.GetCurrentAnimatorStateInfo(0).IsName("walk")  && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack(1)") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack(2)") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack(3)") && anim.GetInteger("currentAnimationState") != 8 && anim.GetInteger("currentAnimationState") != 9) {
                if (anim.GetInteger("currentAnimationState") != 8 && anim.GetInteger("currentAnimationState") != 9) {
                    anim.SetInteger("currentAnimationState", 1);
            }
        }

        if (previousAnimationState == 0) {
            if ((anim.GetInteger("currentAnimationState") == 2 || anim.GetInteger("currentAnimationState") == 3 || anim.GetInteger("currentAnimationState") == 4) && anim.GetInteger("currentAnimationState") != 8 && anim.GetInteger("currentAnimationState") != 9) {
                anim.SetInteger("currentAnimationState", 0);
            }
        }

        if (anim.GetInteger("currentAnimationState") != 0 && anim.GetInteger("currentAnimationState") != 1 && anim.GetInteger("currentAnimationState") != 2 && anim.GetInteger("currentAnimationState") != 3 && anim.GetInteger("currentAnimationState") != 4 && anim.GetInteger("currentAnimationState") != 5 && anim.GetInteger("currentAnimationState") != 6 && anim.GetInteger("currentAnimationState") != 7 && anim.GetInteger("currentAnimationState") != 8 && anim.GetInteger("currentAnimationState") != 9) {
            Destroy(this);
        }

        if ((previousAnimationState != 8 || previousAnimationState != 9) && (anim.GetInteger("currentAnimationState") == 8 || anim.GetInteger("currentAnimationState") == 9)) {
            deathTimerStartTime = Time.time;
        }

        if (Time.time >= deathTimerStartTime + 5 && (anim.GetInteger("currentAnimationState") == 8 || anim.GetInteger("currentAnimationState") == 9)) {
            // put sick fire here
            fireEffect.GetComponent<ParticleSystem>().Play();
        }

        if (Time.time >= deathTimerStartTime + 7 && (anim.GetInteger("currentAnimationState") == 8 || anim.GetInteger("currentAnimationState") == 9)) {
            Destroy(this);
        }

        previousAnimationState = anim.GetInteger("currentAnimationState");
    }

    public void playDamaged() {
        if ((anim.GetInteger("currentAnimationState") != 8 || anim.GetInteger("currentAnimationState") != 9)) {
            anim.Play("Walk", 0);
        }
    }

    void attack() {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack(1)") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack(2)") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack(3)") && (anim.GetInteger("currentAnimationState") != 8 || anim.GetInteger("currentAnimationState") != 9)) {
            Random.InitState(Mathf.RoundToInt(Time.time) * Mathf.RoundToInt(transform.position.x * transform.position.y * transform.position.z));
            int randInt = Mathf.RoundToInt(Random.Range(0, 15));
            if (randInt <= 5) {
                anim.Play("Attack(1)", 0);
            } else if(randInt <= 10) {
                anim.Play("Attack(2)", 0);
            } else {
                anim.Play("Attack(3)", 0);
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if ((anim.GetCurrentAnimatorStateInfo(0).IsName("Attack(1)") || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack(2)") || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack(3)")) && (other.tag == "Player" || (other.tag == "Weapon" && !player.GetComponent<PlayerMovement>().isAttacking()))) {
            if (attackFix) {
                player.GetComponent<playerManager>().takeDamage(damage);
                playHitEffects();
                attackTimer = Time.time;
                boxCollider.GetComponent<BoxCollider>().enabled = false;
                print(boxCollider.GetComponent<BoxCollider>().enabled);
                attackFix = false;
            }
        }
    }

    public void playDeath() {
        if (anim.GetInteger("currentAnimationState") == 1 || anim.GetInteger("currentAnimationState") == 4) {
            anim.Play("Death on the move", 0);
        } else {
            anim.Play("Dead", 0);
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
