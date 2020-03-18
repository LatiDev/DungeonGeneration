using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Item, ICanDamage
{
    public Transform BulletSpawnPoint;

    public void Damage(IDamagable e, Damage d)
    {
        e.TakeDamage(d);
    }
}
