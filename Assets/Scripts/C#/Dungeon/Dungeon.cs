using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dungeon : MonoBehaviour
{
    public GameObject Starter;

    public GameObject RoomPrefab;
    public GameObject StairPrefab;
    public GameObject EndRoomPrefab;

    public Transform RoomsParent;

    private List<GameObject[,]> DungeonMap = new List<GameObject[,]>();
    
    [Range(0, 100)]
    public int Rooms = 10;
    [Range(1, 30)]
    public int Layer;

    private GameObject LastRoom;
    private Transform CurrentLayerParent;
    private GameObject[,] CurrentLayer;

    private int LastLayer = 0;
    private int LastRoomX = 0;
    private int LastRoomY = 0;
    private int LastRoomZ = 0;

    private delegate GameObject Func_G_Pos(Vector3 Pos);
    private delegate void Func_Map<T>(T M) where T: Mapable;
    private delegate Transform Func_GetCompoGObject(int index, int offset);
    private delegate void Func_RP(RelativePosition v);


    private void ResetState(int Rooms, int Layer)
    {
        this.Rooms = Rooms;
        this.Layer = Layer;


        this.LastRoom = Starter;

        this.LastRoomX = 4;
        this.LastRoomY = 4;
        this.LastRoomZ = 0;

        Room s = this.Starter.GetComponent<Room>();
        s.Map_X = this.LastRoomX;
        s.Map_Y = this.LastRoomY;
        s.ResetWalls();

        GameObject[,] RoomMap = new GameObject[Rooms, Rooms];
        RoomMap[LastRoomX, LastRoomY] = Starter;

        this.DungeonMap = new List<GameObject[,]>() { RoomMap, };

        for(int i = 0; i < this.Layer; i++) this.DungeonMap.Add(new GameObject[Rooms, Rooms]);
    }
    private void ResetRandomState()
    {
        print("Random Stats");
        this.ResetState(Rooms, Layer);
    }

    public void BuildDungeon()
    {
        print("Building Dungeon");

        DungeonSpawnParams DSP = GameObject.Find("_DungeonSpawnParams")?.GetComponent<DungeonSpawnParams>();
        if (DSP)
            this.ResetState(DSP.Rooms, DSP.Layer);
        else
            this.ResetRandomState();

        this.DeleteRooms();
        this.CreateRooms();
        this.SetupStarterEnd();

        //GenerateMapable gm = new GenerateMapable();
        //gm.Generate<Mapable>(10, 10, 500, new RelativePosition[] { RelativePosition.North, RelativePosition.South, RelativePosition.East, RelativePosition.West }, true);

        /*

        print("Building River");
        River r = new River();
        List<RiverData[,]> riverdata = r.CreateRiver(this.MaxRoomX - this.MinRoomX, this.MaxRoomY - this.MinRoomX, this.Layer, 500, 50);

        this.OverrideWithRiver(riverdata[0], this.DungeonMap[0]);
        
         */      
    }
    public void BakeDungeon()
    {
        this.Starter.GetComponent<Room>().Bake();
    }
    private void DeleteRooms()
    {
        foreach (Transform t in this.RoomsParent)
        {
            Destroy(t.gameObject);
        }
    }
    private void CreateRooms()
    {
        for (int l = 0; l < this.Layer; l++)
        {
            this.CurrentLayer = this.DungeonMap[l];
            this.LastLayer = l;
            
            int RoomX = LastRoomX;
            int RoomY = LastRoomY;

            bool StairSpawned = false;

            GameObject LayerParent = new GameObject();
            LayerParent.name = $"Layer {l}";
            LayerParent.transform.SetParent(this.RoomsParent);

            this.CurrentLayerParent = LayerParent.transform;

            if (l == 0)
            {
                GameManager.Memory.StarterInstance.transform.SetParent(LayerParent.transform);
            }
            for (int i = 0; i < Rooms; i++)
            {
                List<Vector3> Dirs = new List<Vector3>
                { Vector3.right, Vector3.forward,
                -Vector3.right, -Vector3.forward};

                //Arreter a la limit
                //Veut dire que 28 < 30 - 1
                //Donc 28 < 29
                if (RoomX == this.Rooms - 1 || CurrentLayer[RoomX + 1, RoomY] != null)
                {
                    Dirs.Remove(Vector3.right);
                }
                if (RoomY == this.Rooms - 1 || CurrentLayer[RoomX, RoomY + 1] != null)
                {
                    Dirs.Remove(Vector3.forward);
                }
                if (RoomX == 1 || CurrentLayer[RoomX - 1, RoomY] != null)
                {
                    Dirs.Remove(-Vector3.right);
                }
                if (RoomY == 1 || CurrentLayer[RoomX, RoomY - 1] != null)
                {
                    Dirs.Remove(-Vector3.forward);
                }

                //if block then spawn stair
                if (Dirs.Count == 0)
                {
                    GameObject Room = this.SpawnRoomDestroy(this.SpawnStair, this.LastRoom.transform.position);
                    //Room.GetComponent<Platform>().Build();

                    StairSpawned = true;
                    break;
                }
                else
                {
                    Vector3 Dir = Dirs[Random.Range(0, Dirs.Count)];

                    Vector3 RealPos = this.LastRoom.transform.position + Dir * 50;
                    RealPos.y = this.LastRoomZ;

                    if (Dir == Vector3.right) { RoomX++; }
                    else if (Dir == Vector3.forward) { RoomY++; }
                    else if (Dir == -Vector3.right) { RoomX--; }
                    else if (Dir == -Vector3.forward) { RoomY--; }

                    //Relative pos from last room spawned
                    //this.LastRelativePos = this.VectorToRelativePos(Dir);

                    this.UpdateNextRoomInfo(RoomX, RoomY, this.LastRoomZ, this.LastLayer);                    
                    GameObject Room = this.SpawnNormalRoom(RealPos);
                }
            }
            if (!StairSpawned)
            {
                GameObject S = this.SpawnRoomDestroy(this.SpawnStair, this.LastRoom.transform.position);
                //S.GetComponent<Platform>().Build();
            }

            //print($"Min X:{MinRoomX}, Max X:{MaxRoomX}");
            //print($"Min Y:{MinRoomY}, Max Y:{MaxRoomY}");

            this.SetupWallsForLayer(this.CurrentLayerParent);        
        }
    }
    #region Spawner
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private T SpawnEntity<T>(GameObject Prefab, Vector3 Pos, Transform Parent)
    {
        GameObject Entity = Instantiate(Prefab);
        Entity.transform.position = Pos;
        Entity.transform.SetParent(Parent);

        return Entity.GetComponent<T>();
    }
    private GameObject SpawnEntity(GameObject Prefab, Vector3 Pos, Transform Parent)
    {
        GameObject Entity = Instantiate(Prefab);
        Entity.transform.position = Pos;
        Entity.transform.SetParent(Parent);

        return Entity;
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private Ta SpawnRoom<Ta, Tb>(GameObject Prefab, Vector3 Pos, Func_Map<Tb> Setup) where Tb : Mapable
    {
        GameObject Room = this.SpawnEntity(Prefab, Pos, this.CurrentLayerParent);
        Setup(Room.GetComponent<Tb>());


        Room.name += $"{this.LastLayer}_{this.CurrentLayerParent.childCount}";
        this.LastRoom = Room;

        return Room.GetComponent<Ta>();
    }
    private GameObject SpawnRoom<Tb>(GameObject Prefab, Vector3 Pos, Func_Map<Tb> Setup) where Tb : Mapable
    {
        GameObject Room = this.SpawnEntity(Prefab, Pos, this.CurrentLayerParent);
        Setup(Room.GetComponent<Tb>());


        Room.name += $"{this.LastLayer}_{this.CurrentLayerParent.childCount}";
        this.LastRoom = Room;
        
        return Room;
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private T SpawnNormalRoom<T>(Vector3 pos)
    {
        GameObject r = this.SpawnNormalRoom(pos);
        return r.GetComponent<T>();
    }
    private GameObject SpawnNormalRoom(Vector3 pos)
    {
        GameObject r = this.SpawnRoom<Room>(this.RoomPrefab, pos, this.SetupNormalRoom);
        Room rc = r.GetComponent<Room>();
        rc.SpawnTree();

        return r;
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private T SpawnEndRoom<T>(Vector3 pos)
    {
        GameObject er = this.SpawnEndRoom(pos);
        return er.GetComponent<T>();
    }
    private GameObject SpawnEndRoom(Vector3 pos)
    {
        return this.SpawnRoom<Room>(this.EndRoomPrefab, pos, this.SetupNormalRoom);
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private T SpawnStair<T>(Vector3 pos)
    {
        GameObject sr = this.SpawnStair(pos);
        return sr.GetComponent<T>();
    }
    private GameObject SpawnStair(Vector3 pos)
    {
        return this.SpawnRoom<Platform>(this.StairPrefab, pos, this.SetupStair);
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private GameObject SpawnRoomDestroy(Func_G_Pos Func, Vector3 pos, int childindex = 1)
    {
        Destroy(this.CurrentLayerParent.GetChild(this.CurrentLayerParent.childCount - childindex).gameObject);
        //Func -> SpawnRoom Type of function
        return Func(pos);
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #endregion
    #region Spawn Setuper
    private void SetupNormalRoom(Mapable Room)
    {
        Room.Map_X = this.LastRoomX;
        Room.Map_Y = this.LastRoomY;
        Room.Map_Z = this.LastRoomZ;

        Room.Layer = this.LastLayer;

        //Room.BeforeRoomPos = this.LastRelativePos;


        this.CurrentLayer[this.LastRoomX, this.LastRoomY] = Room.gameObject;
    }
    private void SetupStair(Platform Stair)
    {
        this.SetupNormalRoom(Stair);

        this.LastRoomZ += 25;

        GameObject[,] Layer = this.DungeonMap[this.LastLayer + 1];
        Layer[this.LastRoomX, this.LastRoomY] = Stair.gameObject;

        Stair.IsOnLayer = this.LastLayer;
        Stair.NextLayerToMove = this.LastLayer+1;

    }
    #endregion
    private void UpdateNextRoomInfo(int RoomX, int RoomY, int RoomZ, int RoomL)
    {
        this.LastRoomX = RoomX;
        this.LastRoomY = RoomY;
        this.LastRoomZ = RoomZ;

        this.LastLayer = RoomL;
    }
    #region Wall Setuper
    private void SetupWalls(int RoomX, int RoomY, Func_RP f)
    {
        if (this.IsRoomExits(RoomX, RoomY, RelativePosition.North)) { f(RelativePosition.North); }
        if (this.IsRoomExits(RoomX, RoomY, RelativePosition.South)) { f(RelativePosition.South); }
        if (this.IsRoomExits(RoomX, RoomY, RelativePosition.East))  { f(RelativePosition.East); }
        if (this.IsRoomExits(RoomX, RoomY, RelativePosition.West))  { f(RelativePosition.West); }

        if (this.IsRoomExits(RoomX, RoomY, RelativePosition.North_East))
        {
            if (this.IsRoomExits(RoomX, RoomY, RelativePosition.North) && 
                this.IsRoomExits(RoomX, RoomY, RelativePosition.East)) 
            { f(RelativePosition.North_East); }
        }
        if (this.IsRoomExits(RoomX, RoomY, RelativePosition.North_West))
        {
            if (this.IsRoomExits(RoomX, RoomY, RelativePosition.North) && 
                this.IsRoomExits(RoomX, RoomY, RelativePosition.West))
            { f(RelativePosition.North_West); }
        }
        if (this.IsRoomExits(RoomX, RoomY, RelativePosition.South_East))
        {
            if (this.IsRoomExits(RoomX, RoomY, RelativePosition.South) && 
                this.IsRoomExits(RoomX, RoomY, RelativePosition.East))
            { f(RelativePosition.South_East); }
        }
        if (this.IsRoomExits(RoomX, RoomY, RelativePosition.South_West))
        {
            if (this.IsRoomExits(RoomX, RoomY, RelativePosition.South) && 
                this.IsRoomExits(RoomX, RoomY, RelativePosition.West))
            { f(RelativePosition.South_West); }
        }
    }
    private void SetupWalls(GameObject r)
    {
        Room r_ = r.GetComponent<Room>();
        this.SetupWalls(r_.Map_X, r_.Map_Y, r_.OrientTo);
    }
    public bool IsRoomExits(int RoomX, int RoomY, RelativePosition p)
    {
        switch (p)
        {
            case RelativePosition.North:
                return RoomY + 1 < this.Rooms && this.CurrentLayer[RoomX, RoomY + 1];
            case RelativePosition.South:
                return RoomY - 1 > 0 && this.CurrentLayer[RoomX, RoomY - 1];
            case RelativePosition.East:
                return RoomX + 1 < this.Rooms && this.CurrentLayer[RoomX + 1, RoomY];
            case RelativePosition.West:
                return RoomX - 1 > 0 && this.CurrentLayer[RoomX - 1, RoomY];

            case RelativePosition.North_East:
                return (RoomX + 1 < this.Rooms && RoomY + 1 < this.Rooms) && this.CurrentLayer[RoomX + 1, RoomY + 1];
            case RelativePosition.South_East:
                return (RoomX + 1 < this.Rooms && RoomY - 1 > 0) && this.CurrentLayer[RoomX + 1, RoomY - 1];
            case RelativePosition.South_West:
                return (RoomX - 1 > 0 && RoomY - 1 > 0) && this.CurrentLayer[RoomX - 1, RoomY - 1];
            case RelativePosition.North_West:
                return (RoomX - 1 > 0 && RoomY + 1 < this.Rooms) && this.CurrentLayer[RoomX - 1, RoomY + 1];

            default:
                return false;
        }
    }
    #endregion
    private void SetupStarterEnd()
    {
        GameObject EndRoom = this.SpawnRoomDestroy(this.SpawnEndRoom, this.LastRoom.transform.position);
        this.SetupWalls(EndRoom);

        this.CurrentLayer = this.DungeonMap[0];
        this.SetupWalls(this.Starter);
    }
    private void SetupWallsForLayer(Transform CurrentLayer_Transform)
    {
        for (int R = 0; R < CurrentLayer_Transform.transform.childCount - 1; R++ )
        {
            Room r = CurrentLayer_Transform.GetChild(R).GetComponent<Room>();            
            this.SetupWalls(r.Map_X, r.Map_Y, r.OrientTo);
        }
    }
    private void OverrideTerrain(int RoomX, int RoomY, int layer, GameObject Terrain)
    {
        GameObject[,] o = this.DungeonMap[layer];
        Room r = o[RoomX, RoomY].GetComponent<Room>();

        r.ChangeTerrain(Terrain);
    }
    private void OverrideWithRiver(RiverData[,] RiverData, GameObject[,] Layer)
    {

    }
}