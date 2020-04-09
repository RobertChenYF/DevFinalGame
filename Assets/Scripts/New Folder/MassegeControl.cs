using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MassegeControl : MonoBehaviour
{
    
    public Canvas messageCanvas;

    public Text massage1;

    public TextAsset textFile1;
    public string[] textLines;

    public int currentLine;
    public int endLineAt;
    

    void Start()
    {
        messageCanvas.enabled = false;
        textLines = (textFile1.text.Split('\n'));
        if (endLineAt == 0)
        {
            endLineAt = textLines.Length - 1;
        }

    }


     void Update()
     {
         massage1.text = textLines[currentLine];

         if (Input.GetKeyDown(KeyCode.Space))
         {
             currentLine += 1;
         }

         if (currentLine > endLineAt)
         {
             messageCanvas.enabled = false;
             currentLine = 0;
         }
         
     }

        
        
        
     void OnTriggerEnter(Collider other)
     {
         if (other.gameObject.tag == "Claire")
         {
                TurnOnMessage();
                Debug.Log("lllll");
         }
     }
 
    private void TurnOnMessage()
    {
        messageCanvas.enabled = true;
    }
         
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Claire")
        {
            TurnOffMessage();
        }
    }
 
    private void TurnOffMessage()
    {
        messageCanvas.enabled = false;
    }

}
