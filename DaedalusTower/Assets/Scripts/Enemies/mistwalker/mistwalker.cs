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

    public float stage2Health;
    public float stage3Health;
    public GameObject stage2Spawner;
    public GameObject stage3Spawner;
    private float maxHealth;

    private float stageReturnDelay;
    public float postSpawnDelay;

    private bool stage2;
    private bool stage3;
    private bool fightReset;

    public float finalStageSpeedBoost;

    public float pulseDelay;
    private float pulseTimeDelay;
    public GameObject pulsePad;
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
        maxHealth = health;
        bossCanvas.enabled = true;
        attackTimer = 0;

        stage2 = false;
        stage3 = false;
        fightReset = false;
    }

    // Update is called once per frame
    void Update() {
        if (health <= 0) {
            bossCanvas.enabled = false;
            Destroy(transform.parent.gameObject);
        }

        bossHealthBar.value = health;

        float playerDistance = Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.z), new Vector2(transform.position.x, transform.position.z));
        transform.GetComponentInParent<NavMeshAgent>().destination = player.transform.position;

        if (GetComponentInParent<NavMeshAgent>().speed == 0 && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 2") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 1")) {
            //anim.Play("Take 001", 0, 0f);
            anim.SetBool("moving", false);

        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001")) {
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
        }

        if (playerDistance <= minDistance) {
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


        if (fogDensity < maxFogDensity) {
            fogDensity += maxFogDensity * Time.deltaTime * fogGrowthRate / 100;
        } else if (fogDensity > maxFogDensity) {
            fogDensity = maxFogDensity;
        }
        RenderSettings.fogDensity = fogDensity;
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 2")) {
            clawsActive = false;
        }

        fightStages();

        if(Time.time > pulseDelay + pulseTimeDelay && stage2) {
            Instantiate(pulsePad, player.transform.position, Quaternion.identity);
            pulseTimeDelay = Time.time;
        }
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

    private void fightStages() {
        if(health < maxHealth * stage3Health && !stage3) {
            stage3Spawner = Instantiate(stage3Spawner, transform.parent.position, Quaternion.identity);
            stage3Spawner.GetComponent<BasicSpawner>().spawnTrigger = this.gameObject;
            GetComponentInParent<NavMeshAgent>().baseOffset += 100;
            stageReturnDelay = Time.time;
            stage3 = true;
            fightReset = true;
            attackDelay /= 2;
            anim.speed *= finalStageSpeedBoost;
            GetComponent<NavMeshAgent>().speed *= finalStageSpeedBoost;
        }else if(health < maxHealth * stage2Health && !stage2){
            GetComponent<SpawnTrigger>().bossFight = true;
            stage2Spawner = Instantiate(stage2Spawner, transform.parent.position, Quaternion.identity);
            stage2Spawner.GetComponent<BasicSpawner>().spawnTrigger = this.gameObject;
            GetComponentInParent<NavMeshAgent>().baseOffset += 100;
            stageReturnDelay = Time.time;
            stage2 = true;
            fightReset = true;
            attackDelay /= 2;
            pulseTimeDelay = Time.time;
        }

        if(Time.time > stageReturnDelay + postSpawnDelay && fightReset) {
            GetComponentInParent<NavMeshAgent>().baseOffset -= 100;
            GetComponentInParent<NavMeshAgent>().ResetPath();
            transform.parent.localPosition = new Vector3(0, 0.5f, 0);
            fightReset = false;
        }

        if (fightReset) {
            health += Mathf.RoundToInt((maxHealth/100f) * Time.deltaTime);
        }
    }

}
