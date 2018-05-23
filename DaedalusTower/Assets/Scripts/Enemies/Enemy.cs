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

    private int knockbackG;

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        if (this.gameObject.GetComponent<RangedAttack>() != null) {
            knockable = false;
        }

        if (health <= 0) {
            Destroy(this.gameObject);
        }

        if (knockable == true) {
            transform.position = transform.position - transform.forward * knockbackG * Time.deltaTime;
            GetComponent<NavMeshAgent>();
        }

        if (Time.time > vulnerableCount + knockbackTime) {
            knockable = false;
        } else {
            knockable = true;
        }

    }

    public void takeDamage(GameObject source, int damage, int knockback) {
        if (vulnerable) {
            knockbackG = knockback;
            health = health - damage;

            vulnerableCount = Time.time;
        }

        if (Time.time > vulnerableCount + invulnerableStateLength) {
            vulnerable = true;
        } else {
            vulnerable = false;
        }
    }

}
