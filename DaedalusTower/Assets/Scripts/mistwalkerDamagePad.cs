﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mistwalkerDamagePad : MonoBehaviour {

    public float radius;
    public float growthSpeed;
    public int damage;
    public float timeFromDamToDest;
    public bool damageTicked = false;
    public GameObject puddleBoom;
    float timerStart;

	// Use this for initialization
	void Start () {
        transform.localScale = new Vector3(0, 0.001f, 0);
        timerStart = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
        if (Time.time >= timerStart + timeFromDamToDest) {
            Destroy(this.gameObject);
        }

        if (transform.localScale.x <= radius) {
            transform.localScale += new Vector3(Time.deltaTime * growthSpeed, 0, Time.deltaTime * growthSpeed);
            timerStart = Time.time;
        }
        if (transform.localScale.x >= radius) {
            Instantiate(puddleBoom, transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerStay(Collider other) {
        if (transform.localScale.x >= radius && !damageTicked) {
            damageTicked = true;
            timerStart = Time.time;
            other.GetComponent<playerManager>().takeDamage(damage);
            Instantiate(puddleBoom, transform.position, Quaternion.identity);
            damageTicked = true;
        }
    }
}
