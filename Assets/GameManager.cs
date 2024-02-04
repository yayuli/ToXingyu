using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject pauseMenu;
    private bool isGamePaused = false;
    // Start is called before the first frame update
    void Awake()
    {
        if(instance ==null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance!= this)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        isGamePaused = !isGamePaused;
        pauseMenu.SetActive(isGamePaused);
        Time.timeScale = isGamePaused ? 0 : 1;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void EndGame(bool isWin)
    {
        if(isWin)
        {
            Debug.Log("You Win!");
        }
        else
        {
            Debug.Log("Game Over!");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }


}
