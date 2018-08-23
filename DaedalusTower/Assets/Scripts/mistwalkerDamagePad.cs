using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mistwalkerDamagePad : MonoBehaviour {

    public float radius;
    public float growthSpeed;
    public int damage;
    public float timeFromDamToDest;
    float timerStart;

	// Use this for initialization
	void Start () {
        transform.localScale = new Vector3(0, 0.001f, 0);
        timerStart = Time.time + 1000000;
    }
	
	// Update is called once per frame
	void Update () {
        print(Time.time >= timerStart + timeFromDamToDest);
        if (Time.time >= timerStart + timeFromDamToDest) {
            Destroy(this.gameObject);
        }


        if (transform.localScale.x <= radius) {
            transform.localScale += new Vector3(Time.deltaTime * growthSpeed, 0, Time.deltaTime * growthSpeed);
        }
	}

    private void OnTriggerStay(Collider other) {
        if (transform.localScale.x >= radius) {
            timerStart = Time.time;
            other.GetComponent<playerManager>().takeDamage(damage);
            //play damage particle effect here
        }
    }
}
