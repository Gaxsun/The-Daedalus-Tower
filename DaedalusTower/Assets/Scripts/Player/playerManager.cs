using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerManager : MonoBehaviour {

    //Holds current player config
    public GameObject currentWeapon;

	// Use this for initialization
	void Start () {
        // Place selected weapon in player's hand
        Instantiate(currentWeapon, transform.GetChild(0));        
	}
	
	// Update is called once per frame
	void Update () {
        
    }
}
