using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIExp : MonoBehaviour
{
    public static UIExp instance;

    public Slider expSlider;
    public TMP_Text expLevelText;
    //public TMPro.TextMeshPro expLevelText;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateExperience(int currentExp,int levelExp, int currentLvl)
    {
        expSlider.maxValue = levelExp;
        expSlider.value = currentExp;

        expLevelText.text = "Level: " + currentLvl;
    }
}
