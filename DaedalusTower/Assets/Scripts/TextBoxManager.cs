using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxManager : MonoBehaviour{

    public GameObject textBox;

    public Text theText;

    public TextAsset textFile;
    public string[] textLines;

    public int currentLine;
    public int endAtLine;

    public PlayerMovement player;

    private float secondCount;
    public float freezeDelay;


    // Use this for initialization
    void Start()
    {
        secondCount = Time.time;

        player = FindObjectOfType<PlayerMovement>();
        //GetComponent<PlayerInput>().controlsEnabled = false;

        if (textFile != null)
        {
            textLines = (textFile.text.Split('\n'));
        }

        if(endAtLine ==0)
        {
            endAtLine = textLines.Length - 1;
        }

    }

    private void Update()
    {
        theText.text = textLines[currentLine];
        if (Input.GetButtonUp("LeftStickClick") ) {
            currentLine = 8;
        }
        if (Input.GetButtonUp("RightStickClick") && currentLine == 0)
        {
            currentLine = 1;
        }
        else if (Input.GetButtonUp("X") && currentLine == 1)
        {
            currentLine = 2;
            secondCount = Time.time;
        }
        else if (Input.GetButtonUp("X") && currentLine == 2)
        {
            if (Time.time > secondCount + freezeDelay)
            {
                currentLine = 3;
            }
        }
        else if (Input.GetButtonUp("Y") && currentLine == 3)
        {
            currentLine = 4;
            secondCount = Time.time;
        }
        else if (Input.GetButtonUp("Y") && currentLine == 4)
        {
            if (Time.time > secondCount + freezeDelay)
            {
                currentLine = 5;
                secondCount = Time.time;
            }
        }
        else if ((Input.GetButtonUp("Y") || Input.GetButtonUp("X")) && currentLine == 5)
        {
            if (Time.time > secondCount + freezeDelay)
            {
                currentLine = 6;
            }
        }
        else if (Input.GetButtonUp("B") && currentLine == 6)
        {
            currentLine = 7;
        }
        else if (Input.GetButtonUp("A") && currentLine == 7)
        {
            currentLine +=1;
        }

        if (currentLine > endAtLine)
        {
            textBox.SetActive(false);
        }
    }

}

