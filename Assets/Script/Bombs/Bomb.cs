using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject explosionEffect; // 爆炸效果Prefab
    public float delay = 2f; // 爆炸延迟时间

    void Start()
    {
        Invoke("Explode", delay); // 2秒后调用Explode方法
    }

    void Explode()
    {
        // 实例化爆炸效果并获取粒子系统的引用
        GameObject explosionInstance = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        ParticleSystem explosionParticles = explosionInstance.GetComponent<ParticleSystem>();

        // 爆炸范围内检测
        float explosionRadius = 2f; // 爆炸半径
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D hit in colliders)
        {
            // 如果检测到的是墙壁
            if (hit.CompareTag("Wall"))
            {
                Destroy(hit.gameObject); // 销毁墙壁
            }
        }

        // 确保粒子播放完毕后销毁粒子效果GameObject
        if (explosionParticles != null)
        {
            // 等待粒子播放完毕
            Destroy(explosionInstance, explosionParticles.main.duration);
        }
        else
        {
            // 如果找不到粒子系统组件，立即销毁
            Destroy(explosionInstance, 2f); // 假设2秒足够播放大多数粒子效果
        }

        // 销毁炸弹自身
        Destroy(gameObject);
    }

}

