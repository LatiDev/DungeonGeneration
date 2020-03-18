using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverData
{
    public GameObject _Prefab;
    public RiverDataBlock DataBlock;

    public void Setup(GameObject p) 
    {
        this._Prefab = p;
        this.DataBlock = p.GetComponent<RiverDataBlock>(); 
    }
}
