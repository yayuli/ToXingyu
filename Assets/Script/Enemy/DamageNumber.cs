using System.Collections;
using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    public TMP_Text damageText;

    public float lifetime = 1.5f; // 持续时间，可以根据需要调整
    public float floatSpeed = 0.5f; // 上浮速度
    public Vector3 scaleRate = new Vector3(0.05f, 0.05f, 0.05f); // 每帧缩小的比率
    public float fadeSpeed = 1f; // 淡出速度

    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        StartCoroutine(FadeOut());
    }

    void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;
    }

    public void Setup(int damageDisplay)
    {
        damageText.text = damageDisplay.ToString();
    }

    IEnumerator FadeOut()
    {
        float startLifetime = lifetime;

        while (lifetime > 0)
        {
            lifetime -= Time.deltaTime;
            transform.localScale -= scaleRate * Time.deltaTime; // 逐渐缩小
            canvasGroup.alpha = lifetime / startLifetime; // 逐渐淡出
            yield return null;
        }

        Destroy(gameObject);
    }
}
