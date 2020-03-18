using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mapable : MonoBehaviour
{
    [Header("Info")]
    public int Map_X;
    public int Map_Y;
    public int Map_Z;

    public int Layer;
}
