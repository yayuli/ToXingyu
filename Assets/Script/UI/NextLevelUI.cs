using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextLevelUI : MonoBehaviour
{
    public CanvasGroup fadePanel;
    public float fadeDuration = 2f;  // 设置淡入淡出持续时间为2秒

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn()
    {
        Time.timeScale = 0;  // 游戏开始时暂停
        float time = 0;
        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;  // 使用unscaledDeltaTime来确保计时器在游戏暂停时继续工作
            fadePanel.alpha = 1 - time / fadeDuration;
            yield return null;
        }
        fadePanel.gameObject.SetActive(false);
        Time.timeScale = 1;  // 淡入结束后恢复游戏
    }

    public IEnumerator FadeOut()
    {
        fadePanel.gameObject.SetActive(true);
        Time.timeScale = 0;  // 开始淡出时暂停游戏
        float time = 0;
        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;  // 使用unscaledDeltaTime来确保计时器在游戏暂停时继续工作
            fadePanel.alpha = time / fadeDuration;
            yield return null;
        }
        Time.timeScale = 1;  // 淡出结束后恢复游戏
    }
}
