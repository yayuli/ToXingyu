using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private MazeGenerator mazeGenerator;

    public Slider expSlider;
    public TMP_Text expLevelText;
    public TMP_Text currentExpText;

    public GameObject levelUpPanel;
    public GameObject gameOverScreen;
    public GameObject pauseScreen;

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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
    }
    public void DestroyAllDontDestroyOnLoadObjects()
    {
        foreach (var go in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (go.hideFlags == HideFlags.None)
            {
                if (go.scene.buildIndex == -1)  // 检查是否属于 DontDestroyOnLoad 场景
                {
                    Destroy(go);
                }
            }
        }
    }


    public void UpdateExperience(int totalExp, int currentExp, int levelExp, int currentLvl)
    {
        expSlider.maxValue = levelExp;
        expSlider.value = currentExp;

        expLevelText.text = "Level: " + currentLvl;
        currentExpText.text = "Total Exp: " + totalExp;  // 显示总经验值
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main");
        pauseScreen.SetActive(false);
    }

    public void Restart()
    {
        // 首先销毁所有 DontDestroyOnLoad 对象
        DestroyAllDontDestroyOnLoadObjects();

        // 重新加载游戏场景
        SceneManager.LoadScene("GameScene");
        mazeGenerator.ResetMazeSizeToDefault();

        // 关闭暂停界面并恢复时间流逝
        pauseScreen.SetActive(false);
        Time.timeScale = 1f;
    }


    public void QiutGame()
    {
        Application.Quit();
    }

    public void GameOverMenu()
    {
        gameOverScreen.SetActive(true);
        Time.timeScale = 0f;  // 暂停游戏
    }

    public void HideGameOverScreen()
    {
        gameOverScreen.SetActive(false);
        Time.timeScale = 1f;  // 恢复游戏时间流逝
    }

    public void PauseUnpause()
    {
        if (pauseScreen.activeSelf==false)
        {
            pauseScreen.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            pauseScreen.SetActive(false);
            Time.timeScale = 1f;
        }

    }
}
