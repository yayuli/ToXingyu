using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "Config/EnemyConfig")]
public class EnemyConfig : ScriptableObject
{
    public string enemyType;
    public float baseHealth;
    public float healthIncrementPerWave;
    public float baseSpeed;
    public float speedIncrementPerWave;
    public float baseDamage;
    public float damageIncrementPerWave;
    //public EnemyAbility ability;  // Reference to ability
}