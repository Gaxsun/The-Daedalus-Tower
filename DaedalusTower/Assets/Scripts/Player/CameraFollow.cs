using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public GameObject player;
    public float cameraDistance;

    public float minCameraDistance;
    public float cameraPadding;
    
    public Vector3 initOffset;
    public Vector3 springOffset;
    public Vector3 rotateOffset;

    public float rotateSpeed;

    public float cameraDirection;
    public Vector3 cameraTarget;
    private float cameraYTarget;
    public float wallOffset;
    public float modelYOffset;

    public bool bossFight;
    public float bossCamY;

    CursorLockMode wantedMode;

    public Vector3 playerLocation;

	// Use this for initialization
	void Start () {
        initOffset.z = -cameraDistance;
        transform.position = player.transform.position + initOffset;
        rotateOffset = initOffset;
        springOffset = initOffset;
        cameraYTarget = cameraTarget.y;
        cameraTarget = player.transform.position + springOffset * -(1 + 10 * Mathf.Asin((1 - Mathf.Sqrt(springOffset.x * springOffset.x + springOffset.z * springOffset.z) / cameraDistance))) / Mathf.PI;
        cameraTarget.y = cameraYTarget;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerLocation = new Vector3(player.transform.position.x, player.transform.position.y + modelYOffset, player.transform.position.z);
        }
	
	// Update is called once per frame
	void Update () {
        springArm();
        if(Mathf.Sqrt(springOffset.x * springOffset.x + springOffset.z * springOffset.z) > cameraDistance) {
            cameraDistanceReset();
        }
        transform.position = player.transform.position + springOffset;
        cameraYTarget = cameraTarget.y;
        cameraTarget = player.transform.position + (springOffset * -(1 + 10 * Mathf.Asin((1 - Mathf.Sqrt(springOffset.x * springOffset.x + springOffset.z * springOffset.z) / cameraDistance))) / Mathf.PI).normalized * cameraDistance;
        cameraTarget.y = cameraYTarget;
        transform.LookAt(new Vector3(cameraTarget.x, playerLocation.y + cameraTarget.y, cameraTarget.z));
        Debug.DrawRay(transform.position, new Vector3(cameraTarget.x, playerLocation.y + cameraTarget.y, cameraTarget.z) - transform.position, Color.blue);
        if (bossFight) {
            cameraTarget.y = bossCamY;
            float distancePoint;

            distancePoint = new Vector2(springOffset.x, springOffset.z).magnitude;
            distancePoint /= cameraDistance;
            cameraTarget *= distancePoint;
        }
        playerLocation = new Vector3(player.transform.position.x, player.transform.position.y + modelYOffset, player.transform.position.z);
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
        rotateOffset.x = transform.position.x - playerLocation.x;
        rotateOffset.z = transform.position.z - playerLocation.z;

        springArm();
        
        if (springOffset.y > initOffset.y) {
            springOffset.y = initOffset.y;
        } else if (springOffset.y < playerLocation.y) {
            springOffset.y = playerLocation.y;
        }
    }

    public void springArm() {
        RaycastHit hit;
        Vector2 distancePoint;
        distancePoint = new Vector2(rotateOffset.x, rotateOffset.z).normalized * cameraDistance;

        Debug.DrawRay(playerLocation, new Vector3(distancePoint.x, initOffset.y - modelYOffset, distancePoint.y), Color.white);
        if (Physics.Raycast(playerLocation, new Vector3(distancePoint.x, initOffset.y - modelYOffset, distancePoint.y), out hit, springOffset.magnitude)) {
            Debug.DrawRay(playerLocation, hit.point - playerLocation, Color.green);

            if (hit.collider.tag == "terrain"|| hit.collider.tag == "destTerrain") {
                springOffset.x = hit.point.x - playerLocation.x;
                springOffset.z = hit.point.z - playerLocation.z;
                springOffset.y = (Mathf.Sqrt(springOffset.x * springOffset.x + springOffset.z * springOffset.z) / cameraDistance) * initOffset.y + modelYOffset * (1 - Mathf.Sqrt(springOffset.x * springOffset.x + springOffset.z * springOffset.z) / cameraDistance);
                
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
}
