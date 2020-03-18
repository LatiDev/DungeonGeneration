using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCreator : MonoBehaviour
{
    public Dungeon Dungeon;
    
    private void Start()
    {
        Dungeon.BuildDungeon();
    }
    [ContextMenu("Create Dungeon")]
    public void Build()
    {
        Dungeon.BuildDungeon();
    }
}
