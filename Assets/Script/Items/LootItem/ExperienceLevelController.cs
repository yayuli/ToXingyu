using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceLevelController : MonoBehaviour
{
    public static ExperienceLevelController instance;

    [SerializeField] private int experiencePerLevel = 20;//each level requires experience
    public int currentExperience = 0;
    public int currentLevel = 1;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void AddExperience(int amount)
    {
        currentExperience += amount;
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {

        while (currentExperience >= experiencePerLevel * currentLevel)
        {
            currentExperience -= experiencePerLevel * currentLevel;
            currentLevel++;
            Debug.Log("Level up! Current level: " + currentLevel);
        }
    }

}
