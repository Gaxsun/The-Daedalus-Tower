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
    public GameObject fill;
    public int healthRegen = 2; // per sec
    public int health = 200;
    private float secondCount;

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

        print(health);
        if (gameObject.GetComponent<PlayerMovement>().playerCam.GetComponent<CameraFollow>().bossFight && fill.GetComponent<Image>().color.g < 0.3) {
            fill.GetComponent<Image>().color = new Color(fill.GetComponent<Image>().color.r, fill.GetComponent<Image>().color.g + Time.deltaTime*0.1f, fill.GetComponent<Image>().color.b + Time.deltaTime * 0.1f);
        }

        if (health + healthRegen <= healthBar.maxValue && Time.time > secondCount + 1) {
            health = health + healthRegen;
            secondCount = Time.time;
        } else if(Time.time > secondCount + 1) {
            health = Mathf.RoundToInt(healthBar.maxValue);
            secondCount = Time.time;
        }

    }

    public void takeDamage(int damage) {
        health = health - damage;
    }

}
