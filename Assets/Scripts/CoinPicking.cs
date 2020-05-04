using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CoinPicking : MonoBehaviour
{

    public Canvas coinCanvas; //the canvas for the coin system
    public Text coinCounterText;
    public static int coinScore; //set up to get a coin counter from claire controller script
    
    void Start()
    {
        coinCanvas.enabled = false; // turn off the coin box at the beginnig

    }

    // Update is called once per frame
    void Update()
    {
        coinScore = ClaireController.coinCounter; // get coin counter from the claire script
        string coinScoreString = coinScore.ToString();
        coinCounterText.text = coinScoreString;
        

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Claire")
        {
            TurnOnCoinMessage();
        }
    }
    
    private void TurnOnCoinMessage()
    {
        coinCanvas.enabled = true;//function for enable canvas
        
    }
    
    
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Claire")
        {
            TurnOffCoinMessage(); // turn off canvas when Claire enters the collider
            Destroy(gameObject);
        }
    }
    
    
    private void TurnOffCoinMessage()
    {
        coinCanvas.enabled = false; //function for disable the canvas
    }
}
