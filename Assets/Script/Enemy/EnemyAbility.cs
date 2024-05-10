using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// other enemies' specific abilities will be inherited form this script
/// </summary>
public abstract class EnemyAbility : ScriptableObject
{
    public abstract void Execute(GameObject enemy);
}

