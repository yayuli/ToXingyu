using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SporeBurstAbility", menuName = "Enemy Abilities/Spore Burst")]
public class SporeBurstAbility : EnemyAbility
{
    [SerializeField] private GameObject sporeCloudPrefab; // Reference to the spore cloud prefab
    public float burstRadius = 3f; // The radius within which it triggers the burst
    [SerializeField] private float cloudDuration = 5f; // How long the cloud lasts before disappearing
    [SerializeField] private int burstDamage = 1; // Damage caused at the moment of the burst
    public override void Execute(GameObject enemy)
    {
        Transform target = GameObject.FindWithTag("Player").transform;
        if (target != null && Vector3.Distance(enemy.transform.position, target.position) <= burstRadius)
        {
            GameObject sporeCloud = GameObject.Instantiate(sporeCloudPrefab, enemy.transform.position, Quaternion.identity);
            GameObject.Destroy(sporeCloud, cloudDuration); // destroy spore cloud

            Player player = target.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(burstDamage);
            }
        }
    }
}
