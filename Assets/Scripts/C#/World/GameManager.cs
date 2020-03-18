using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static LayerController LayerController;
    public static Memory Memory;
    public static WorldSetting WorldSetting;

    [SerializeField] private Dungeon Dungeon;
    
    private void Awake()
    {
        LayerController = GameObject.Find("_LayerController").GetComponent<LayerController>();
        Memory = GameObject.Find("_Memory").GetComponent<Memory>();
        WorldSetting = GameObject.Find("_WorldSetting").GetComponent<WorldSetting>();
    }
    private void Start()
    {
        this.BuildDungeon();
    }
    private void BuildDungeon()
    {
        Dungeon.BuildDungeon();
        LayerController.Each();
    }

}
