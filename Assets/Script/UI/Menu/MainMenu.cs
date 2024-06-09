using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        // 首先销毁所有 DontDestroyOnLoad 对象
        DestroyAllDontDestroyOnLoadObjects();

        // 重新加载游戏场景
        SceneManager.LoadScene("GameScene");

        GameManager.instance.StartGame(2, 2);

        // 游戏速度重置为正常速度
        Time.timeScale = 1f;
    }

    public void DestroyAllDontDestroyOnLoadObjects()
    {
        foreach (var go in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (go.hideFlags == HideFlags.None && go.scene.buildIndex == -1)
            {
                Destroy(go);
            }
        }
    }
}
