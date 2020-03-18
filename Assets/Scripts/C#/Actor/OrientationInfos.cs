using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationInfos
{
    public Vector3 Vector;
    public Vector3 NextPos;

    public OrientationInfos(Vector3 v, Vector3 np)
    {
        this.Vector = v;
        this.NextPos = np;
    }
}
