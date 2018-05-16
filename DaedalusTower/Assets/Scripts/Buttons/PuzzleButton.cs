using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuzzleButton : MonoBehaviour {

    private bool timerTick = true;
    private float timerValue;

    public float incrementDelay;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (Time.time >= timerValue + incrementDelay) {
            timerTick = true;
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.tag == "Player" & timerTick) {
            int outputValue = 9;
            TextMesh textPoint = GetComponentInChildren<TextMesh>();
            int.TryParse(textPoint.text, out outputValue);
            outputValue++;
            if (outputValue > 9) {
                outputValue = 0;
            }
            textPoint.text = outputValue.ToString();
            timerTick = false;
            timerValue = Time.time;
        }
        
}
}
