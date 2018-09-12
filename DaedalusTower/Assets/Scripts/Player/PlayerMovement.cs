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
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), Vector3.down, out hit, 0.11f))
        {
            isAirBorne = false;
        }
        else
        {
            isAirBorne = true;
        }
    }

    void FixedUpdate() {
        if (dashing) {
            Dash();
            dashEnabled = false;
        }
    }

    public void forwardAxisMovement(float direction) {
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

        anim.Play("run", 0, 0f);

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
            
            if (jumpSound.clip != jumpClip[1] || jumpSound.isPlaying == false)
            {
                jumpSound.Stop();
                jumpSound.clip = null;
                jumpSound.clip = jumpClip[1];
                jumpSound.loop = false;
                jumpSound.Play();
            }

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
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("1L")) {
            GetComponent<playerManager>().weaponPosition.GetComponentInChildren<Weapon>().heavyAttack = false;
            return true;
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("1H")) {
            GetComponent<playerManager>().weaponPosition.GetComponentInChildren<Weapon>().heavyAttack = true;
            return true;
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("2L")) {
            GetComponent<playerManager>().weaponPosition.GetComponentInChildren<Weapon>().heavyAttack = false;
            return true;
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("6H")) {
            GetComponent<playerManager>().weaponPosition.GetComponentInChildren<Weapon>().heavyAttack = true;           
            return true;
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("2H")) {
            GetComponent<playerManager>().weaponPosition.GetComponentInChildren<Weapon>().heavyAttack = true;
            return true;
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("5L")) {
            GetComponent<playerManager>().weaponPosition.GetComponentInChildren<Weapon>().heavyAttack = false;
            return true;
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("3L")) {
            GetComponent<playerManager>().weaponPosition.GetComponentInChildren<Weapon>().heavyAttack = false;
            return true;
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("4H")) {
            GetComponent<playerManager>().weaponPosition.GetComponentInChildren<Weapon>().heavyAttack = true;
            return true;
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("7H")) {
            GetComponent<playerManager>().weaponPosition.GetComponentInChildren<Weapon>().heavyAttack = true;
            return true;
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("3H")) {
            GetComponent<playerManager>().weaponPosition.GetComponentInChildren<Weapon>().heavyAttack = true;
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
        
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("run")) {
            animationCurrentFrame = "run";
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("idle")) {
            animationCurrentFrame = "idle";
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("1L")) {
            animationCurrentFrame = "1L";
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("1H")) {
            animationCurrentFrame = "1H";
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("2L")) {
            animationCurrentFrame = "2L";
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("6H")) {
            animationCurrentFrame = "6H";   
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("2H")) {
            animationCurrentFrame = "2H";
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("5L")) {
            animationCurrentFrame = "5L";
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("3L")) {
            animationCurrentFrame = "3L";
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("4H")) {
            animationCurrentFrame = "4H";
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("7H")) {
            animationCurrentFrame = "7H";
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("3H")) {
            animationCurrentFrame = "3H";
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("6L")) {
            animationCurrentFrame = "6L";
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("1L 0")) {
            animationCurrentFrame = "1L 0";
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("5H")) {
            animationCurrentFrame = "5H";
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("8H")) {
            animationCurrentFrame = "8H";
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("2L 0")) {
            animationCurrentFrame = "2L 0";
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("4L")) {
            animationCurrentFrame = "4L";
        } else {
            animationCurrentFrame = "notAttack";
        }
        if (animationLastFrame != animationCurrentFrame) {
            anim.SetInteger("nextAttack", 0);
            
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
            if (animationLastFrame == "4H") {
                return true;
            } else if (animationLastFrame == "4L") {
                return true;
            } else if (animationLastFrame == "5H") {
                return true;
            } else if (animationLastFrame == "6H") {
                return true;
            }
        }
        return false;
    }

}
