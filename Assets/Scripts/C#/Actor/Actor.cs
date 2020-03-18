using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : SceneObject, ICanDamage, IDamagable
{
    public float Life;

    public virtual void Damage(IDamagable e, Damage d)
    {
        e.TakeDamage(d);
    }
    public virtual void TakeDamage(Damage d)
    {
        Dictionary<ElementType, float> A = this.GetGlobalArmor();
        /* this.Life -= A.ReduceDamage(d); */
    }
    public virtual Dictionary<ElementType, float> GetGlobalArmor()
    {
        return default(Dictionary<ElementType, float>);
    }
}
