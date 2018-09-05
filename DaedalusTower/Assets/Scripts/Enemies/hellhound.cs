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
    private float currentMinDistance;

    public AudioClip[] attackSounds;
    public AudioClip[] deathSounds;
    public AudioClip[] ambientSounds;
    public AudioClip[] damageSounds;
    public AudioSource hellhoundSounds;

    public int damage;
    public float attackDelay;
    private float attackTimer;
    private int previousAnimationState = 0;
    private bool alive = true;

    private float deathTimerStartTime;
    public float deathTimerDelay;

    public GameObject hitEffectObject;
    private Vector3 currentCollisionPoint;
    private bool attackFix = true;

    public float stage2Health;
    public float stage3Health;
    private float health;
    private float maxHealth;
    private bool stage2;
    private bool stage3;
    private float maxSpeed;

    private bool leapAdjust = false;
    private bool leapt = false;
    private Vector3 leapDestination;

    public GameObject leapTarget;
    private float slerpTime;
    public float slerpDelay;
    private float slerper;
    private bool boom = true;
    // Use this for initialization
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        attackTimer = 0;
        health = GetComponent<Enemy>().health;
        maxSpeed = GetComponent<NavMeshAgent>().speed;
        maxHealth = health;
        leapDestination = leapTarget.transform.position;
        slerpTime = Time.time;
    }

    // Update is called once per frame
    void Update() {
        health = GetComponent<Enemy>().health;
        if (!alive) {
            GetComponent<NavMeshAgent>().enabled = false;
            playDeath();
        } else {
            GetComponent<NavMeshAgent>().destination = player.transform.position;
        }

        if (Vector3.Distance(player.transform.position, transform.position) > minDistance && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack(3)") && alive) {
            if(Time.time > slerpDelay + slerpTime) {
                GetComponent<NavMeshAgent>().speed = maxSpeed / 5.0f;
                slerper = 5.0f;
            }else {
                GetComponent<NavMeshAgent>().speed = maxSpeed;
                slerper = 0.5f;
            }
            GetComponent<NavMeshAgent>().velocity = Vector3.Slerp(GetComponent<NavMeshAgent>().velocity.normalized, GetComponent<NavMeshAgent>().desiredVelocity.normalized, slerper * Time.deltaTime) * maxSpeed;
        }

        if (Vector3.Distance(player.transform.position, transform.position) <= minDistance && alive) {
            GetComponent<NavMeshAgent>().destination = transform.position;
            if (Vector3.Angle(transform.forward, player.transform.position - transform.position) > -35.0f && Vector3.Angle(transform.forward, player.transform.position - transform.position) < 35.0f) {
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("idle") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack(1)") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack(2)") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack(3)")) {
                    if (alive) {
                        anim.SetInteger("currentAnimationState", 0);
                    }
                    if (Time.time > attackTimer + attackDelay) {
                        boxCollider.GetComponent<BoxCollider>().enabled = true;
                        attackFix = true;
                        attack();
                    }
                }
            }else {
                transform.LookAt(Vector3.Slerp(transform.forward, player.transform.position - transform.position, 3 * Time.deltaTime) + transform.position);
                Debug.DrawRay(transform.position, player.transform.position - transform.position);
                print("Tuuuuuurn");
            }
        } else if (Vector3.Distance(player.transform.position, transform.position) > minDistance && !anim.GetCurrentAnimatorStateInfo(0).IsName("Walk")  && !anim.GetCurrentAnimatorStateInfo(0).IsName("Run") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack(1)") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack(2)") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack(3)")) {
            if (alive) {
                anim.SetInteger("currentAnimationState", 1);
            }
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack(3)")) {
            leapt = true;
            leapDestination = leapTarget.transform.position;
            if (alive) {
                anim.SetInteger("currentAnimationState", 0);
            }
        } else if(leapt){
            leapt = false;
            leapAdjust = true;
        }
        if (leapAdjust) {
            transform.position = leapDestination;
            leapAdjust = false;
        }

        if (alive) {
            deathTimerStartTime = Time.time;
        }

        if (Time.time >= deathTimerStartTime + deathTimerDelay/2.0f && !alive && boom) {
            // put sick fire here
            fireEffect.GetComponent<ParticleSystem>().Play();
            Random.InitState(Mathf.RoundToInt(Time.time) * Mathf.RoundToInt(transform.position.x * transform.position.y * transform.position.z));
            hellhoundSounds.clip = deathSounds[Mathf.RoundToInt(Random.Range(0, 2))];
            hellhoundSounds.loop = false;
            hellhoundSounds.Play();
            boom = false;
        }
        if (Time.time >= deathTimerStartTime + deathTimerDelay && !alive) {
            Destroy(this);
        }

        previousAnimationState = anim.GetInteger("currentAnimationState");
        
        if(hellhoundSounds.isPlaying == false && alive)
        {
            hellhoundSounds.clip = ambientSounds[0];
            hellhoundSounds.loop = false;
            hellhoundSounds.Play();
        }

        if(hellhoundSounds.isPlaying == false)
        {
            hellhoundSounds.clip = null;
        }
        anim.SetFloat("moveSpeed", GetComponent<NavMeshAgent>().velocity.magnitude);
        fightStages();

    }

    public void playDamaged() {
        if (alive) {
            anim.Play("Get hit", 0);
        }
    }

    void attack() {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack(1)") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack(2)") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack(3)") && alive) {
            Random.InitState(Mathf.RoundToInt(Time.time) * Mathf.RoundToInt(transform.position.x * transform.position.y * transform.position.z));
            if (stage3) {
                //Attack(2), Bite
                anim.Play("Attack(2)", 0);
            } else if(stage2) {
                //Attack(1), Big Bite
                anim.Play("Attack(1)", 0);
            } else {
                //Attack(3), Leaping strike
                anim.SetInteger("currentAnimationState", 4);
                GetComponent<NavMeshAgent>().velocity = new Vector3(0, 0, 0);
                slerpTime = Time.time;
            }

            if (hellhoundSounds.clip != attackSounds[0] && hellhoundSounds.clip != attackSounds[1] && hellhoundSounds.clip != attackSounds[2])
            {
                Random.InitState(Mathf.RoundToInt(Time.time) * Mathf.RoundToInt(transform.position.x * transform.position.y * transform.position.z));
                hellhoundSounds.clip = attackSounds[Mathf.RoundToInt(Random.Range(0, 3))];
                hellhoundSounds.loop = false;
                hellhoundSounds.Play();               
            }
        }
        attackTimer = Time.time;
    }

    private void OnTriggerStay(Collider other) {
        if ((anim.GetCurrentAnimatorStateInfo(0).IsName("Attack(1)") || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack(2)") || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack(3)")) && (other.tag == "Player" || (other.tag == "Weapon" && !player.GetComponent<PlayerMovement>().isAttacking()))) {
            if (attackFix) {
                player.GetComponent<playerManager>().takeDamage(damage);
                playHitEffects();
                attackTimer = Time.time;
                boxCollider.GetComponent<BoxCollider>().enabled = false;
                attackFix = false;
            }
        }
    }

    public void playDeath() {
        if (anim.GetInteger("currentAnimationState") == 1 || anim.GetInteger("currentAnimationState") == 4) {
            anim.SetInteger("currentAnimationState", 9);
        } else {
            anim.SetInteger("currentAnimationState", 8);
        }

    }

    void OnCollisionEnter(Collision other) {
        currentCollisionPoint = other.contacts[0].point;
    }

    public void playHitEffects() {
        Instantiate(hitEffectObject, currentCollisionPoint, Quaternion.identity);
        //instantiate spark and flash
    }

    private void fightStages() {
        if (health < maxHealth * stage3Health && !stage3) {
            stage3 = true;
            GetComponent<NavMeshAgent>().speed = maxSpeed * stage3Health;
            anim.speed *= 0.7f;
            damage = 3;
        } else if (health < maxHealth * stage2Health && !stage2) {
            stage2 = true;
            GetComponent<NavMeshAgent>().speed = maxSpeed * stage2Health;
            GetComponent<NavMeshAgent>().autoBraking = true;
            damage = 7;
        }

        if (!stage2) {
            if (GetComponent<NavMeshAgent>().velocity.magnitude > 1 &&  Vector3.Angle(GetComponent<NavMeshAgent>().desiredVelocity, GetComponent<NavMeshAgent>().velocity) < 10.0f) {
                GetComponent<NavMeshAgent>().velocity = GetComponent<NavMeshAgent>().velocity.normalized * maxSpeed;
            }else if(Vector3.Distance(transform.position, player.transform.position) < minDistance) {
                GetComponent<NavMeshAgent>().velocity = GetComponent<NavMeshAgent>().velocity.normalized * maxSpeed;
            }
        }

        if (health <= 0 && alive == true) {
            alive = false;
        }
    }
}
