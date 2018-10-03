using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CameraFollow : MonoBehaviour {

    public GameObject player;
    public float cameraDistance;
    public float cameraPadding;

    [Header("Player/Camera Local Offsets")]
    public Vector3 initOffset;
    public Vector3 springOffset;
    public Vector3 rotateOffset;

    public float rotateSpeed;

    public Vector3 cameraTarget;
    private float cameraYTarget;
    public float modelYOffset;

    public bool bossFight;
    public float bossCamY;
    public bool lockOn;
    public GameObject lockTarget;
    CursorLockMode wantedMode;

    public AudioClip[] ambientSounds;
    public AudioSource ambientSoundSource;

    public Vector3 playerLocation;
    private float currentCamDistance;

    public float cameraVertDelay;
    [HideInInspector]public float cameraVertTimer;
    public float cameraVertResetTime;
    
    [Header("Camera Vertical Min-Max")]
    public Vector2 cameraTargetMinMax = new Vector2(-1, 2);

    // Use this for initialization
    void Start () {
        initOffset.z = -cameraDistance;
        transform.position = player.transform.position + initOffset;
        rotateOffset = initOffset;
        springOffset = initOffset;
        cameraYTarget = cameraTarget.y;
        currentCamDistance = Mathf.Sqrt(springOffset.x * springOffset.x + springOffset.z * springOffset.z);
        cameraTarget = player.transform.position + springOffset * - (1 + 10 * Mathf.Asin(1 - currentCamDistance / cameraDistance) / Mathf.PI);
        cameraTarget.y = cameraYTarget;
        cameraVertTimer = Time.time;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerLocation = new Vector3(player.transform.position.x, player.transform.position.y + modelYOffset, player.transform.position.z);
        lockOn = false;

        ambientSoundSource.Stop();
        ambientSoundSource.clip = null;
        ambientSoundSource.clip = ambientSounds[0];
        ambientSoundSource.loop = true;
        ambientSoundSource.Play();
        
        }
	
	// Update is called once per frame
	void Update () {
        currentCamDistance = Mathf.Sqrt(springOffset.x * springOffset.x + springOffset.z * springOffset.z);
        targetLock();
        playerLocation = new Vector3(player.transform.position.x, player.transform.position.y + modelYOffset, player.transform.position.z);
        springArm();
        if (currentCamDistance >= cameraDistance) {
            cameraDistanceReset();
        }

        transform.position = player.transform.position + springOffset;
        Debug.DrawRay(transform.position, new Vector3(cameraTarget.x, playerLocation.y + cameraTarget.y, cameraTarget.z) - transform.position, Color.blue);

        if(Time.time > cameraVertTimer + cameraVertDelay) {
            if (bossFight) {
                cameraTarget.y = Mathf.Lerp(cameraTarget.y, bossCamY, Time.deltaTime * Mathf.Max(Mathf.Abs(cameraTarget.y), 1) / cameraVertResetTime);
                if (cameraTarget.y - bossCamY > -0.05f && cameraTarget.y - bossCamY < 0.05f) {
                    cameraVertTimer = Time.time;
                }
            } else {
                cameraTarget.y = Mathf.Lerp(cameraTarget.y, 0, Time.deltaTime * Mathf.Max(Mathf.Abs(cameraTarget.y), 1) / cameraVertResetTime);
                if (cameraTarget.y > -0.05f && cameraTarget.y < 0.05f) {
                    cameraVertTimer = Time.time;
                }
            }
        }
        if (bossFight) {
            cameraTarget.y = Mathf.Clamp(cameraTarget.y, cameraTargetMinMax.x, bossCamY + cameraTargetMinMax.y);
        } else {
            cameraTarget.y = Mathf.Clamp(cameraTarget.y, cameraTargetMinMax.x, cameraTargetMinMax.y);
        }
        transform.LookAt(new Vector3(cameraTarget.x, playerLocation.y + cameraTarget.y, cameraTarget.z));
    }

    public void cameraRotate(float rotateValueX) {
        transform.RotateAround(playerLocation, player.transform.up, rotateSpeed * Time.deltaTime * rotateValueX);
        rotateOffset.x = transform.position.x - playerLocation.x;
        rotateOffset.z = transform.position.z - playerLocation.z;

        if (rotateOffset.x > cameraDistance + cameraPadding) {
            rotateOffset.x = cameraDistance;
        } else if (rotateOffset.x < -cameraDistance - cameraPadding) {
            rotateOffset.x = -cameraDistance;
        }

        if (rotateOffset.z > cameraDistance + cameraPadding) {
            rotateOffset.z = cameraDistance;
        } else if (rotateOffset.z < -cameraDistance - cameraPadding) {
            rotateOffset.z = -cameraDistance;
        }

        springArm();

        if (springOffset.x > cameraDistance + cameraPadding) {
            springOffset.x = cameraDistance;
        } else if (springOffset.x < -cameraDistance - cameraPadding) {
            springOffset.x = -cameraDistance;
        }

        if (springOffset.z > cameraDistance + cameraPadding) {
            springOffset.z = cameraDistance;
        } else if (springOffset.z < -cameraDistance - cameraPadding) {
            springOffset.z = -cameraDistance;
        }
    }

    public void playerCounterRotate() {
        if (lockOn) {
            targetLock();
        }else { 
            rotateOffset.x = transform.position.x - player.transform.position.x;
            rotateOffset.z = transform.position.z - player.transform.position.z;
        }
    }

    public void springArm() {
        RaycastHit hit;
        Vector2 distancePoint;
        targetLock();
        distancePoint = new Vector2(rotateOffset.x, rotateOffset.z).normalized * cameraDistance;

        Debug.DrawRay(playerLocation, new Vector3(distancePoint.x, initOffset.y - modelYOffset, distancePoint.y), Color.white);
        if (Physics.Raycast(playerLocation, new Vector3(distancePoint.x, initOffset.y - modelYOffset, distancePoint.y), out hit, cameraDistance)) {
            Debug.DrawRay(playerLocation, hit.point - playerLocation, Color.green);

            if (hit.collider.tag == "terrain"|| hit.collider.tag == "destTerrain") {
                springOffset.x = hit.point.x - playerLocation.x;
                springOffset.z = hit.point.z - playerLocation.z;
                springOffset.y = currentCamDistance / cameraDistance * initOffset.y + modelYOffset * (1 - currentCamDistance / cameraDistance);
                
            } else {
                springOffset.x = rotateOffset.x;
                springOffset.z = rotateOffset.z;
                springOffset.y = initOffset.y;
            }
        } else {
            cameraDistanceReset();
            springOffset.x = initOffset.x;
            springOffset.z = initOffset.z;
            springOffset.y = initOffset.y;
            rotateOffset.x = initOffset.x;
            rotateOffset.z = initOffset.z;
        }
        if (springOffset.y > initOffset.y) {
            springOffset.y = initOffset.y;
        }
    }

    private void cameraDistanceReset() {
        Vector2 distancePoint;
        
        distancePoint = new Vector2(rotateOffset.x, rotateOffset.z).normalized * cameraDistance;

        initOffset = new Vector3(distancePoint.x, initOffset.y, distancePoint.y);

        if (initOffset.x > cameraDistance) {
            initOffset.x = cameraDistance;
        } else if (initOffset.x < -cameraDistance) {
            initOffset.x = -cameraDistance;
        }

        if (initOffset.z > cameraDistance) {
            initOffset.z = cameraDistance;
        } else if (initOffset.z < -cameraDistance) {
            initOffset.z = -cameraDistance;
        }

        springOffset = initOffset;
        rotateOffset = initOffset;
    }

    private void targetLock() {
        if (lockOn) {            
            if (lockTarget == null) {
                lockOn = false;
            } else {
                if (lockTarget.GetComponent<Enemy>() && lockTarget.GetComponent<Enemy>().health <= 0) {
                    lockOn = false;
                } else if (lockTarget.GetComponent<mistwalker>() && lockTarget.GetComponent<mistwalker>().health <= 0) {
                    lockOn = false;
                } else if (lockTarget.GetComponent<mistwalker>() && lockTarget.GetComponentInParent<NavMeshAgent>().baseOffset >= 50){
                    lockOn = false;
                } else {
                    cameraTarget = lockTarget.transform.position;
                    if (bossFight && lockOn) {
                        cameraTarget.y = bossCamY;
                        /*
                        float distancePoint;
                        distancePoint = new Vector2(springOffset.x, springOffset.z).magnitude;
                        distancePoint /= cameraDistance;
                        cameraTarget *= distancePoint;
                        */
                    } else {
                        cameraTarget.y = 0;
                    }
                    Vector3 lockRotate = (lockTarget.transform.position - player.transform.position).normalized * -cameraDistance;
                    rotateOffset = new Vector3(lockRotate.x, rotateOffset.y, lockRotate.z);
                }
            }
        }
        cameraYTarget = cameraTarget.y;
        cameraTarget = player.transform.position + (springOffset * -(1 + 10 * Mathf.Asin(1 - currentCamDistance / cameraDistance) / Mathf.PI)).normalized * cameraDistance;
        cameraTarget.y = cameraYTarget;

        if (bossFight) {
            if (ambientSoundSource.clip == ambientSounds[0]) {
                ambientSoundSource.Stop();
                ambientSoundSource.clip = ambientSounds[1];
                ambientSoundSource.loop = true;
                ambientSoundSource.Play();
            }
        }
    }
}
