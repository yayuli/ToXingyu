using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
        public static GameManager instance;
        private IGenerator[] generators;
        [SerializeField] private MazeGenerator mazeGenerator;

        [SerializeField] private int addRows = 2;
        [SerializeField] private int addCols = 2;

        private int level = 1;

        public NextLevelUI nextLevelUI;

        //public AudioManager audioManager;
        //public LoadingScreen loadingScreen;

        void Awake()
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
            else
            {
                Debug.LogError("MazeGenerator instance not found!");
            }
        }

        // 生成关卡的逻辑
        private void GenerateLevel()
        {
            MazeGenerator.mazeRows += addRows;
            MazeGenerator.mazeColumns += addCols;

            // 生成迷宫
            mazeGenerator.GenerateMaze(MazeGenerator.mazeRows, MazeGenerator.mazeColumns);

            // 设置玩家位置
            Vector3 startPosition = mazeGenerator.GetStartPosition();
            Player.instance.transform.position = startPosition;

            // 生成敌人
            if (EnemyManager.Instance != null)
            {
                EnemyManager.Instance.SpawnEnemy();
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
                // 重置 EnemyManager 的生成计时和状态
                EnemyManager.Instance.RemoveEnemy(gameObject);
                // 此处应确保所有需要的初始化步骤都被调用
                EnemyManager.Instance.SpawnEnemy();
                Debug.Log("Attempted to regenerate enemies.");
            }


            // 重置玩家和武器而不是重新创建它们
            Player.instance.ResetPlayerState();

            WeaponPanel.instance.UpdateWeaponSlotsDisplay();
            //WeaponManager.instance.ResetWeapons();

            //GenerateLevel();
        }


        // 清理关卡的方法
        void CleanUpLevel()
        {
            
        }
}

