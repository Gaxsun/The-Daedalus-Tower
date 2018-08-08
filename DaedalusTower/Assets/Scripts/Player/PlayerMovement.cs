using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public Camera playerCam;

    public Animator anim;

    public CapsuleCollider capsule;

    Rigidbody rb;

    public float forwardMoveSpeed;
    private float moddableForwardMoveSpeed;
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

    bool isAirBorne = false;
    public float jumpForce;
    float jumpCoolDown;

    public float rotateSpeed;
    private float moddableRotateSpeed;

    bool dashing = false;


    // Game Time Started
    void Start() {
        anim = GetComponentInChildren<Animator>();
        rb = this.GetComponent<Rigidbody>();

        moddableForwardMoveSpeed = forwardMoveSpeed;
        moddableRotateSpeed = rotateSpeed;

    }

    // Update is called once per frame
    void Update() {
        if (gameObject.GetComponentInChildren<Transform>().gameObject.GetComponentInChildren<Weapon>().attackActive == false && GetComponent<PlayerInput>().controlsEnabled) {
            setRotation();
        } else {
            faceEnemy();
        }
        
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 2") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 3") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 4") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 5")) {
            gameObject.GetComponentInChildren<Transform>().gameObject.GetComponentInChildren<Weapon>().attackActive = false;
            moddableForwardMoveSpeed = forwardMoveSpeed;
            moddableRotateSpeed = rotateSpeed;
        } else {
            gameObject.GetComponentInChildren<Transform>().gameObject.GetComponentInChildren<Weapon>().attackActive = true;
            moddableForwardMoveSpeed = forwardMoveSpeed / 10;
            moddableRotateSpeed = rotateSpeed / 10;
        }

        if (Time.time >= dashCooldown + dashTimeStart) {
            dashEnabled = true;
        }
        nextAttackReset();

    }

    void FixedUpdate() {
        if (dashing) {
            Dash();
            dashEnabled = false;
        }
    }

    public void forwardAxisMovement(float direction) {
        transform.position = transform.position + new Vector3(playerCam.transform.forward.x, 0, playerCam.transform.forward.z).normalized * moddableForwardMoveSpeed * -direction * Time.deltaTime;
        playerCam.transform.position = playerCam.transform.position + new Vector3(playerCam.transform.forward.x, 0, playerCam.transform.forward.z).normalized * moddableForwardMoveSpeed * -direction * Time.deltaTime;
    }

    public void sidewaysAxisMovement(float direction) {
        CameraFollow followCam = playerCam.GetComponent<CameraFollow>();
        transform.RotateAround(playerCam.transform.position, transform.up, moddableRotateSpeed * direction * Time.deltaTime);// * (2 - Mathf.Sqrt(followCam.springOffset.x * followCam.springOffset.x + followCam.springOffset.z * followCam.springOffset.z) / followCam.cameraDistance));
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
        moddableForwardMoveSpeed = forwardMoveSpeed;
        moddableRotateSpeed = rotateSpeed;

        anim.Play("Take 001 0", 0, 0f);
        print("running");

        forwardAxisMovement(dashDirectionY * dashSpeed);
        sidewaysAxisMovement(dashDirectionX * dashSpeed);

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
            anim.Play("oneHandRun", 0, 0f);
            gameObject.GetComponentInChildren<Transform>().gameObject.GetComponentInChildren<Weapon>().attackActive = false;
            print("running");
            dashTimeStart = Time.time;
        }
        
    }

    public void Jump() {

        RaycastHit hit;
        
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), new Vector3(0,-1,0), out hit, 0.1f)) {
            isAirBorne = false;
        } else {
            isAirBorne = true;
        }

        if (!isAirBorne && Time.time > jumpCoolDown + 1) {
            rb.AddForce(new Vector3(0,jumpForce,0));
            jumpCoolDown = Time.time;
        }
    }

    public void playRun() {
        //if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 0") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 2") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 3") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 4") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 5")) {
        //    anim.Play("Take 001 0", 0, 0f);
        anim.SetBool("running", true);
        //}
        print("running");
    }

    public void playIdle() {
        //if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 2") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 3") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 4") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 5")) {
            //anim.Play("Take 001", 0, 0f);
            anim.SetBool("running", false);
        //}
        print("Idle");
    }

    public void playerAttack() {
        if (Time.time > attackAgainDelay + 0.2f) {

            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 2") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 3") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 4") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 5")) {
                anim.Play("Take 001 1", 0, 0f);
            } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 1") || anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 2") || anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 3") || anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 4") || anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 5")) {
                anim.SetBool("nextAttack", true);
            }

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 3")) {
                this.gameObject.GetComponent<playerManager>().weaponPosition.GetComponentInChildren<Weapon>().knockbackModdable = this.gameObject.GetComponent<playerManager>().weaponPosition.GetComponentInChildren<Weapon>().knockback * 2.5f;
            } else {
                this.gameObject.GetComponent<playerManager>().weaponPosition.GetComponentInChildren<Weapon>().knockbackModdable = this.gameObject.GetComponent<playerManager>().weaponPosition.GetComponentInChildren<Weapon>().knockback;
            }

            print("Attacking");
            attackAgainDelay = Time.time;
        }
    }

    void nextAttackReset() {
        
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 1")) {
            animationCurrentFrame = "attack 1";
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 5")) {
            animationCurrentFrame = "attack 5";
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 4")) {
            animationCurrentFrame = "attack 4";
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 2")) {
            animationCurrentFrame = "attack 2";
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Take 001 3")) {
            animationCurrentFrame = "attack 3";
        } else {
            animationCurrentFrame = "notAttack";
        }


        if (animationLastFrame != animationCurrentFrame) {
            anim.SetBool("nextAttack", false);
            setRotation();
        }

        animationLastFrame = animationCurrentFrame;
    }
}
