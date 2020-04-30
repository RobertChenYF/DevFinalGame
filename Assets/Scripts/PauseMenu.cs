using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMuneUI;
    public GameObject coinPanel;
    public Text coinCount;
    public int coinNum = 4;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
        pauseMuneUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }
    
    private void Pause()
    {
        pauseMuneUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
        
    }

    public void QuitTheGame()
    {
        Debug.Log("quited");
        Application.Quit();
    }
    
    public void ShowCoins()
    {
        coinCount.text = "Coin: " + coinNum;
        coinPanel.SetActive(true);
    }

    
    
    





}
