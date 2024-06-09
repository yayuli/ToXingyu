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

    public int initialRows = 2;
    public int initialColumns = 2;
    [SerializeField] private int addRows = 2;
    [SerializeField] private int addCols = 2;

    private int level = 1;

    public NextLevelUI nextLevelUI;

    //public AudioManager audioManager;
    //public LoadingScreen loadingScreen;

    // Initialization of the singleton pattern
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Ensures that the object persists across scenes
        }
        else
        {
            Destroy(gameObject); // Ensures only one instance of the GameManager exists
        }

        generators = FindObjectsOfType<MonoBehaviour>().OfType<IGenerator>().ToArray();
        InitializeGenerators();
    }

    // Starts the game by loading the specified "GameScene"
    public void StartGame(int rows, int columns)
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to the scene loaded event
    }

    // Handles actions after the scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene")
        {
            MazeGenerator mazeGenerator = FindObjectOfType<MazeGenerator>();
            mazeGenerator.ResetMazeSizeToDefault();
            SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe to prevent multiple calls
        }
    }

    // Ensures required components are initialized at start
    private void Start()
    {
        if (nextLevelUI == null)
            nextLevelUI = FindObjectOfType<NextLevelUI>();
        //SFX
    }

    // Manages the transition between levels with a fade out effect
    IEnumerator TransitionToLevel(string sceneName)
    {
        Time.timeScale = 0f; // Stops gameplay
        yield return StartCoroutine(nextLevelUI.FadeOut());

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f); // Normalizing load progress
            nextLevelUI.SetProgress(progress);

            if (asyncLoad.progress >= 0.9f)
            {
                nextLevelUI.SetProgress(1);
                asyncLoad.allowSceneActivation = true; // Allows scene to activate
            }

            yield return null;
        }

        yield return new WaitForSeconds(2.0f);
        yield return StartCoroutine(nextLevelUI.FadeIn());
        Time.timeScale = 1f; // Resumes gameplay
        EnemyManager.Instance.OnEnable();
    }


    // Initializes generators if they are available
    private void InitializeGenerators()
    {
        if (mazeGenerator != null)
        {
            GenerateLevel();
        }
    }

    // Generates the level and manages enemy and player placement
    private void GenerateLevel()
    {
        MazeGenerator.mazeRows += addRows;
        MazeGenerator.mazeColumns += addCols;
        mazeGenerator.GenerateMaze(MazeGenerator.mazeRows, MazeGenerator.mazeColumns);
        Vector3 startPosition = mazeGenerator.GetStartPosition();
        Player.instance.transform.position = startPosition;

    }

    // Triggered when the player collides with a specific trigger collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            RefreshCurrentLevel();
        }
    }

    // Refreshes the current level by reloading the scene and resetting game elements
    public void RefreshCurrentLevel()
    {
        StartCoroutine(TransitionToLevel(SceneManager.GetActiveScene().name));
        Player.instance.ResetPlayerState();
        if (EnemyManager.Instance != null)
        {
            EnemyManager.Instance.ResetAndSpawnEnemies();
        }
        WeaponPanel.instance.UpdateWeaponSlotsDisplay();
    }
}
