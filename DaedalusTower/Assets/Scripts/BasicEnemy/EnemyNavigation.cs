using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavigation : MonoBehaviour {
    
    private GameObject player;

    public Animator anim;

    public int health = 200;

    bool vulnerable = true;
    float vulnerableCount;
    public float invulnerableStateLength = 1;
    bool knockable = true;
    public float knockbackTime = 0.5f;
    
    public float minDistance;

    private int knockbackG;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<NavMeshAgent>().destination = player.transform.position;

        if (Vector3.Distance(player.transform.position, this.gameObject.transform.position) <= minDistance && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 0")) {
            anim.Play("Take 001", 0, 0f);
        }else if (Vector3.Distance(player.transform.position, this.gameObject.transform.position) > minDistance && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 0")) {
            anim.Play("Take 001 1", 0, 0f);
        }

        if (health <= 0) {
            Destroy(this.gameObject);
        }

        if (knockable == false) {
            transform.position = transform.position - transform.forward * knockbackG * Time.deltaTime;
            GetComponent<NavMeshAgent>();
        }

        if (Time.time > vulnerableCount + knockbackTime) {
            knockable = true;
        } else {
            knockable = false;
        }
    }

    void attack() {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 0")) {
            anim.Play("Take 001 0", 0, 0f);
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
