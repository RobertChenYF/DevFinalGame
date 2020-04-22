using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MassegeControl : MonoBehaviour
{
    
    public Canvas messageCanvas;// The canvas for all massage text
    public GameObject textBoxContainer;//Inside canvas, holds the text box, set a maximum size for the text box
    
    //public GameObject claire;
    //public GameObject NPC;
    public Text massage1;// the text object

    public TextAsset textFile1;//the content of the text
    public string[] textLines;

    public int currentLine;//the line counter
    public int endLineAt;//the number for the last line

    private float typeWriterDelay = 0.02f;
    public string fullText;
    public string currentText;
    

    void Start()
    {
        messageCanvas.enabled = false;//turn off canvas at the beginning
        textLines = (textFile1.text.Split('\n'));//splitting the text file in to lines

        if (endLineAt == 0)
        {
            endLineAt = textLines.Length - 1; // set the number for the last line by how many lines the file has
        }
        
        
    }


     void Update()
     {
         
         if( messageCanvas.enabled == true)
         {
             Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
             textBoxContainer.transform.position = screenPos;

             //massage1.text = textLines[currentLine];

             if (Input.GetKeyDown(KeyCode.Space))
             {
                 StartCoroutine(TypeWriter());
                 currentLine += 1;
                 fullText = textLines[currentLine];
             }

             if (currentLine > endLineAt - 1)
             {
                 messageCanvas.enabled = false;
                 currentLine = 0;
             }
         }
         

     }
     
        
     void OnTriggerEnter(Collider other)
     {
         if (other.gameObject.tag == "Claire")
         {
                TurnOnMessage();  // turn on canvas when Claire enters the collider
                
                Debug.Log("entered");
                StartCoroutine(TypeWriter());
                currentLine = 1;
         }
     }
 
    private void TurnOnMessage()
    {
        messageCanvas.enabled = true;//function for enable canvas
    }
         
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Claire")
        {
            TurnOffMessage(); // turn off canvas when Claire enters the collider
            currentLine = 0; // reset line counter
        }
    }
 
    private void TurnOffMessage()
    {
        messageCanvas.enabled = false; //function for disable the canvas
    }

    private IEnumerator TypeWriter()
    {
        for (int i = 0; i < fullText.Length; i ++ )
        {
            currentText = fullText.Substring(0,i);
            massage1.text = currentText;
            yield return new WaitForSeconds(typeWriterDelay);
        }
    }

}
