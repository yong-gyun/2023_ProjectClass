using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public const float LIMITED_MOVE = 27.5F;

    public enum BulletType
    {
        None,
        Bomb,
        Follow
    }

    public enum EnemyType
    {
        Meteor,
        Dron,
        MaxCount,
        Boss,
    }

    public enum ItemType
    {
        Heal,
        Attack,
        Shield,
        MaxCount
    }

    public enum DroneType
    {
        OneShotDrone,
        DoubleShotDrone,
        MeleeDrone,
        ExplosionDrone,
        MaxCount
    }
}