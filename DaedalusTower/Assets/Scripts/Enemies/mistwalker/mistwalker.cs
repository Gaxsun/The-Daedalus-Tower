using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class mistwalker : MonoBehaviour {

    public bool clawsActive = false;
    public int health = 1000;

    public Animator anim;

    private GameObject player;
    public float minDistance;

    public Slider bossHealthBar;
    public Canvas bossCanvas;

    private float fogDensity;
    public float maxFogDensity;
    public float fogGrowthRate;

    private float normSpeed;
    private float normAccel;
    
    public float attackDelay;
    private float attackTimer;
    private float attackPrep;
    private float attackChargeTime;

    bool vulnerable = true;
    float vulnerableCount;
    public float invulnerableStateLength = 1;
    private bool attacking = false;
    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerMovement>().playerCam.GetComponent<CameraFollow>().bossFight = true;
        RenderSettings.fog = true;
        fogDensity = 0;
        normSpeed = GetComponentInParent<NavMeshAgent>().speed;
        normAccel = GetComponentInParent<NavMeshAgent>().acceleration;

        bossCanvas = GameObject.FindGameObjectWithTag("bossCanvas").GetComponent<Canvas>();
        bossHealthBar = GameObject.FindGameObjectWithTag("bossCanvas").GetComponent<Canvas>().GetComponentInChildren<Slider>();

        bossHealthBar.maxValue = health;
        
        bossCanvas.enabled = true;
        attackTimer = 0;
    }
	
	// Update is called once per frame
	void Update () {

        if (health <= 0) {
            bossCanvas.enabled = false;
        }

        bossHealthBar.value = health;

        float playerDistance = Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.z), new Vector2(transform.position.x, transform.position.z));
        transform.GetComponentInParent<NavMeshAgent>().destination = player.transform.position;
        
        if (GetComponentInParent<NavMeshAgent>().speed == 0 && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 2") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 1")) {
            //anim.Play("Take 001", 0, 0f);
            anim.SetBool("moving", false);
            
        }
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001")) {
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
        }

        if(playerDistance <= minDistance) {
            GetComponentInParent<NavMeshAgent>().speed = 0;
            GetComponentInParent<NavMeshAgent>().acceleration = 1000;
            if (Time.time > attackTimer + attackDelay) {
                attackPrep = Time.time;
                attackChargeTime = 2;
                clawsActive = false;
                attack();
            }
            if (Time.time > attackPrep + attackChargeTime && attacking) {
                clawsActive = true;
                attackPrep = Mathf.Infinity;
                attacking = false;
            }
        } else {
            GetComponentInParent<NavMeshAgent>().speed = normSpeed;
            GetComponentInParent<NavMeshAgent>().acceleration = normAccel;
            move();
        }


        if(fogDensity < maxFogDensity) {
            fogDensity += maxFogDensity * Time.deltaTime * fogGrowthRate/100;
        }else if(fogDensity > maxFogDensity) {
            fogDensity = maxFogDensity;
        }
        RenderSettings.fogDensity = fogDensity;
    }

    void move() {
        attackTimer = 0;
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 0")) {
            //anim.Play("Take 001 0", 0, 0f);
            anim.SetBool("moving", true);
        }
    }

    void attack() {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 2")) {
            transform.LookAt(player.transform);
            attacking = true;
            anim.Play("Take 001 2", 0, 0f);
            //anim.SetTrigger("attack");
        }
        print("Mistwalker Attacking");
        attackTimer = Time.time;
    }

    public void takeDamage(int damage) {
        if (vulnerable) {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 2")) {
                anim.Play("Take 001 1", 0, 0f);
                print(!anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 2"));
            }
            health = health - damage;
            print("Mistwalker Took Damage");

            vulnerableCount = Time.time;

        }

        if (Time.time > vulnerableCount + invulnerableStateLength) {
            vulnerable = true;
        } else {
            vulnerable = false;
        }

    }

}
