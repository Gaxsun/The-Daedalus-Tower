using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

    private GameObject player;

    bool vulnerable = true;
    float vulnerableCount;
    public float invulnerableStateLength = 1;
    bool knockable = false;
    public float knockbackTime = 0.5f;

    public int health = 200;

    private float knockbackG;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
    }
	//lmao
	// Update is called once per frame
	void Update () {
        if (this.gameObject.GetComponent<RangedAttack>() != null) {
            knockable = false;
        }

        if (health <= 0) {
            if (this.gameObject.GetComponent<Skeleton>() != null) {
                GetComponent<Skeleton>().playDeath();
            } else if (this.gameObject.GetComponent<hellhound>() != null) {
                //DO ABSOLUTELY NOTHIIIIIIING
            } else {
                Destroy(this.gameObject);
            }
        }

        if (knockable == true) {
            transform.position = transform.position - transform.forward * knockbackG * Time.deltaTime;
        }

        if (Time.time > vulnerableCount + knockbackTime) {
            knockable = false;
        } else {
            knockable = true;
        }

    }

    public void takeDamage(GameObject source, int damage, float knockback) {
        if (vulnerable) {

            if (GetComponent<Skeleton>() != null) {
                if (GetComponent<Skeleton>().anim.GetInteger("currentAnimationState") == 5) {
                    return;
                }
                AudioSource skeletonSounds = GetComponent<Skeleton>().skeletonSounds;
                skeletonSounds.clip = GetComponent<Skeleton>().damageSounds[Mathf.RoundToInt(Random.Range(0, 3))];
                skeletonSounds.loop = false;
                skeletonSounds.Play();
                GetComponent<Skeleton>().playDamaged();
            }else if (GetComponent<hellhound>() != null)
            {
                AudioSource hellhoundSounds = GetComponent<hellhound>().hellhoundSounds;
                hellhoundSounds.clip = GetComponent<hellhound>().damageSounds[Mathf.RoundToInt(Random.Range(0, 2))];
                hellhoundSounds.loop = false;
                hellhoundSounds.Play();
                GetComponent<hellhound>().playDamaged();
            }
            player.GetComponent<playerManager>().addGodPower(13);
            knockbackG = knockback;
            health = health - damage;
            if (source.GetComponent<Weapon>() != null) {
                source.GetComponent<Weapon>().playHitEffects();
            }
            vulnerableCount = Time.time;
        }

        if (Time.time > vulnerableCount + invulnerableStateLength) {
            vulnerable = true;
        } else {
            vulnerable = false;
        }
    }

}
