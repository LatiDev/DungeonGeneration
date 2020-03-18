using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSetting : MonoBehaviour
{
    public Light Ligth;

    public List<GameObject> Trees_Configs;
    public List<GameObject> Rivers;

    public GameObject GetRandomTreeConfig
    {
        get
        {
            return this.Trees_Configs[Random.Range(0, this.Trees_Configs.Count)];
        }
    }
}
