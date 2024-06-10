using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NextLevelUI : MonoBehaviour
{
    public CanvasGroup fadePanel;
    public float fadeDuration = 4f;  // 设置淡入淡出持续时间为4秒

    public GameObject loadingScreenObj;
    public Slider progressBar;
    public TMP_Text loadingText;

    void Start()
    {
        loadingScreenObj.SetActive(false);  // 确保开始时加载屏幕不可见
       // SFXManager.instance.PlaySFX(2);

    }

    public IEnumerator FadeIn()
    {
        Time.timeScale = 0;  // 游戏开始时暂停
        float time = 0;
        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;
            fadePanel.alpha = 1 - time / fadeDuration;
            yield return null;
            //SFXManager.instance.PlaySFX(2);
        }
        fadePanel.gameObject.SetActive(false);
        loadingScreenObj.SetActive(false);  // 确保加载屏幕隐藏
        Time.timeScale = 1;  // 淡入结束后恢复游戏
    }

    public IEnumerator FadeOut()
    {
        loadingScreenObj.SetActive(true);  // 在淡出前显示加载屏幕
        fadePanel.gameObject.SetActive(true);
        Time.timeScale = 0;  // 开始淡出时暂停游戏
        float time = 0;
        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;
            fadePanel.alpha = time / fadeDuration;
            yield return null;
        }
        Time.timeScale = 1;  // 淡出结束后恢复游戏
    }

    public void SetProgress(float progress)
    {
        progressBar.value = progress;
        loadingText.text = "Loading... " + (int)(progress * 100) + "%";
    }
}
