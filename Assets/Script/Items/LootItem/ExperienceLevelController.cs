using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceLevelController : MonoBehaviour
{
    public static ExperienceLevelController instance;

    public int currentExperience = 0;
    public int currentLevel = 1;
    public int levelCount = 100;
    public List<int> expLevels = new List<int> { 100 }; // 初始化第一个经验等级

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
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
        currentExperience += amount;
        // 使用循环处理可能多次升级的情况
        while (currentExperience >= expLevels[currentLevel])
        {
            LevelUp();
        }
        UIManager.instance.UpdateExperience(currentExperience, expLevels[currentLevel], currentLevel);
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
        // 暂停所有游戏活动
        Time.timeScale = 0f;
    }

    // 恢复时间方法
    public void ResumeGame()
    {
        UIManager.instance.levelUpPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
