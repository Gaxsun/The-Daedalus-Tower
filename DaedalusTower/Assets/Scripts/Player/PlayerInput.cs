using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    CursorLockMode wantedMode;

    public bool controlsEnabled = true;
    bool pausePrimed;
    float lockTimer;
    public float lockDelay;
    bool liveTarget;
    RaycastHit lockTarget;
    public GameObject lockCircle;
    GameObject currentCircle;
    GameObject targetEnemy;
    public Color targetFade;
    public Color targetActive;
    // Use this for initialization
    void Start () {
        //Lock Cursor
        wantedMode = CursorLockMode.Locked;
        lockTimer = 0;
    }
	
	// Update is called once per frame
	void Update () {

        Cursor.lockState = wantedMode;
        //Pause();      

        if (controlsEnabled) {
            inputs();
        }
        lockingOn();
    }

    private void inputs() {
        /*
         * 
         * 
         * 
         * Keyboard Input (obsolete)
         * 
         * 
         * 
         * 
         */

        if (Input.GetAxis("Vertical") != 0) {
            GetComponent<PlayerMovement>().forwardAxisMovement(Input.GetAxis("Vertical") * -1);
        }
        if (Input.GetAxis("Horizontal") != 0) {
            GetComponent<PlayerMovement>().sidewaysAxisMovement(Input.GetAxis("Horizontal"));
        }

        if (Input.GetAxis("Mouse X") != 0) {
            GetComponent<PlayerMovement>().playerCam.GetComponent<CameraFollow>().cameraRotate(Input.GetAxis("Mouse X"));
        }

        if (Input.GetMouseButtonDown(0)) {
            gameObject.GetComponentInChildren<Transform>().gameObject.GetComponentInChildren<Weapon>().attackActive = true;
        }


        /*
         * 
         * 
         * 
         * 
         * Controller Input
         * 
         * 
         * 
         */
        if (Input.GetAxis("LeftStickY") != 0 || Input.GetAxis("LeftStickX") != 0) {
            GetComponent<PlayerMovement>().playRun();
        } else {
            GetComponent<PlayerMovement>().playIdle();
        }

        GetComponent<PlayerMovement>().forwardAxisMovement(Input.GetAxis("LeftStickY"));
        GetComponent<PlayerMovement>().sidewaysAxisMovement(Input.GetAxis("LeftStickX"));

        if (Input.GetAxis("X") != 0) {
            GetComponent<PlayerMovement>().playerAttack();
            GetComponent<Animator>().SetInteger("nextAttack", 1);
        }

        if(Input.GetAxis("Y") != 0) {
            GetComponent<Animator>().SetInteger("nextAttack", 2);
        }

        if (Input.GetAxis("A") != 0) {
            GetComponent<PlayerMovement>().Jump();
        }

        if (Input.GetAxis("RightBumper") != 0) {
            if (GetComponent<PlayerMovement>().dashEnabled) {
                GetComponent<PlayerMovement>().DashStart();
            }           
        }

        if (Input.GetAxis("RightStickX") != 0 && GetComponent<PlayerMovement>().playerCam.GetComponent<CameraFollow>().lockOn == false) {
            GetComponent<PlayerMovement>().playerCam.GetComponent<CameraFollow>().cameraRotate(Input.GetAxis("RightStickX"));
        }

        if (Input.GetAxis("RightStickClick") != 0) {
        
            if (Time.time > (lockTimer + lockDelay)) {
                print(Time.time);
                print(lockTimer + lockDelay);
                lockTimer = Time.time;
                if (GetComponent<PlayerMovement>().playerCam.GetComponent<CameraFollow>().lockOn) {
                    GetComponent<PlayerMovement>().playerCam.GetComponent<CameraFollow>().lockOn = false;
                } else {
                    if (liveTarget && lockTarget.collider.gameObject.tag == "enemy") {
                        GetComponent<PlayerMovement>().playerCam.GetComponent<CameraFollow>().lockTarget = lockTarget.collider.gameObject;
                        GetComponent<PlayerMovement>().playerCam.GetComponent<CameraFollow>().lockOn = true;
                    } else if (liveTarget && lockTarget.collider.gameObject.tag == "mistwalker") {
                        GetComponent<PlayerMovement>().playerCam.GetComponent<CameraFollow>().lockTarget = lockTarget.collider.gameObject;
                        GetComponent<PlayerMovement>().playerCam.GetComponent<CameraFollow>().lockOn = true;
                    }
                }
            }
        }
    }

    private void lockingOn() {
        Debug.DrawRay(GetComponent<PlayerMovement>().playerCam.GetComponent<CameraFollow>().playerLocation, new Vector3(GetComponent<PlayerMovement>().playerCam.transform.forward.x, 0, GetComponent<PlayerMovement>().playerCam.transform.forward.z) * 30, Color.yellow, 1);
        if (Physics.BoxCast(GetComponent<PlayerMovement>().playerCam.GetComponent<CameraFollow>().playerLocation, transform.localScale / 4, new Vector3(GetComponent<PlayerMovement>().playerCam.transform.forward.x, 0, GetComponent<PlayerMovement>().playerCam.transform.forward.z),
                                out lockTarget, transform.rotation, 30) && lockTarget.collider.gameObject.tag == "enemy") {
            liveTarget = true;
            if (!currentCircle) {
                targetEnemy = lockTarget.collider.gameObject;
                currentCircle = Instantiate(lockCircle, targetEnemy.transform.position, Quaternion.identity);
            }
        } else if (Physics.BoxCast(GetComponent<PlayerMovement>().playerCam.GetComponent<CameraFollow>().playerLocation, transform.localScale / 4, new Vector3(GetComponent<PlayerMovement>().playerCam.transform.forward.x, 0, GetComponent<PlayerMovement>().playerCam.transform.forward.z),
                                    out lockTarget, transform.rotation, 30) && lockTarget.collider.gameObject.tag == "mistwalker") {
            liveTarget = true;
            if (!currentCircle) {
                targetEnemy = lockTarget.collider.gameObject;
                currentCircle = Instantiate(lockCircle, new Vector3(targetEnemy.transform.position.x, targetEnemy.transform.position.y+2, targetEnemy.transform.position.z), Quaternion.identity);
            }
        } else {
            liveTarget = false;
        }

        if (currentCircle) {
            if(lockTarget.collider.gameObject != targetEnemy && !GetComponent<PlayerMovement>().playerCam.GetComponent<CameraFollow>().lockOn) {
                targetEnemy = lockTarget.collider.gameObject;
            }
            currentCircle.transform.position = new Vector3(targetEnemy.transform.position.x, targetEnemy.transform.position.y + 2, targetEnemy.transform.position.z);
            currentCircle.transform.LookAt(GetComponent<PlayerMovement>().playerCam.transform);
        }
        if (!GetComponent<PlayerMovement>().playerCam.GetComponent<CameraFollow>().lockOn && !liveTarget) {
            Destroy(currentCircle);
        } else if (!GetComponent<PlayerMovement>().playerCam.GetComponent<CameraFollow>().lockOn) {
            currentCircle.GetComponent<TextMesh>().color = new Color(0.45f, .45f, .45f, .45f);
        } else {
            currentCircle.GetComponent<TextMesh>().color = new Color(1, 1, 1, 1);

        }
    }
    /*private void Pause() {
        if (Input.GetAxisRaw("StartButton") != 0) {
            pausePrimed = true; 
        }
        
        if (pausePrimed && Input.GetAxisRaw("StartButton") == 0) {
            if (Time.timeScale == 1) {
                Time.timeScale = 0F;
                GetComponent<playerManager>().inventoryWindow.enabled = true;
                controlsEnabled = false;
            } else {
                
                Time.timeScale = 1F;
                GetComponent<playerManager>().inventoryWindow.enabled = false;
                controlsEnabled = true;
            }
            pausePrimed = false;
        }
    }*/
}
