using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Zehua and Greg worked on this on Apr 22.
// still needs disable jump when reading text.

public class MassegeControl : MonoBehaviour
{
    RectTransform Canvas;
    RectTransform textBoxUI;

    public Canvas messageCanvas;// The canvas for all massage text
    public GameObject textBoxContainer;//Inside canvas, holds the text box, set a maximum size for the text box
    
    //public GameObject claire;
    //public GameObject NPC;
    public Text massage1;// the text object

    public TextAsset textFile1;
    public TextAsset goldenFeatherText;//the content of the text
    public string[] textLines;

    public int currentLine;//the line counter
    public int endLineAt;//the number for the last line

    private float typeWriterDelay = 0.02f;
    public string fullText;
    public string currentText;

    public bool insideCollider;
    public bool thisIsFeatherTrader;
    private bool featherIsCreated;

    public Transform featherSpawnlocation;
    public GameObject feather;
    
    


    //public int coinCounts;
    

    void Start()
    {
        textBoxUI = textBoxContainer.GetComponent<RectTransform>();
        Canvas = messageCanvas.GetComponent<RectTransform>();
        messageCanvas.enabled = false;//turn off canvas at the beginning
        
        if (this.tag == "FeatherTrader")
        {
            thisIsFeatherTrader = true;
        }
        else
        {
            thisIsFeatherTrader = false;
        }
        
        textLines = (textFile1.text.Split('\n'));//splitting the text file in to lines
        if (endLineAt == 0)
        {
            endLineAt = textLines.Length - 1; // set the number for the last line by how many lines the file has
        }
        
    }


    void Update()
    {
        Debug.Log(ClaireController.coinCounter);
        if (thisIsFeatherTrader == true && ClaireController.coinCounter >= 50)
        {
            textLines = (goldenFeatherText.text.Split('\n'));//splitting the text file in to lines
            endLineAt = textLines.Length - 1; // set the number for the last line by how many lines the file has
            if (currentLine == endLineAt + 1)
            {
                Instantiate(feather,featherSpawnlocation.position,Quaternion.identity);
                    ClaireController.coinCounter -= 20;
                    //featherIsCreated = true;
                    //BuyFeather();
            }
        }

        if (messageCanvas.enabled == true)
        {
            if (insideCollider)
            {
                // Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
                //  textBoxContainer.transform.position = screenPos;

                Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(transform.position);
                Vector2 WorldObject_ScreenPosition = new Vector2(
                ((ViewportPosition.x * Canvas.sizeDelta.x) - (Canvas.sizeDelta.x * 0.5f)),
                ((ViewportPosition.y * Canvas.sizeDelta.y) - (Canvas.sizeDelta.y * 0.5f))); //fix the UI problem code credit to unity forum

                //now you can set the position of the ui element
                textBoxUI.anchoredPosition = WorldObject_ScreenPosition;
            }

            //massage1.text = textLines[currentLine];


            if ((Input.GetButtonDown("Jump")) && insideCollider)
            {
                if (currentLine > endLineAt)
                {
                    ClaireController.me.cantMove = false;
                    messageCanvas.enabled = false;
                    currentLine = 0;
                    return;
                }

                ClaireController.me.cantMove = true;
                StopAllCoroutines();
                fullText = textLines[currentLine];
                StartCoroutine(TypeWriter());
                currentLine += 1;
            }
        }


    }


    void OnTriggerEnter(Collider other)
     {
         Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
         textBoxContainer.transform.position = screenPos;

         insideCollider = true;
         if (other.gameObject.tag == "Claire")
         {
                massage1.text = "!";
                TurnOnMessage();  // turn on canvas when Claire enters the collider
                currentLine = 0;
                fullText = textLines[currentLine];//set in the current text in to the type writer
                //Debug.Log("entered");
                //StartCoroutine(TypeWriter());
                
         }
     }
 
    private void TurnOnMessage()
    {
        //Debug.Log("Turning on massage canvas");
        messageCanvas.enabled = true;//function for enable canvas
        
    }
         
    void OnTriggerExit(Collider other)
    {
        insideCollider = false;
        if (other.gameObject.tag == "Claire")
        {
            TurnOffMessage(); // turn off canvas when Claire enters the collider
            currentLine = 0; // reset line counter
            //fullText = textLines[currentLine];//set in the current text in to the type writer
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

    void BuyFeather()
    {
        if(!featherIsCreated)
        {
            Instantiate(feather,featherSpawnlocation.position,Quaternion.identity);
            ClaireController.coinCounter -= 50;
            featherIsCreated = true;
        }
    }

//    private IEnumerator BuyFeather()
//    {
//        Instantiate(feather, transform.position , Quaternion.identity);
//        yield return new WaitForSeconds(.1f);
//    }

}
