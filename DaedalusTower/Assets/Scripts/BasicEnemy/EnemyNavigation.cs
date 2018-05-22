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
    public float knockbackTime = 0.5f;

    public Transform destinationPoint;
    public float minDistance;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {

        if(Vector3.Distance(player.transform.position, transform.position) <= minDistance) {
            destinationPoint.position = transform.position;
        }else {
            destinationPoint.position = player.transform.position;
        }
        transform.GetComponent<NavMeshAgent>().destination = destinationPoint.position;

        if (Vector3.Distance(player.transform.position, this.gameObject.transform.position) <= minDistance && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 0")) {
            anim.Play("Take 001", 0, 0f);
        }else if (Vector3.Distance(player.transform.position, this.gameObject.transform.position) > minDistance && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 0")) {
            anim.Play("Take 001 1", 0, 0f);
        }

        if (health <= 0) {
            Destroy(this.gameObject);
        }

    }

    void attack() {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 0")) {
            anim.Play("Take 001 0", 0, 0f);
        }
    }

    public void takeDamage(GameObject source, int damage, int knockback) {
        if (vulnerable) {
            health = health - damage;
            //transform.position = transform.position - transform.forward * knockback * Time.deltaTime;
            //transform.GetComponent<NavMeshAgent>().speed = transform.GetComponent<NavMeshAgent>().speed * -1;

            vulnerableCount = Time.time;
        }

        if (Time.time > vulnerableCount + invulnerableStateLength) {
            vulnerable = true;
        } else {
            vulnerable = false;
        }

    }
}
