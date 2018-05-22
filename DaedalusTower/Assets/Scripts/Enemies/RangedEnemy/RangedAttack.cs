using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : MonoBehaviour {
    public GameObject projectile;
    public GameObject eye;
    public float fireRate;

    private GameObject target;
    private float fireTimer;
	// Use this for initialization
	void Start () {
        fireTimer = Time.time;
        target = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
        if(Time.time > fireTimer + fireRate) {
            Instantiate(projectile, eye.transform);
            fireTimer = Time.time;
        }
	}
}
