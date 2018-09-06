using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerGroundPound : MonoBehaviour {

    public float radius;
    public float growthSpeed;
    public int damage;
    public float knockback;
    public float timeFromDamToDest;
    //public GameObject puddleBoom;
    float timerStart;

    // Use this for initialization
    void Start() {
        transform.localScale = new Vector3(0, 0.001f, 0);
        timerStart = Time.time;
    }

    // Update is called once per frame
    void Update() {
        print(Time.time >= timerStart + timeFromDamToDest);
        if (Time.time >= timerStart + timeFromDamToDest) {
            Destroy(this.gameObject);
        }

        if (transform.localScale.x <= radius) {
            transform.localScale += new Vector3(Time.deltaTime * growthSpeed, 0, Time.deltaTime * growthSpeed);
            timerStart = Time.time;
        } else {
            Color color = GetComponent<Renderer>().material.color;
            color.a -= Time.deltaTime;
            GetComponent<Renderer>().material.color = color;
        }

        if (transform.localScale.x >= radius) {
            //Instantiate(puddleBoom, transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerStay(Collider other) {
        if (transform.localScale.x >= radius) {
            
            if (other.GetComponent<Enemy>() != null) {
                other.GetComponent<Enemy>().takeDamage(this.gameObject, damage, knockback);
            }
            //Instantiate(puddleBoom, transform.position, Quaternion.identity);
        }
    }
}
