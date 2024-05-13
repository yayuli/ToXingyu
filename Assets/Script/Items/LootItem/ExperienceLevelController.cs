using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceLevelController : MonoBehaviour
{
    public static ExperienceLevelController instance;

    public int currentExperience = 0;
    public int currentLevel = 1; 
    public int levelCount=100;
    public List<int> expLevels;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        while (expLevels.Count<levelCount)
        {
            expLevels.Add(Mathf.CeilToInt(expLevels[expLevels.Count - 1] * 1.1f));
        }
    }
    public void AddExperience(int amount)
    {
        currentExperience += amount;
        if (currentExperience >= expLevels[currentLevel])
        {
            LevelUp();
        }

        UIExp.instance.UpdateExperience(currentExperience, expLevels[currentLevel], currentLevel); ;
    }

    private void LevelUp()
    {
        currentExperience-= expLevels[currentLevel];

        currentLevel++;
        if (currentLevel>= expLevels.Count)
        {
            currentLevel = expLevels.Count-1;
        }
    }

}
