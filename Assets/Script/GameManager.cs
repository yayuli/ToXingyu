using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private IGenerator[] generators;
    [SerializeField] private MazeGenerator mazeGenerator;
    public MiniCamera miniCamera; // Assign this in the Inspector

    [SerializeField] private int addRows = 2;
    [SerializeField] private int addCols = 2;

    private int level = 1;

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

    IEnumerator TransitionToLevel(string sceneName)
    {
        yield return StartCoroutine(nextLevelUI.FadeOut());

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;


        // 循环检查场景是否加载完毕
        while (!asyncLoad.isDone)
        {
            // 更新加载进度
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);  // 因为加载进度在技术上永远到达不了1，所以除以0.9
            nextLevelUI.SetProgress(progress);

            // 如果加载完成，但未到100%，则手动设置为100%
            if (asyncLoad.progress >= 0.9f)
            {
                nextLevelUI.SetProgress(1);
                asyncLoad.allowSceneActivation = true;  // 允许场景激活
            }

            yield return null;
        }
        miniCamera.IncreaseCameraSize();
        yield return new WaitForSeconds(2.0f);
        // 开始淡入
        yield return StartCoroutine(nextLevelUI.FadeIn());

        //yield return StartCoroutine(nextLevelUI.FadeOut());
    }

    private void InitializeGenerators()
    {
        if (mazeGenerator != null)
        {
            GenerateLevel();
        }
    }

    // 生成关卡的逻辑
    private void GenerateLevel()
    {
        MazeGenerator.mazeRows += addRows;
        MazeGenerator.mazeColumns += addCols;

        // 生成迷宫
        mazeGenerator.GenerateMaze(MazeGenerator.mazeRows, MazeGenerator.mazeColumns);

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
       
        //GenerateLevel();
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
