using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dungeon Setting", menuName = "Custom/Dungeon Setting")]
public class DungeonSettings : ScriptableObject
{
    public GameObject Bush_Prefab;
    public GameObject Flower_Prefab;
    public GameObject Grass_Prefab;
    public GameObject Rock_Prefab;
    public GameObject Terrain_Prefab;
    public GameObject Tree_Prefab;
}
