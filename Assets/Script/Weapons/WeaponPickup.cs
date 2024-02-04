using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Weapon weaponPrefab; // 在Unity编辑器中设置

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                // 传递Weapon类型的实例，而不是GameObject
                player.ChangeWeapon(weaponPrefab);
                Destroy(gameObject); // 拾取后销毁武器拾取物体
            }
        }
    }

}
