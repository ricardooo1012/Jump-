using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingScene : MonoBehaviour
{
    // 拖入 UI 的进度条（Slider）
    public Slider progressBar;

    public TMP_Text levelText;

    public Image fadeImage;

    public float fadeSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        string nextSceneName = LoadingData.nextScene;

        // ⭐ 显示关卡名
        levelText.text = "Loading " + nextSceneName + "...";

        StartCoroutine(LoadingFlow());
    }


    IEnumerator LoadingFlow()
    {
        // 1️⃣ 先淡入（黑 → 透明）
        yield return StartCoroutine(FadeIn());

        // 2️⃣ 开始加载
        yield return StartCoroutine(LoadSceneAsync());
    }

    // 协程：可以一边执行一边等待（不会卡死游戏）
    IEnumerator LoadSceneAsync()
    {
        string nextSceneName = LoadingData.nextScene;

        // 1. 开始“后台加载场景”
        AsyncOperation op = SceneManager.LoadSceneAsync(nextSceneName);

        // 2. 先不允许直接进入场景（关键！）
        op.allowSceneActivation = false;

        // 3. 只要还没完全结束，就一直循环
        while (!op.isDone)
        {
            // 4. Unity 的进度最大只到 0.9
            // 所以要除以 0.9，让它变成 0~1（对应进度条）
            float progress = Mathf.Clamp01(op.progress / 0.9f);//Clamp01 = 安全保护，让数值永远在 0~1

            // 5. 更新 UI 进度条
            progressBar.value = progress;

            // 6. 当加载到 0.9（其实已经加载完成）
            if (op.progress >= 0.9f)
            {
                // 停一下，让玩家看到“100%”
                yield return new WaitForSeconds(1f);

                // ⭐ 先淡出（变黑）
                yield return StartCoroutine(FadeOut());

                // 7. 允许进入场景（真正切换）
                op.allowSceneActivation = true;
            }

            // 等待下一帧继续（避免卡死）
            yield return null;
        }
    }
    IEnumerator FadeIn()
    {
        Color c = fadeImage.color;

        while (c.a > 0)
        {
            c.a -= Time.deltaTime * fadeSpeed;
            fadeImage.color = c;
            yield return null;
        }

        c.a = 0;
        fadeImage.color = c;
    }

    IEnumerator FadeOut()
    {
        Color c = fadeImage.color;

        while (c.a < 1)
        {
            c.a += Time.deltaTime * fadeSpeed;
            fadeImage.color = c;
            yield return null;
        }

        c.a = 1;
        fadeImage.color = c;
    }

}
