using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairCase : Room
{
    public GameObject Core;
    public void Rotate(Vector3 NextPos)
    {
        Vector3 l = NextPos - this.transform.position;
        l.y = 0;

        float a = Vector3.Angle(-this.Core.transform.forward, l.normalized);

        Debug.DrawRay(this.transform.position, -this.Core.transform.forward * 4, Color.red);
        Debug.DrawRay(this.transform.position, l, Color.green);

        this.Core.transform.Rotate(new Vector3(0, a, 0));
    }
    public override void SpawnTree()
    {
        
    }
}
