using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Platform : Room
{
    [Header("Platform")]
    public GameObject Plat;
    public Collider Collider;

    private Vector3 Original_Pos;
    private Vector3 Next_Pos;

    private bool IsMoving = false;
    private bool AsMovedToNextLayer = false;
    private bool PlayerHasExitPlat = false;

    public int IsOnLayer = 0;
    public int NextLayerToMove = 1;

    private void Start()
    {
        //Plat.GetComponent<Renderer>().material.color = WorldSetting.Singleton.WorldPalet.Platform;
    }
    private void Update()
    {
        GameObject PlayerInstance = GameManager.Memory.PlayerInstance;

        bool IsPlayerOnPlat = Collider.bounds.Contains(PlayerInstance.transform.position);

        if (IsPlayerOnPlat && this.AsMovedToNextLayer == false) 
        { 
            MoveToNextLayer();
        }
    }
    public void Build()
    {
        this.transform.GetChild(0).GetComponent<NavMeshSurface>().BuildNavMesh();
    }
    private void MoveToNextLayer()
    {
        if (!IsMoving)
        {
            this.Original_Pos = this.transform.position;

            this.Next_Pos = Original_Pos;
            this.Next_Pos.y += 25;

            this.IsMoving = true;

            GameManager.Memory.PlayerInstance.transform.SetParent(this.transform);
            //GameManager.LayerController.Both();
        }
        else
        {
            if (this.Next_Pos.y - this.transform.position.y != 0)
            {
                this.transform.Translate(Vector3.up / 1);
            }
            else
            {
                //this.IsMoving = false;
                this.AsMovedToNextLayer = true;
                this.IsOnLayer = this.NextLayerToMove;

                GameManager.LayerController.Actual_Layer++;
                this.transform.SetParent(GameManager.LayerController.Rooms.GetChild(GameManager.LayerController.Actual_Layer));
                
                GameManager.LayerController.Each();


                GameManager.Memory.PlayerInstance.transform.SetParent(GameManager.Memory.PlayerParent);
            }
        }
    }
}
