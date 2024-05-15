using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///this is the base class for all weapon classes, defining the basic behaviors and properties of weapons
/// </summary>
public abstract class WeaponBase : MonoBehaviour
{
    [Header("Config")]
    public float fireRate;//开火频率
    public float fireDistance;//射程
    protected Transform player;
    protected Vector2 offset;//武器对于玩家的位置偏移
    protected float timeSinceLastShot;

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

    //设置武器的位置偏移
    public void SetOffset(Vector2 o)
    {
        offset = o;
    }
    //抽象方法，由具体的武器类实现具体的行为
    public abstract void Shoot();
}
