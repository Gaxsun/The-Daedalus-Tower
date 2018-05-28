using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
    public GameObject mainCam;

    public float projectileSpeed;
    public int damage;
    public float fadeRate;

    private bool hit;
    // Use this for initialization
    void Start() {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        transform.LookAt(mainCam.GetComponent<CameraFollow>().playerLocation);
        transform.parent = null;
        hit = false;
    }

    // Update is called once per frame
    void Update() {
        transform.position += transform.forward * projectileSpeed * Time.deltaTime;
        if (hit) {
            Renderer shotRender = GetComponentInChildren<Renderer>();
            Color fadeShot = new Color(shotRender.material.GetColor("_TintColor").r - fadeRate*Time.deltaTime, shotRender.material.GetColor("_TintColor").g - fadeRate * Time.deltaTime, shotRender.material.GetColor("_TintColor").b - fadeRate * Time.deltaTime);
            shotRender.material.SetColor("_TintColor", fadeShot);
            if(shotRender.material.GetColor("_TintColor").r <= 0.05) {
                if (transform.parent != null) {
                    if (transform.parent.gameObject.tag != "Player" && transform.parent.gameObject.tag != "enemy") {
                        Destroy(transform.parent.gameObject);
                    }
                }
                Destroy(transform.GetChild(0).gameObject);
                Destroy(this.gameObject);
            }
        }
    }



    private void OnTriggerEnter(Collider other) {
        if (other.isTrigger == false) {
            //GameObject empty = new GameObject();
            projectileSpeed = 0;
            if (other.tag == "Player") {
                other.gameObject.GetComponent<playerManager>().takeDamage(damage);
                transform.SetParent(other.transform);
            }else if(other.tag == "enemy") {
                transform.SetParent(other.gameObject.transform);
            }
            GetComponent<BoxCollider>().enabled = false;
            hit = true;
        }
    }
}
