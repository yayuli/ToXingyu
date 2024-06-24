using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMusic : MonoBehaviour
{
    public AudioSource audioSource; // 指定一个AudioSource组件
    public List<AudioClip> musicClips = new List<AudioClip>(); // 存储音乐列表

    void Start()
    {
        PlayRandomMusic(); // 开始时播放随机音乐
    }

    void Update()
    {
        if (!audioSource.isPlaying) // 检查当前是否有音乐播放
        {
            PlayRandomMusic(); // 如果没有音乐播放，则播放新的随机音乐
        }
    }

    void PlayRandomMusic()
    {
        if (musicClips.Count == 0) return; // 如果音乐列表为空，则直接返回

        int randomIndex = Random.Range(0, musicClips.Count); // 随机选择一个音乐索引
        audioSource.clip = musicClips[randomIndex]; // 设置音乐片段
        audioSource.Play(); // 播放音乐
    }
}