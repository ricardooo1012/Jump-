using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//处理button
using TMPro;

public class UIGameManager : MonoBehaviour
{
    public PlayerController player;
    public Button playButton;
    public Button pauseButton;

    void Start()
    {
        playButton.onClick.AddListener(TogglePlay);
        pauseButton.onClick.AddListener(TogglePause);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TogglePlay()
    {
        Time.timeScale =1f;
        player.enabled = true;
    }

    public void TogglePause()
    {
        Time.timeScale = 0f;
        player.enabled = false;
    }
}
