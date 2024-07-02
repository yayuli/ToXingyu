using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class IntroAnimationController : MonoBehaviour
{
    public PlayableDirector director;
    public GameObject screen; // 指向显示动画的屏幕的引用
    private static bool hasPlayed = false;

    void Start()
    {
        if (!hasPlayed)
        {
            director.Play();
            director.stopped += OnAnimationStopped; // 添加事件监听器，动画结束时调用OnAnimationStopped
        }
        else
        {
            screen.SetActive(false); // 如果已经播放过动画，启动时隐藏屏幕
        }
    }

    private void OnAnimationStopped(PlayableDirector aDirector)
    {
        if (director == aDirector)
        {
            screen.SetActive(false); // 动画播放完毕后隐藏屏幕
            hasPlayed = true;
        }
    }

    void OnDestroy()
    {
        director.stopped -= OnAnimationStopped; // 移除事件监听器，避免内存泄漏
    }
}
