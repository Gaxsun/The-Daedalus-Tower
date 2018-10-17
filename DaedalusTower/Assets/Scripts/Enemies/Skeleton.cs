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

    public GameObject hitEffectObject;
    private Vector3 currentCollisionPoint;

    public AudioClip[] attackSounds;
    public AudioClip[] deathSounds;
    public AudioClip[] ambientSounds;
    public AudioClip[] spawnSounds;
    public AudioClip[] damageSounds;
    public AudioSource skeletonSounds;
    private bool dead = false;

    // Use this for initialization
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        attackTimer = 0;

        Random.InitState(Mathf.RoundToInt(Time.time) * Mathf.RoundToInt(transform.position.x * transform.position.y * transform.position.z));
        int randNum = Mathf.RoundToInt(Random.Range(0, 60));
        if (randNum >= 50)
        {
            skeletonSounds.clip = ambientSounds[0];
        }
        else if (randNum >= 40)
        {
            skeletonSounds.clip = ambientSounds[1];
        }
        else if (randNum >= 30)
        {
            skeletonSounds.clip = ambientSounds[2];
        }
        else if (randNum >= 20)
        {
            skeletonSounds.clip = ambientSounds[3];
        }
        else if (randNum >= 10)
        {
            skeletonSounds.clip = ambientSounds[4];
        }
        else
        {
            skeletonSounds.clip = deathSounds[0];
        }
        skeletonSounds.loop = false;
        skeletonSounds.Play();
    }

    // Update is called once per frame
    void Update() {
        stopIfAttacking();
        if (anim.GetInteger("currentAnimationState") != 5) {
            GetComponent<NavMeshAgent>().destination = player.transform.position;
        } else {
            GetComponent<NavMeshAgent>().destination = transform.position;
        }

        if (Vector3.Distance(player.transform.position, transform.position) <= minDistance && anim.GetInteger("currentAnimationState") != 5) {
            GetComponent<NavMeshAgent>().destination = transform.position;
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("attack") && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack2")) {
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("idle") && anim.GetInteger("currentAnimationState") != 5) {
                    anim.SetInteger("currentAnimationState", 0);
                }
                if (Time.time > attackTimer + attackDelay) {
                    boxCollider.GetComponent<BoxCollider>().enabled = true;
                    attack();
                }
            }
        } else if (Vector3.Distance(player.transform.position, this.gameObject.transform.position) > minDistance && !anim.GetCurrentAnimatorStateInfo(0).IsName("walk") && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack") && !anim.GetCurrentAnimatorStateInfo(0).IsName("walk2") && anim.GetInteger("currentAnimationState") != 5) {
            if (Mathf.RoundToInt(Random.Range(0, 100)) >= 98) {
                if (anim.GetInteger("currentAnimationState") != 5) {
                    anim.SetInteger("currentAnimationState", 4);
                }
            } else {
                if (anim.GetInteger("currentAnimationState") != 5) {
                    anim.SetInteger("currentAnimationState", 1);
                }
            }  
        }
        
        if ((anim.GetInteger("currentAnimationState") == 3 || anim.GetInteger("currentAnimationState") == 2) && anim.GetInteger("currentAnimationState") != 5) {
            anim.SetInteger("currentAnimationState", 0);
            attackTimer = Time.time;
        }

        if (anim.GetInteger("currentAnimationState") != 0 && anim.GetInteger("currentAnimationState") != 1 && anim.GetInteger("currentAnimationState") != 2 && anim.GetInteger("currentAnimationState") != 3 && anim.GetInteger("currentAnimationState") != 4 && anim.GetInteger("currentAnimationState") != 5) {
            Destroy(this);
        }
        
        if (previousAnimationState != 5 && anim.GetInteger("currentAnimationState") == 5) {
            deathTimerStartTime = Time.time;
            GetComponent<CapsuleCollider>().enabled = false;
        }

        if (Time.time >= deathTimerStartTime + 5 && anim.GetInteger("currentAnimationState") == 5) {
            // put sick fire here
            fireEffect.GetComponent<ParticleSystem>().Play();
        }

        if (Time.time >= deathTimerStartTime + 7 && anim.GetInteger("currentAnimationState") == 5) {
            Destroy(this);
        }

        previousAnimationState = anim.GetInteger("currentAnimationState");

        if(skeletonSounds.isPlaying == false)
        {
            Random.InitState(Mathf.RoundToInt(Time.time) * Mathf.RoundToInt(transform.position.x * transform.position.y * transform.position.z));
            skeletonSounds.clip = ambientSounds[Mathf.RoundToInt(Random.Range(0, 6))];
            skeletonSounds.loop = false;
            skeletonSounds.Play();

        }           

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
                anim.SetInteger("currentAnimationState", 3);
            } else {
                anim.Play("attack2", 0);
                anim.SetInteger("currentAnimationState", 2);
            }

            int randNum = Mathf.RoundToInt(Random.Range(0, 40));
            if (randNum >= 30)
            {
                skeletonSounds.clip = attackSounds[0];
            }
            else if(randNum >= 20)
            {
                skeletonSounds.clip = attackSounds[1];
            }
            else if (randNum >= 10)
            {
                skeletonSounds.clip = attackSounds[2];
            }
            else
            {
                skeletonSounds.clip = attackSounds[3];
            }

            skeletonSounds.loop = false;
            skeletonSounds.Play();
        }
    }

    private void OnTriggerStay(Collider other) {
        if ((anim.GetCurrentAnimatorStateInfo(0).IsName("attack") || anim.GetCurrentAnimatorStateInfo(0).IsName("attack2")) && other.tag == "Player") {
            player.GetComponent<playerManager>().takeDamage(damage);
            playHitEffects();
            boxCollider.GetComponent<BoxCollider>().enabled = false;
        }
    }

    public void playDeath() {
        anim.SetInteger("currentAnimationState", 5);
        if (!dead)
        {
            Random.InitState(Mathf.RoundToInt(Time.time) * Mathf.RoundToInt(transform.position.x * transform.position.y * transform.position.z));
            int randNum = Mathf.RoundToInt(Random.Range(0, 30));
            if (randNum >= 20)
            {
                skeletonSounds.clip = deathSounds[0];
            }
            else if (randNum >= 10)
            {
                skeletonSounds.clip = deathSounds[1];
            }
            else
            {
                skeletonSounds.clip = deathSounds[2];
            }
            skeletonSounds.loop = false;
            skeletonSounds.Play();
            dead = true;
        }
    }

    void OnCollisionEnter(Collision other) {
        currentCollisionPoint = other.contacts[0].point;
    }

    public void playHitEffects() {
        Instantiate(hitEffectObject, currentCollisionPoint, Quaternion.identity);
        //instantiate spark and flash
    }

    public void stopIfAttacking() {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("attack") || anim.GetCurrentAnimatorStateInfo(0).IsName("attack2") || anim.GetInteger("currentAnimationState") == 5) {
            GetComponent<NavMeshAgent>().speed = 0;
            transform.LookAt(player.transform);
        } else {
            GetComponent<NavMeshAgent>().speed = 3.5f;
        }
    }

}
