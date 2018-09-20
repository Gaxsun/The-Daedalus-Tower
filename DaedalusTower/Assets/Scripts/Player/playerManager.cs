using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class playerManager : MonoBehaviour {

    //Holds current player config
    public GameObject currentWeapon;
    public GameObject currentArmor;
    public GameObject weaponPosition;
    public Slider healthBar;
    public Canvas can;
    public Canvas death;
    public Canvas bossCanvas;
    public Canvas win;
    public Canvas introTut;
    public GameObject fill;
    public int health = 100;
    public int healthMax = 100;
    private float secondCount;
    private float restartCount = 0;

    public Slider powerOfGodsBar;
    public Image powerOfGodsBarBG;
    public bool powerOfGodsActive = false;
    public int powerOfGodsMax = 200;
    public float powerOfGods = 0;
    public float powerOfGodsDecayRate = 1;
    public float powerOfGodsSpeedBoost = 2;
    public int powerOfGodsDamageBoost = 3;
    public int healthRegen = 2; // per sec
    public Canvas inventoryWindow;

    public AudioClip[] playerSounds;
    public AudioSource playerSoundsSource;
    public bool dead;

    public List<GameObject> inventory;

	// Use this for initialization
	void Start () {
        // Place selected weapon in player's hand
        Instantiate(currentWeapon, weaponPosition.transform);
        healthBar.maxValue = healthMax;
        powerOfGodsBar.maxValue = powerOfGodsMax;
        inventoryWindow.enabled = false;

        dead = false;
    }
	
	// Update is called once per frame
	void Update () {

        if (introTut.enabled == true)
        {
            inventoryWindow.enabled = false;
        }

        powerOfTheGods();
        

        healthBar.value = health;
        if (health <= 0) {
            can.enabled = false;
            //win.enabled = false;
            death.enabled = true;
            restartCount += Time.deltaTime;
            print(death.enabled);

            if(playerSoundsSource.isPlaying == false && dead == false)
            {
                playerSoundsSource.Stop();
                playerSoundsSource.clip = playerSounds[0];
                playerSoundsSource.loop = false;
                playerSoundsSource.Play();
                dead = true;
            }

        }

        if (!bossCanvas.enabled && transform.position.x < -90) {
            win.enabled = true;
            restartCount += Time.deltaTime;
        }


        if (death.enabled || win.enabled) {
            if ( restartCount > 5) {
                Scene loadedLevel = SceneManager.GetActiveScene();
                SceneManager.LoadScene(loadedLevel.buildIndex);
            }            
        }

        if (gameObject.GetComponent<PlayerMovement>().playerCam.GetComponent<CameraFollow>().bossFight && fill.GetComponent<Image>().color.g < 0.3) {
            fill.GetComponent<Image>().color = new Color(fill.GetComponent<Image>().color.r, fill.GetComponent<Image>().color.g + Time.deltaTime*0.1f, fill.GetComponent<Image>().color.b + Time.deltaTime * 0.1f);
        }

    }

    void powerOfTheGods() {
        powerOfGodsBar.value = powerOfGods;

        //set bar color if ready to activate.
        if (powerOfGods == powerOfGodsMax) {
            powerOfGodsBarBG.rectTransform.localScale = new Vector3(1.05f, 2.5f, powerOfGodsBarBG.rectTransform.localScale.z);
            powerOfGodsBarBG.color = new Color(255,255,255, 82.5f + (17.5f * Mathf.Sin(Time.time)));
        } else if(powerOfGodsActive) {
            powerOfGodsBarBG.rectTransform.localScale = new Vector3(1.05f, 2.5f, powerOfGodsBarBG.rectTransform.localScale.z);
            powerOfGodsBarBG.color = new Color(255, 255, 255, 100);
        } else {
            powerOfGodsBarBG.rectTransform.localScale = new Vector3(1, 1, powerOfGodsBarBG.rectTransform.localScale.z);
            powerOfGodsBarBG.color = new Color(255, 255, 255, 65);
        }

        if (powerOfGodsActive) {
            Component[] materialRenderers;
            materialRenderers = GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in materialRenderers)
            {
                renderer.material.SetInt("_godPowerActive", 1);
            }

            powerOfGods -= powerOfGodsDecayRate * Time.deltaTime;

            if (health + healthRegen <= healthBar.maxValue && Time.time > secondCount + 1) {
                health = health + healthRegen;
                secondCount = Time.time;
            } else if (Time.time > secondCount + 1) {
                health = Mathf.RoundToInt(healthBar.maxValue);
                secondCount = Time.time;
            }
        } else {
            Component[] materialRenderers;
            materialRenderers = GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in materialRenderers) {
                renderer.material.SetInt("_godPowerActive", 0);
            }
        }

        if (powerOfGods <= 0) {
            endGodPower();
        }

    }

    public void endGodPower() {
        if (powerOfGodsActive) {
            powerOfGodsActive = false;
            GetComponent<PlayerMovement>().moddableSpeed = GetComponent<PlayerMovement>().moddableSpeed / powerOfGodsSpeedBoost;
            GetComponent<PlayerMovement>().speed = GetComponent<PlayerMovement>().speed / powerOfGodsSpeedBoost;
            weaponPosition.GetComponentInChildren<Weapon>().baseDamage = Mathf.RoundToInt(weaponPosition.GetComponentInChildren<Weapon>().baseDamage / powerOfGodsDamageBoost);
        }
        //weaponPosition.GetComponentInChildren<Weapon>().baseDamage = GetComponent<Weapon>().baseDamage / powerOfGodsDamageBoost;
    }

    public void addGodPower(int powerToAdd) {
        if (powerOfGods + powerToAdd >= powerOfGodsMax) {
            powerOfGods = powerOfGodsMax;
        } else {
            powerOfGods += powerToAdd;
        }
    }

    public void takeDamage(int damage) {
        health = health - damage;
        if (powerOfGodsActive) {
            powerOfGods -= damage;
        } 

        if (playerSoundsSource.isPlaying == false && dead == false)
        {
            playerSoundsSource.Stop();
            playerSoundsSource.clip = playerSounds[Mathf.RoundToInt(Random.Range(1, 3))];
            playerSoundsSource.loop = false;
            playerSoundsSource.Play();
        }

    }

}
