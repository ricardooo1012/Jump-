using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;       //让任何脚本都能找到 UIManager
    public TextMeshProUGUI scoreText;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        AddScore(0);
    }

    void Update()
    {
        
    }

    public void AddScore(int score)
    {
        scoreText.text = "SCORE: " + score.ToString("00000");
    }
}
