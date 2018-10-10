using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    //0->no attack  1->light attack  2->heavy attack   

    public Camera playerCam;

    public Animator anim;

    public CapsuleCollider capsule;

    public GameObject groundPoundEffect;

    Rigidbody rb;

    public float speed;
    public float moddableSpeed;
    //public float backwardMoveSpeed;
    public float sidewaysMoveSpeed;

    private float attackAgainDelay;
    private string animationLastFrame;
    private string animationCurrentFrame;

    public float faceEnemyRadius = 0.3f;
    public float faceEnemyDistance = 1;

    public float dashSpeed;
    public float dashTime;
    private float dashTimeStart;
    public float dashCooldown;
    float dashDirectionX;
    float dashDirectionY;
    public bool dashEnabled = true;
    
    private AudioSource jumpSound;
    private AudioClip[] jumpClip;
    public AudioClip[] bangClip;
    public AudioSource bangSource;


    public bool isAirBorne = false;
    public float jumpForce;
    float jumpCoolDown;

    public float rotateSpeed;

    bool dashing = false;

    public float idleChangeDelay = 3;
    private float idleTimer;

    private bool dontPlayDeathAgain = false;

    // Game Time Started
    void Start() {
        anim = GetComponentInChildren<Animator>();
        rb = this.GetComponent<Rigidbody>();
        jumpSound = GetComponent<PlayerInput>().inputSoundsSource;
        jumpClip = GetComponent<PlayerInput>().inputSounds;
        moddableSpeed = speed;
    }

    // Update is called once per frame
    void Update() {

        if (GetComponent<playerManager>().health <= 0) {
            if (!dontPlayDeathAgain) {
                Random.InitState(Mathf.RoundToInt(Time.time));
                int rand = Random.Range(1, 3);
                if (rand == 1) {
                    anim.Play("Death");
                } else {
                    anim.Play("Death2");
                }
                dontPlayDeathAgain = true;
            }
        } else {
            if (gameObject.GetComponentInChildren<Transform>().gameObject.GetComponentInChildren<Weapon>().attackActive == false && GetComponent<PlayerInput>().controlsEnabled) {
                setRotation();
            } else {
                faceEnemy();
            }

            if (!isAttacking()) {
                gameObject.GetComponentInChildren<Transform>().gameObject.GetComponentInChildren<Weapon>().attackActive = false;
                moddableSpeed = speed;
            } else {
                gameObject.GetComponentInChildren<Transform>().gameObject.GetComponentInChildren<Weapon>().attackActive = true;
                moddableSpeed = speed / 10;
            }

            if (Time.time >= dashCooldown + dashTimeStart) {
                dashEnabled = true;
            }

            nextAttackReset();
            RaycastHit hit;
            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), Vector3.down, out hit, 0.11f)) {
                isAirBorne = false;
            } else {
                isAirBorne = true;
            }

            if (idleTimer + idleChangeDelay < Time.time) {
                Random.InitState(Mathf.RoundToInt(Time.time));
                int rand = Random.Range(1, 3);
                anim.SetInteger("idleState", rand);
                idleTimer = Time.time;
            }
        }
    }

    void FixedUpdate() {
        if (dashing) {
            Dash();
            dashEnabled = false;
        }
    }

    public void forwardAxisMovement(float direction) {
        CameraFollow followCam = playerCam.GetComponent<CameraFollow>();
        if (followCam.lockOn && Vector3.Distance(transform.position, followCam.lockTarget.transform.position) < 1.3f && direction < 0.0f) {
            direction = 0;
        }
        transform.position = transform.position + new Vector3(playerCam.transform.forward.x, 0, playerCam.transform.forward.z).normalized * moddableSpeed * -direction * Time.deltaTime;
        playerCam.transform.position = playerCam.transform.position + new Vector3(playerCam.transform.forward.x, 0, playerCam.transform.forward.z).normalized * moddableSpeed * -direction * Time.deltaTime;
    }

    public void sidewaysAxisMovement(float direction) {
        CameraFollow followCam = playerCam.GetComponent<CameraFollow>();
        transform.RotateAround(playerCam.transform.position, transform.up, moddableSpeed * direction * Time.deltaTime * 8 * (4 - Mathf.Sqrt(followCam.springOffset.x * followCam.springOffset.x + followCam.springOffset.z * followCam.springOffset.z)/followCam.cameraDistance * 3));
        followCam.playerCounterRotate();
    }

    private void setRotation() {
        Vector2 joystick = new Vector2(Input.GetAxis("LeftStickX"), Input.GetAxis("LeftStickY"));
        if (joystick.x != 0 || joystick.y != 0) {
            float angle = Mathf.Atan2(joystick.x, -joystick.y) * Mathf.Rad2Deg + playerCam.transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }

    }

    private void faceEnemy() {
        RaycastHit hit;
        if (Physics.SphereCast(new Vector3(transform.position.x, transform.position.y + capsule.height/2, transform.position.z), faceEnemyRadius, transform.forward, out hit, faceEnemyDistance)) {
            if (hit.transform.tag == "enemy" || hit.transform.tag == "mistwalker") {
                transform.LookAt(new Vector3(hit.transform.position.x, transform.position.y, hit.transform.position.z));
            }
        }
    }

    public void Dash() {
        if (Time.time >= dashTime + dashTimeStart || dashCheck()) {
            dashing = false;
            GetComponent<PlayerInput>().controlsEnabled = true;
            playIdle();
            dashTimeStart = Time.time;
        }

        gameObject.GetComponentInChildren<Transform>().gameObject.GetComponentInChildren<Weapon>().attackActive = false;
        moddableSpeed = speed;

        anim.Play("Run", 0, 0f);

        float POTGMod = 1;
        if (GetComponent<playerManager>().powerOfGodsActive) {
            POTGMod = GetComponent<playerManager>().powerOfGodsSpeedBoost;
        }

        forwardAxisMovement((dashDirectionY * dashSpeed)/POTGMod);
        sidewaysAxisMovement(dashDirectionX * dashSpeed);
        playerCam.GetComponent<CameraFollow>().springArm();
    }

    private bool dashCheck() {
        RaycastHit hit;
        
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 1)) {
            return true;
        }
        return false;
    }

    public void DashStart() {
        if (!dashCheck()) {

            dashDirectionX = Input.GetAxis("LeftStickX") / (Mathf.Sqrt(Mathf.Pow(Input.GetAxis("LeftStickX"), 2) + Mathf.Pow(Input.GetAxis("LeftStickY"), 2)));

            dashDirectionY = Mathf.Sin(Mathf.Acos(Input.GetAxis("LeftStickX") / (Mathf.Sqrt(Mathf.Pow(Input.GetAxis("LeftStickX"), 2) + Mathf.Pow(Input.GetAxis("LeftStickY"), 2)))));

            if (Input.GetAxis("LeftStickY") < 0) {
                dashDirectionY = dashDirectionY * -1;
            }

            if (double.IsNaN(dashDirectionX)) {
                dashDirectionX = 0;
            }
            if (double.IsNaN(dashDirectionY)) {
                dashDirectionY = 0;
            }
            if (Input.GetAxis("LeftStickX") == 0 && Input.GetAxis("LeftStickY") == 0) {
                dashDirectionY = -1;
            }

            dashing = true;
            GetComponent<PlayerInput>().controlsEnabled = false;
            anim.Play("run", 0, 0f);
            gameObject.GetComponentInChildren<Transform>().gameObject.GetComponentInChildren<Weapon>().attackActive = false;
            dashTimeStart = Time.time;
        }
        
    }

    public void Jump() {
        if (!isAirBorne && Time.time > jumpCoolDown + 1) {
            rb.AddForce(new Vector3(0,jumpForce,0));
            jumpCoolDown = Time.time;


            if (!isAttacking()) {
                if (anim.GetBool("running")) {
                    anim.Play("Jump");
                } else {
                    anim.Play("jumpStationary");
                }
            }


            if (jumpSound.clip != jumpClip[1] || jumpSound.isPlaying == false)
            {
                jumpSound.Stop();
                jumpSound.clip = null;
                jumpSound.clip = jumpClip[1];
                jumpSound.loop = false;
                jumpSound.Play();
            }

        } else {
            anim.SetBool("jumping", false);
        }
    }

    public void playRun() {
        //if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 0") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 2") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 3") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 4") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 5")) {
        //    anim.Play("Take 001 0", 0, 0f);
        anim.SetBool("running", true);

        //}
    }

    public void playIdle() {
        //if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 2") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 3") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 4") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 5")) {
            //anim.Play("Take 001", 0, 0f);
            anim.SetBool("running", false);
        //}
    }

    public bool attackInputSanitization() {
        if (Time.time > attackAgainDelay + 0.04f) {
            attackAgainDelay = Time.time;
            return true;
        } else {
            return false;
        }
    }

    public bool isAttacking() {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Slash")) {
            GetComponent<playerManager>().weaponPosition.GetComponentInChildren<Weapon>().heavyAttack = false;
            return true;
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Slash2")) {
            GetComponent<playerManager>().weaponPosition.GetComponentInChildren<Weapon>().heavyAttack = false;
            return true;
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Kick")) {
            GetComponent<playerManager>().weaponPosition.GetComponentInChildren<Weapon>().heavyAttack = false;
            return true;
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("ComboSlash1")) {
            GetComponent<playerManager>().weaponPosition.GetComponentInChildren<Weapon>().heavyAttack = true;           
            return true;
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("ComboSlash2")) {
            GetComponent<playerManager>().weaponPosition.GetComponentInChildren<Weapon>().heavyAttack = false;
            return true;
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("ComboSlash3")) {
            GetComponent<playerManager>().weaponPosition.GetComponentInChildren<Weapon>().heavyAttack = false;
            return true;
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("SpinAttack")) {
            GetComponent<playerManager>().weaponPosition.GetComponentInChildren<Weapon>().heavyAttack = true;
            return true;
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("SpinAttack2")) {
            GetComponent<playerManager>().weaponPosition.GetComponentInChildren<Weapon>().heavyAttack = true;
            return true;
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Slash3")) {
            GetComponent<playerManager>().weaponPosition.GetComponentInChildren<Weapon>().heavyAttack = true;
            return true;
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("SlideAttack")) {
            GetComponent<playerManager>().weaponPosition.GetComponentInChildren<Weapon>().heavyAttack = false;
            return true;
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("6L")) {
            GetComponent<playerManager>().weaponPosition.GetComponentInChildren<Weapon>().heavyAttack = false;
            return true;
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("1L 0")) {
            GetComponent<playerManager>().weaponPosition.GetComponentInChildren<Weapon>().heavyAttack = false;
            return true;
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("5H")) {
            GetComponent<playerManager>().weaponPosition.GetComponentInChildren<Weapon>().heavyAttack = true;
            return true;
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("8H")) {
            GetComponent<playerManager>().weaponPosition.GetComponentInChildren<Weapon>().heavyAttack = true;
            return true;
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("2L 0")) {
            GetComponent<playerManager>().weaponPosition.GetComponentInChildren<Weapon>().heavyAttack = false;
            return true;
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("4L")) {
            GetComponent<playerManager>().weaponPosition.GetComponentInChildren<Weapon>().heavyAttack = false;
            return true;
        } else {
            return false;
        }
    }

    void nextAttackReset() {
        
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Run")) {
            animationCurrentFrame = "Run";
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle")) {
            animationCurrentFrame = "Idle";
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle2")) {
            animationCurrentFrame = "Idle2";
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle3")) {
            animationCurrentFrame = "Idle3";
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Jump")) {
            animationCurrentFrame = "Jump";
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("JumpStationary")) {
            animationCurrentFrame = "JumpStationary";   
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Hurt")) {
            animationCurrentFrame = "Hurt";
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Hurt2")) {
            animationCurrentFrame = "Hurt2";
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Slash")) {
            animationCurrentFrame = "Slash";
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Slash2")) {
            animationCurrentFrame = "Slash2";
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Slash3")) {
            animationCurrentFrame = "Slash3";
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("SpinAttack")) {
            animationCurrentFrame = "SpinAttack";
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("SpinAttack2")) {
            animationCurrentFrame = "SpinAttack2";
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("ComboSlash1")) {
            animationCurrentFrame = "ComboSlash1";
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("ComboSlash2")) {
            animationCurrentFrame = "ComboSlash2";
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("ComboSlash3")) {
            animationCurrentFrame = "ComboSlash3";
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Kick")) {
            animationCurrentFrame = "Kick";
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Death")) {
            animationCurrentFrame = "Death";
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Death2")) {
            animationCurrentFrame = "Death2";
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("SlideAttack")) {
            animationCurrentFrame = "SlideAttack";
        } else {
            animationCurrentFrame = "notAttack";
        }
        if (animationLastFrame != animationCurrentFrame) {
            anim.SetInteger("nextAttack", 0);

            if (animationCurrentFrame == "Idle") {
                anim.SetInteger("idleState", 0);
                idleTimer = Time.time;
            }

            setRotation();
        }

        groundPound();
        animationLastFrame = animationCurrentFrame;
    }

    private void groundPound() {
        if (isGroundPoundAttack()) {
            Instantiate(groundPoundEffect, transform.position, Quaternion.identity);

            if (bangSource.isPlaying == false)
            {
                bangSource.clip = bangClip[0];
                bangSource.loop = false;
                bangSource.Play();
            }
        }
        
    }

    private bool isGroundPoundAttack() {
        if (animationCurrentFrame != animationLastFrame) {
            if (animationLastFrame == "SpinAttack2") {
                return true;
            } 
        }
        return false;
    }

    public void playHurt() {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") || anim.GetCurrentAnimatorStateInfo(0).IsName("Idle2") || anim.GetCurrentAnimatorStateInfo(0).IsName("Idle3")) {

            Random.InitState(Mathf.RoundToInt(Time.time));
            int rand = Mathf.RoundToInt(Random.Range(0, 1));
            if (rand == 0) {
                anim.Play("Hurt");
            } else {
                anim.Play("Hurt2");
            }            
        }
    }
}
