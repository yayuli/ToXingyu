using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // 调用此方法以开始游戏并加载下一个场景
    public void StartGame()
    {
        // 这里假设下一个场景的索引是 1
        SceneManager.LoadScene(1);
    }
}
