using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
<<<<<<< HEAD
    public GameObject explosionEffect; // explosion effect Prefab
    public float delay = 2f; // explosion delay time 

    void Start()
    {
        Invoke("Explode", delay); // call the explosion method after 2 secs
=======
    public GameObject explosionEffect; // 爆炸效果Prefab
    public float delay = 2f; // 爆炸延迟时间

    void Start()
    {
        Invoke("Explode", delay); // 2秒后调用Explode方法
>>>>>>> b0bdbdcdd17ecdd501e2b3891178de4b898c4323
    }

    void Explode()
    {
<<<<<<< HEAD
        // instantiate the explosion effect get a reference to the particle system
        GameObject explosionInstance = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        ParticleSystem explosionParticles = explosionInstance.GetComponent<ParticleSystem>();

        // detection within explosion range
        float explosionRadius = 2f; // explosion radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        //if a destructible layer is detected, destroy it
        foreach (Collider2D hit in colliders)
        {
            if (hit.CompareTag("Wall"))
            {
                Destroy(hit.gameObject); 
            }
        }

        // destroyed the particle effect after the particales have finished playing
        if (explosionParticles != null)
        {
            //wait for the particles to finish playing
=======
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
>>>>>>> b0bdbdcdd17ecdd501e2b3891178de4b898c4323
            Destroy(explosionInstance, explosionParticles.main.duration);
        }
        else
        {
<<<<<<< HEAD
            Destroy(explosionInstance, 2f); 
        }

=======
            // 如果找不到粒子系统组件，立即销毁
            Destroy(explosionInstance, 2f); // 假设2秒足够播放大多数粒子效果
        }

        // 销毁炸弹自身
>>>>>>> b0bdbdcdd17ecdd501e2b3891178de4b898c4323
        Destroy(gameObject);
    }

}

