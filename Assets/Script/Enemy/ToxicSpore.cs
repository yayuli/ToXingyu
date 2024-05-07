using System.Collections;
using UnityEngine;

public class ToxicSpore : Enemy
{
    [SerializeField] private GameObject sporeCloudPrefab; // Reference to the spore cloud prefab
    [SerializeField] private float burstRadius = 3f; // The radius within which it triggers the burst
    [SerializeField] private float cloudDuration = 5f; // How long the cloud lasts before disappearing
    [SerializeField] private int burstDamage = 1; // Damage caused at the moment of the burst

    private bool hasBurstOccurred = false; // Flag to prevent multiple bursts

    protected override void Start()
    {
        base.Start(); // Call the base start to initialize health and other components
    }

    void Update()
    {
        // Check distance to the player to potentially trigger the spore burst
        if (!hasBurstOccurred)
        {
            CheckProximity();
        }
    }

    private void CheckProximity()
    {
        if (target != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, target.position);
            if (distanceToPlayer <= burstRadius)
            {
                TriggerSporeBurst();
                hasBurstOccurred = true; // Ensure the burst only happens once
            }
        }
    }

    protected override void Die()
    {
        if (!hasBurstOccurred)
        {
            TriggerSporeBurst();
        }
        base.Die();
    }

    private void TriggerSporeBurst()
    {
        if (sporeCloudPrefab)
        {
            GameObject sporeCloud = Instantiate(sporeCloudPrefab, transform.position, Quaternion.identity);
            Destroy(sporeCloud, cloudDuration); // Destroy the spore cloud after it exists for the duration

            if (target != null)
            {
                Player player = target.GetComponent<Player>();
                if (player != null)
                {
                    player.TakeDamage(burstDamage); // Apply damage directly to the player
                }
            }
        }
    }
}
