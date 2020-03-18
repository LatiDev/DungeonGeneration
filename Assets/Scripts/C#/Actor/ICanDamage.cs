using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICanDamage
{
    void Damage(IDamagable e, Damage d);
}
