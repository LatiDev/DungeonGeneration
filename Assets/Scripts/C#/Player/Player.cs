using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    public Armor Casque;
    public Armor Plastron;
    public Armor Jambiere;
    public Armor Botte;

    private delegate void AddCorrectly(Armor a, Dictionary<ElementType, float> Dict);

    private void Update()
    {
    }
    public override void Damage(IDamagable e, Damage d)
    {
        base.Damage(e, d);
    }
    public override void TakeDamage(Damage d)
    {
        base.TakeDamage(d);
    }
    public override Dictionary<ElementType, float> GetGlobalArmor()
    {
        AddCorrectly d = delegate (Armor a, Dictionary<ElementType, float> Dict)
        {
            if (Dict.ContainsKey(a.Element))
            {
                Dict[a.Element] += a.Value;
            }
            else
            {
                Dict.Add(a.Element, a.Value);
            }
        };


        Dictionary<ElementType, float> D = new Dictionary<ElementType, float>();

        d(Casque, D);
        d(Plastron, D);
        d(Jambiere, D);
        d(Botte, D);

        return D;
    }
}
