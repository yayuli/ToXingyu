using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private IGenerator[] generators;
    [SerializeField] private MazeGenerator mazeGenerator;

    public NextLevelUI nextLevelUI;
    //public AudioManager audioManager;
    //public LoadingScreen loadingScreen;

    void Awake()
    {
        generators = FindObjectsOfType<MonoBehaviour>().OfType<IGenerator>().ToArray();
        InitializeGenerators();
    }

    private void Start()
    {
        if (nextLevelUI == null)
            nextLevelUI = FindObjectOfType<NextLevelUI>();
        //other...like audio and screen
    }

    IEnumerator TransitionToLevel(string levelName)
    {
        yield return StartCoroutine(nextLevelUI.FadeOut());

     
        yield return new WaitForEndOfFrame();  // 确保场景加载完成

        //yield return new WaitForSeconds(1f);
        // 开始淡入
        yield return StartCoroutine(nextLevelUI.FadeIn());

        //yield return StartCoroutine(nextLevelUI.FadeOut());
    }

    public void LoadLevel(string levelName)
    {
        StartCoroutine(TransitionToLevel(levelName));
    }

    private void InitializeGenerators()
    {
        if (mazeGenerator != null)
        {
            GenerateLevel();
        }
        else
        {
            Debug.LogError("MazeGenerator instance not found!");
        }
    }

    // 生成关卡的逻辑
    private void GenerateLevel()
    {
        // 生成迷宫
        mazeGenerator.GenerateMaze(mazeGenerator.mazeRows, mazeGenerator.mazeColumns);

        // 初始化和生成各种生成器的元素
        foreach (var generator in generators)
        {
            generator.Initialize(mazeGenerator);
            generator.Generate();
        }

        // 生成敌人
        if (EnemyManager.Instance != null)
        {
            EnemyManager.Instance.GenerateEnemies();
        }
        else
        {
            Debug.LogError("EnemyManager instance not found!");
        }
    }

    // 当触发器被触发时调用
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player"))
        {
            RefreshCurrentLevel();
        }
    }

    // 刷新当前关卡
    public void RefreshCurrentLevel()
    {
        StartCoroutine(TransitionToLevel(SceneManager.GetActiveScene().name));
        CleanUpLevel();
        if (EnemyManager.Instance != null)
        {
            EnemyManager.Instance.ResetSpawnTimes();
            EnemyManager.Instance.GenerateEnemies();
            Debug.Log("Attempted to regenerate enemies.");
        }
        GenerateLevel();
    }


    // 清理关卡的方法
    void CleanUpLevel()
    {
        // 这里可以添加清理逻辑，例如销毁当前生成的敌人、收集物等
        // 清理现有元素的逻辑
        if (EnemyManager.Instance != null)
        {
            EnemyManager.Instance.OnDestroyAllEnemies();
        }
    }
}
