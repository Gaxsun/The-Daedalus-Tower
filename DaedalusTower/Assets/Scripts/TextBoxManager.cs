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

    // Use this for initialization
    void Start()
    {

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

        if(Input.GetButtonUp("A"))
        {
            currentLine += 1;
        }

        if(currentLine > endAtLine)
        {
            textBox.SetActive(false);
        }
    }

}

