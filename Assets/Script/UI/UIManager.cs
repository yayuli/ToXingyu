using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public Slider expSlider;
    public TMP_Text expLevelText;
    public TMP_Text currentExpText;

    public GameObject levelUpPanel;

    [Header("Level Up Effects")]
    public ParticleSystem ExpUpEffect;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 使对象跨场景持续存在
        }
        else
        {
            Destroy(gameObject); // 确保不创建重复实例
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateExperience(int totalExp, int currentExp, int levelExp, int currentLvl)
    {
        expSlider.maxValue = levelExp;
        expSlider.value = currentExp;

        expLevelText.text = "Level: " + currentLvl;
        currentExpText.text = "Total Exp: " + totalExp;  // 显示总经验值
    }

    public void ShowLevelUpEffect()
    {
        ExpUpEffect.Play();

    }
   
}
