using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour //有 MonoBehaviour → 要挂,纯 static class → 不用挂
{
    public static LevelManager Instance;
    public PlayerController player;

    [Header("UI")]
    public GameObject levelCompleteUI;
    public GameObject gameOverUI;

    private void Awake()
    {
        // 单例模式
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // 关卡完成
    public void LevelComplete()
    {
        Debug.Log("Level Complete!");

        levelCompleteUI.SetActive(true);

        Time.timeScale = 0f;
        player.enabled = false;
    }

    // 游戏失败
    public void GameOver()
    {
        Debug.Log("Game Over");

        gameOverUI.SetActive(true);

        Time.timeScale = 0f;
        player.enabled = false;
    }

    // 重新开始
    public void RestartLevel()
    {
        Time.timeScale = 1f;

        string currentScene = SceneManager.GetActiveScene().name;

        LoadingData.nextScene = currentScene;   // ⭐ 关键
        SceneManager.LoadScene("LoadingScene");  // ⭐ 统一走Loading
    }

    // 下一关
    public void NextLevel()
    {
        Time.timeScale = 1f;

        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        if (nextIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("已经是最后一关");
            return;
        }

        // ⭐ 正确获取方式
        string path = SceneUtility.GetScenePathByBuildIndex(nextIndex);
        string nextSceneName = System.IO.Path.GetFileNameWithoutExtension(path);

        Debug.Log("下一关路径：" + path);
        Debug.Log("下一关名字：" + nextSceneName);

        if (string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogError("场景名为空！检查 Build Settings！");
            return;
        }

        LoadingData.nextScene = nextSceneName;
        SceneManager.LoadScene("LoadingScene");
    }
}
