using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSpace : MonoBehaviour
{
    public float X_Scale;
    public float Y_Scale;
    public float Z_Scale;

    public float X_Offest;
    public float Y_Offest;
    public float Z_Offest;

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(this.transform.position + new Vector3(X_Offest, Y_Offest, Z_Offest),
            new Vector3(X_Scale, Y_Scale, Z_Scale));
    }
}
