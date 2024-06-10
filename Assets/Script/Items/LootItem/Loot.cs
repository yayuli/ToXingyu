using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    private Player player;

    private bool movingToPlayer;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float timeBetweenChecks = 0.2f;
    private float checkCounter;
    
    private LootItem lootData;

    public void Initialize(LootItem data)
    {
        lootData = data;
    }

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.GetComponent<Player>();
        }
        checkCounter = timeBetweenChecks; 
    }

    private void Update()
    {
        if (player == null)
        {
            Debug.LogWarning("Player not found!");
            return;
        }

        if (movingToPlayer)
        {
            //use cached player object reference for position update
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            checkCounter -= Time.deltaTime;
            if (checkCounter < 0)
            {
                checkCounter = timeBetweenChecks;

                if (Vector3.Distance(transform.position, player.transform.position) < player.pickupRange)
                {
                    movingToPlayer = true;
                    moveSpeed += player.moveSpeed; //can increase the movement speed
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SFXManager.instance.PlaySFXPitched(2);
            ExperienceLevelController.instance.AddExperience(lootData.experienceValue);
            //Debug.Log($"Player picked up {lootData.itemName} and gained {lootData.experienceValue} experience.");
            Destroy(gameObject);
        }
    }
}
