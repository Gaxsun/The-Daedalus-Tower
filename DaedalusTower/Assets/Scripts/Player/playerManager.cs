using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerManager : MonoBehaviour {

    //Holds current player config
    public GameObject currentWeapon;
    public GameObject weaponPosition;
    public Slider healthBar;
    public Canvas can;
    public int health = 200;

	// Use this for initialization
	void Start () {
        // Place selected weapon in player's hand
        Instantiate(currentWeapon, weaponPosition.transform);
        healthBar.maxValue = health;
    }
	
	// Update is called once per frame
	void Update () {
        healthBar.value = health;
        if (health <= 0) {
            can.enabled = false;
        }
    }

    public void takeDamage(int damage) {
        health = health - damage;
    }

}
