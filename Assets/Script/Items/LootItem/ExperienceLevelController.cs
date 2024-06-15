using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceLevelController : MonoBehaviour
{
    public static ExperienceLevelController instance;

    [Header("Experience Settings")]
    public int currentExperience = 0;
    public int totalExperience = 0;
    public int currentLevel = 1;
    public int levelCount = 100;
    public List<int> expLevels = new List<int> { 100 }; // 初始化第一个经验等级


    [Header("Refresh cost settings")]
    public int baseRefreshCost = 10;
    [SerializeField] private int refreshIncrement= 10;
//    [SerializeField] private int levelCostIncrement = 6;
    [SerializeField] private int refreshCount = 0;//记录


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

    private void Start()
    {
        // 初始化经验等级列表
        while (expLevels.Count < levelCount)
        {
            expLevels.Add(Mathf.CeilToInt(expLevels[expLevels.Count - 1] * 1.1f));
        }
    }

    public void AddExperience(int amount)
    {
        totalExperience += amount;
        currentExperience += amount;
        // 使用循环处理可能多次升级的情况
        while (currentExperience >= expLevels[currentLevel])
        {
            LevelUp();
        }
        // 更新UI，传递总经验值
        UIManager.instance.UpdateExperience(totalExperience, currentExperience, expLevels[currentLevel], currentLevel);
    }

    public bool CanAfford(int cost)
    {
        bool canAfford = totalExperience >= cost;
        Debug.Log($"Checking affordability: Total Experience: {totalExperience}, Cost: {cost}, Can Afford: {canAfford}");
        return canAfford;
    }

    public void SpendExperience(int cost)
    {
        if (CanAfford(cost))
        {
            totalExperience -= cost;
            UIManager.instance.UpdateExperience(totalExperience, currentExperience, expLevels[currentLevel], currentLevel);
            Debug.Log($"Spent {cost} experience. Remaining Total Experience: {totalExperience}");
        }
        else
        {
            Debug.LogError("Not enough experience points to make this purchase.");
        }
    }

    public int CalculateRefreshCost()
    {
        return baseRefreshCost * (refreshIncrement + refreshCount);
    }

    public void IncrementRefreshCount()
    {
        refreshCount++;
    }

    private void LevelUp()
    {
        currentExperience -= expLevels[currentLevel];
        currentLevel++;
        if (currentLevel >= expLevels.Count)
        {
            currentLevel = expLevels.Count - 1;
        }

        // 升级UI界面
        UIManager.instance.levelUpPanel.SetActive(true);
        SFXManager.instance.PlaySFXPitched(3);

        // 暂停所有游戏活动
        Time.timeScale = 0f;;

        // 确保每次都播放粒子效果
        ExpUpEffect.Stop();
        ExpUpEffect.Play();

    }

    // 恢复时间方法
    public void ResumeGame()
    {
        UIManager.instance.levelUpPanel.SetActive(false);
       
        Time.timeScale = 1f;
       
    }
}
