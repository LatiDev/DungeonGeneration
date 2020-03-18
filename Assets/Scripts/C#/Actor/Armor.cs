using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : SceneObject
{
    public float Value;
    public ElementType Element;

    public float ReduceDamage(Damage d)
    {
        return d.Value;
    }
}
