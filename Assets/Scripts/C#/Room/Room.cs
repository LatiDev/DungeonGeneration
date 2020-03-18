using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Room : Mapable
{
    [Header("Room")]
    public GameObject Plane;
    [SerializeField] protected NavMeshSurface Plane_NavMesh;
    [SerializeField] protected Renderer Plane_R;

    [SerializeField] private GameObject Foward;
    [SerializeField] private GameObject Backward;
    [SerializeField] private GameObject Rigth;
    [SerializeField] private GameObject Left;

    [SerializeField] private GameObject Turn_FowardRigth;
    [SerializeField] private GameObject Turn_FowardLeft;

    [SerializeField] private GameObject Turn_BackRigth;
    [SerializeField] private GameObject Turn_BackLeft;

    //[SerializeField] private GameObject SpawnPoint;
    //[SerializeField] private GameObject Enemys;

    //[SerializeField] private BoxCollider Colid;

    private void Start()
    {
        //Plane_R.material.color = WorldSetting.Singleton.WorldPalet.Grass;
    }
    public void Bake()
    {
        Plane_NavMesh.BuildNavMesh();
    }
    public void OrientTo(RelativePosition p)
    {
        switch (p)
        {
            case RelativePosition.North:
                this.Foward.SetActive(false);
                break;
            case RelativePosition.South:
                this.Backward.SetActive(false);
                break;
            case RelativePosition.West:
                this.Left.SetActive(false);
                break;
            case RelativePosition.East:
                this.Rigth.SetActive(false);
                break;


            case RelativePosition.North_East:
                this.Turn_FowardRigth.SetActive(false);
                break;
            case RelativePosition.South_East:
                this.Turn_BackRigth.SetActive(false);
                break;
            case RelativePosition.South_West:
                this.Turn_BackLeft.SetActive(false);
                break;
            case RelativePosition.North_West:
                this.Turn_FowardLeft.SetActive(false);
                break;            
        }
    }
    public void ResetWalls()
    {
        this.Foward.SetActive(true);
        this.Backward.SetActive(true);
        this.Rigth.SetActive(true);
        this.Left.SetActive(true);
    }
    public virtual void SpawnTree()
    {
        int rn = Random.Range(0, 2);
        if (rn == 1)
        {
            GameObject Tree_Config = Instantiate(GameManager.WorldSetting.Trees_Configs[Random.Range(0, 4)]);

            Tree_Config.transform.position = this.transform.position;
            Tree_Config.transform.SetParent(this.transform);
        }
    }
    public void ChangeTerrain(GameObject Prefab)
    {
        //print($"{this.name}, Change");
        
        GameObject p = Instantiate(Prefab);
        p.transform.position = Plane.transform.position;

        Destroy(this.Plane);

        p.transform.SetParent(this.transform);
        this.Plane = p;
    }
}
