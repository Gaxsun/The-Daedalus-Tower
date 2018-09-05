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
    // Use this for initialization
    void Start() {
        timerStart = Time.time;
    }

    // Update is called once per frame
    void Update() {
        if (transform.localScale.x <= diameter) {
            transform.localScale += new Vector3(Time.deltaTime * growthSpeed, Time.deltaTime * growthSpeed, Time.deltaTime * growthSpeed);
            target = GameObject.FindGameObjectWithTag("Player").transform.position;
            GameObject boss = GameObject.FindGameObjectWithTag("mistwalker");
            transform.position = new Vector3(boss.transform.position.x, boss.transform.position.y + 11, boss.transform.position.z);
        }
        if (transform.localScale.x >= diameter) {
            transform.LookAt(target);
            transform.position += transform.forward * Time.deltaTime;
        }
        if(Vector3.Distance(transform.position, target) >= 0.5) {
            timerStart = Time.time;
        }else {
            if (Time.time >= timerStart + timeFromDamToDest) {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if (transform.localScale.x >= diameter) {
            timerStart = Time.time;
            other.GetComponent<playerManager>().takeDamage(damage);
        }
    }
}
