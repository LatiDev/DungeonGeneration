using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMaker : MonoBehaviour
{
    [SerializeField] private Dungeon Dungeon;
    [SerializeField] private LayerController LController;

    private void Start()
    {
        this.Dungeon.BuildDungeon();
        this.LController.Each();
        //this.Dungeon.BakeDungeon();
    }
}
