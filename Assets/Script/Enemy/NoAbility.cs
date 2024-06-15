using UnityEngine;

[CreateAssetMenu(fileName = "NoAbility", menuName = "Enemy Abilities/No Ability")]
public class NoAbility : EnemyAbility
{
    public string enemyType;
    public float baseHealth;
    public float healthIncrementPerWave;
    public float baseSpeed;
    public float speedIncrementPerWave;
    public float baseDamage;
    public float damageIncrementPerWave;

    public override void Execute(GameObject enemy)
    {
        // noooo ability
    }
}
