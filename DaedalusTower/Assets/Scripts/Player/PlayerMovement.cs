using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public Camera playerCam;

    public Animator anim;

    public CapsuleCollider capsule;

    Rigidbody rb;

    public float forwardMoveSpeed;
    //public float backwardMoveSpeed;
    public float sidewaysMoveSpeed;

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

    bool dashing = false;
    

    // Game Time Started
    void Start () {
        anim = GetComponentInChildren<Animator>();
        rb = this.GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        setRotation();
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("oneHandAttack")) {
            gameObject.GetComponentInChildren<Transform>().gameObject.GetComponentInChildren<Weapon>().attackActive = false;
        } else {
            gameObject.GetComponentInChildren<Transform>().gameObject.GetComponentInChildren<Weapon>().attackActive = true;
        }

        if (Time.time >= dashCooldown + dashTimeStart) {
            dashEnabled = true;
        }

    }

    void FixedUpdate() {
        if (dashing) {
            Dash();
            dashEnabled = false;
        }
    }

    public void forwardAxisMovement(float direction) {
        transform.position = transform.position + new Vector3(playerCam.transform.forward.x, 0, playerCam.transform.forward.z).normalized * forwardMoveSpeed * -direction * Time.deltaTime;
        playerCam.transform.position = playerCam.transform.position + new Vector3(playerCam.transform.forward.x, 0, playerCam.transform.forward.z).normalized * forwardMoveSpeed * -direction * Time.deltaTime;
    }

    public void sidewaysAxisMovement(float direction) {
        transform.RotateAround(playerCam.transform.position, transform.up, rotateSpeed * direction * Time.deltaTime);
        playerCam.GetComponent<CameraFollow>().playerCounterRotate();
    }

    private void setRotation() {
        Vector2 joystick = new Vector2(Input.GetAxis("LeftStickX"), Input.GetAxis("LeftStickY"));
        if (joystick.x != 0 || joystick.y != 0) {
            float angle = Mathf.Atan2(joystick.x, -joystick.y) * Mathf.Rad2Deg + playerCam.transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0,angle,0);
        }
        
    }

    public void Dash() {

        if (Time.time >= dashTime + dashTimeStart || dashCheck()) {
            dashing = false;
            GetComponent<PlayerInput>().controlsEnabled = true;
            playIdle();
            dashTimeStart = Time.time;
        }

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
        if (Physics.Raycast(transform.position, new Vector3(0,-1,0), out hit, this.GetComponent<CapsuleCollider>().height / 1.1f)) {
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
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("oneHandRun") && !anim.GetCurrentAnimatorStateInfo(0).IsName("oneHandAttack")) {
            anim.Play("oneHandRun", 0, 0f);
        }
        print("running");
    }

    public void playIdle() {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("oneHandIdle") && !anim.GetCurrentAnimatorStateInfo(0).IsName("oneHandAttack")) {
            anim.Play("oneHandIdle", 0, 0f);
        }
        print("Idle");
    }

    public void playerAttack() {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("oneHandAttack")) {
            anim.Play("oneHandAttack", 0, 0f);
        }
        print("Attacking");
    }
}
