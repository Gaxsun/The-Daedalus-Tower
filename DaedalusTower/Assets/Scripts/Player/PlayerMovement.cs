using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public Camera playerCam;

    public Animator anim;

    public CapsuleCollider capsule;

    public float forwardMoveSpeed;
    //public float backwardMoveSpeed;
    public float sidewaysMoveSpeed;

    public float rotateSpeed;

    // 0-1 scaled to player height based off capsule
    public float slopeCheckHeight;

    // how far to extrude from player
    public float slopeCheckDistance;

    // Use this for initialization
    void Start () {
        anim = GetComponentInChildren<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        setRotation();
        slopeTooSteep();
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("oneHandAttack")) {
            gameObject.GetComponentInChildren<Transform>().gameObject.GetComponentInChildren<Weapon>().attackActive = false;
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

    // true if too steep
    public bool slopeTooSteep() {
        RaycastHit hit;
        if (Physics.BoxCast(new Vector3(transform.position.x, transform.position.y - (capsule.height / 2) + (capsule.height * slopeCheckHeight), transform.position.z), new Vector3(capsule.radius * 2, 0.1f, 1f), transform.forward, out hit, transform.rotation, capsule.radius * 2 * slopeCheckDistance)) {
            if (hit.collider.tag == "terrain") {
                print("Too Steep");
            }
        }        
        return false;
    }

    private void setRotation() {
        Vector2 joystick = new Vector2(Input.GetAxis("LeftStickX"), Input.GetAxis("LeftStickY"));
        if (joystick.x != 0 || joystick.y != 0) {
            float angle = Mathf.Atan2(joystick.x, -joystick.y) * Mathf.Rad2Deg + playerCam.transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0,angle,0);
        }
        
        //(transform.forward - transform.position) * new Vector3(Input.GetAxis("Horizontal"), 0, -Input.GetAxis("Vertical"));
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
        print("idle");
    }

    public void playerAttack() {
        gameObject.GetComponentInChildren<Transform>().gameObject.GetComponentInChildren<Weapon>().attackActive = true;
        anim.Play("oneHandAttack", 0, 0f);
    }
}
