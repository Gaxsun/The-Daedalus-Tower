using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerManager : MonoBehaviour {

    //Holds current player config
    public GameObject currentWeapon;
    public GameObject weaponPosition;
    public int health = 200;

	// Use this for initialization
	void Start () {
        // Place selected weapon in player's hand
        Instantiate(currentWeapon, weaponPosition.transform);        
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    public void takeDamage(int damage) {
        health = health - damage;
    }

}
