using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mistwalkerSpiritBomb : MonoBehaviour {

    public float diameter;
    public float growthSpeed;
    public int damage;
    public float timeFromDamToDest;
    float timerStart;
    private Vector3 target;
    private GameObject boss;
    // Use this for initialization
    void Start() {
        timerStart = Time.time;
        boss = GameObject.FindGameObjectWithTag("mistwalker");
    }

    // Update is called once per frame
    void Update() {
        if (transform.localScale.x <= diameter) {
            transform.localScale += new Vector3(Time.deltaTime * growthSpeed, Time.deltaTime * growthSpeed, Time.deltaTime * growthSpeed);
            target = GameObject.FindGameObjectWithTag("Player").transform.position;
            transform.position = boss.transform.localToWorldMatrix * new Vector3(boss.transform.position.x, boss.transform.position.y + 8, boss.transform.position.z);
            print(boss.transform.position.x + "  " + boss.transform.position.y + "  " + boss.transform.position.z);

        }
        if (transform.localScale.x >= diameter && Vector3.Distance(transform.position, target) >= 0.5) {
            boss.GetComponent<Animator>().SetBool("charging", false);
            transform.LookAt(target);
            transform.position += transform.forward * Time.deltaTime;
        }
        if(Vector3.Distance(transform.position, target) >= 0.5) {
            timerStart = Time.time;
        }else {
            if (Time.time >= timerStart + timeFromDamToDest) {
                boss.GetComponent<Animator>().SetBool("charging", false);
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if (transform.localScale.x >= diameter) {
            timerStart = Time.time;
            if (other.tag == "Player") {
                other.GetComponent<playerManager>().takeDamage(damage);
            }
        }
    }
}
