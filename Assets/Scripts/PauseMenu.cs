using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject coinPanel;
    public Text coinCount;
    //public int coinNum = 4;
    
    void Update()
    {
        if (Input.GetButtonDown("Menu"))
        {
            if (gameIsPaused)
            {
                Continue();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Continue()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }
    
    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
        ShowCoins();
        
    }

    public void QuitTheGame()
    {
        Debug.Log("quited");
        Application.Quit();
    }
    
    public void ShowCoins()
    {
        coinCount.text = "Coin: " + CoinPicking.coinScore;
        coinPanel.SetActive(true);
    }

    
    
    





}
