using System.Collections;
using UnityEngine;

public class ToxicSpore : Enemy
{
    [SerializeField] private SporeBurstAbility burstAbility;
    private bool hasBurstOccurred = false; // Flag to prevent multiple bursts

    void Update()
    {
        if (!hasBurstOccurred && target != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, target.position);
            if (distanceToPlayer <= burstAbility.burstRadius)
            {
                burstAbility.Execute(gameObject);
                hasBurstOccurred = true;
            }
        }
    }

    protected override void Die()
    {
        if (!hasBurstOccurred)
        {
            burstAbility.Execute(gameObject);
        }
        base.Die();
    }

}
