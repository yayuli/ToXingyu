using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///this is the base class for all weapon classes, defining the basic behaviors and properties of weapons
///所有武器共有的基类，定义了武器的基本行为和属性。可以在这里添加一些基础的属性和方法，如武器的公共方法和一些基本的属性（例如，所有武器共有的冷却逻辑）
/// </summary>
public abstract class WeaponBase : MonoBehaviour
{
    public static WeaponBase Instance;
    [Header("Config")]
    public float fireRate;//开火频率
    public float fireDistance;//射程
    protected Transform player;
    protected Vector2 offset;//武器对于玩家的位置偏移
    protected float timeSinceLastShot;

    // 新添加的属性交互itemdata
    protected int attackPower;
    protected float range;
    protected int damage;
    protected float speed;
    protected float cooldown;

    //初始方法， 在派生类中可以被重写
    protected virtual void Start()
    {
        player = GameObject.Find("Player").transform;
        timeSinceLastShot = fireRate;//初始化开火时间
    }

    // Update is called once per frame，可以在派生类被重写
    protected virtual void Update()
    {
        timeSinceLastShot += Time.deltaTime;//更新距离上次开火的时间
        transform.position = (Vector2)player.position + offset;//更新武器的位置
    }

    // 添加虚的 Initialize 方法接受 ItemData
    public virtual void Initialize(ItemData data)
    {
        if (data != null)
        {
            attackPower = data.attackPower;
            range = data.range;
            damage = data.effectMagnitude;  // 假设 damage 对应于 ItemData 的 effectMagnitude
            speed = data.speed;
            cooldown = data.cooldown;
        }
    }

    //设置武器的位置偏移
    public void SetOffset(Vector2 o)
    {
        offset = o;
    }
    //抽象方法，由具体的武器类实现具体的行为
    public abstract void Shoot();
}
