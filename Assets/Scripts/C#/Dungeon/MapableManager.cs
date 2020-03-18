using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapableManager : MonoBehaviour
{
    public Transform RoomParent;

    public GameObject RoomPrefab;    
    public Transform RoomPool;

    public GameObject StarterPrefab;
    public Transform StarterPool;

    public GameObject PlatformPrefab;
    public Transform PlatformPool;

    [Range(1, 1000)] public int _RoomNumber;
    private int RoomNumber
    {
        get
        {
            return _RoomNumber + 1;
        }
    }

    [Range(1, 1000)] public int ScaleX;
    [Range(1, 1000)] public int ScaleY;

    public RelativePosition[] Pos;
    public bool Override = false;

    private bool IsPoolCreated = false;

    private void Update()
    {
        CreateMapable();
    }

    public void CreateMapable()
    {
        if (!IsPoolCreated)
        {
            ResetRoomPool();
            CreateRoomPool(RoomNumber);
        }

        Mapable_Info[,] m_ = this.Generate<Mapable_Info>(ScaleX, ScaleY, RoomNumber, Pos, Override);
        CreateRooms(m_);

        this.StarterPool.gameObject.SetActive(false);
        this.PlatformPool.gameObject.SetActive(false);
        this.RoomPool.gameObject.SetActive(false);
    }
    public void ResetPool()
    {
        ResetRoomPool();
        CreateRoomPool(RoomNumber);
    }
    private void CreateRoomPool(int Number)
    {
        if (IsPoolCreated)
        {
            //Not Enougth room for dungeon gotta spawned some
            if (Number > RoomPool.childCount)
            {
                int rn = Number - RoomPool.childCount;
                for (int n = 0; n < rn; n++)
                {
                    Instantiate(RoomPrefab, RoomPool);
                }
            }
            //to much room for dungeon gotta delet some
            else if (Number < RoomPool.childCount)
            {
                int rn = RoomPool.childCount - Number;
                for (int n = 0; n < rn; n++)
                {
                    Transform t = RoomPool.GetChild(0);
                    Destroy(t.gameObject);
                }
            }
            else
            {
                //Nothing
            }
        }
        else
        {
            for (int n = 0; n < Number; n++)
            {
                Instantiate(RoomPrefab, RoomPool);
            }

            IsPoolCreated = true;
        }
    }
    private void ResetRoomPool()
    {
        int n = RoomParent.childCount;
        for (int c = 0; c < n; c++)
        {
            Transform child = RoomParent.GetChild(0);

            child.SetParent(RoomPool);
            child.transform.position = Vector3.zero;
        }
    }
    private void CreateRooms(Mapable_Info[,] Mi)
    {
        for (int x = 0; x < ScaleX; x++)
        {
            for (int y = 0; y < ScaleY; y++)
            {
                Mapable_Info _m = Mi[x, y];

                if (_m != null)
                {
                    //print(_m.ToString());
                    
                    Transform t = default(Transform);
                    if (_m.IsStarter)
                    {
                        t = this.StarterPool.GetChild(0);
                    } 
                    else if (_m.IsPlatform)
                    {
                        t = this.PlatformPool.GetChild(0);
                    }
                    else
                    {
                        t = this.RoomPool.GetChild(0);
                    }
                    
                    t.SetParent(this.RoomParent);

                    Vector3 pos = new Vector3(x, 0, y);
                    t.transform.position = pos * 50;
                }
            }
        }
    }
    public Mapable_Info[,] Generate<T>(int ScaleX, int ScaleY, int RoomNumber, RelativePosition[] Moves, bool Override)
    {
        Mapable_Info[,] Layer = new Mapable_Info[ScaleX + 1, ScaleY + 1];

        int LastRoomX = Random.Range(0, ScaleX);
        int LastRoomY = Random.Range(0, ScaleY);

        Mapable_Info _starter = new Mapable_Info(LastRoomX, LastRoomY);
        _starter.IsStarter = true;

        Layer[LastRoomX, LastRoomY] = _starter;

        for (int _room = 0; _room < _RoomNumber; _room++)
        {
            List<RelativePosition> _moves = Moves.ToList();

            //Override means that it doesnt care about if another room is near it
            if (Override)
            {
                if (LastRoomX == ScaleX) { if (_moves.Has(RelativePosition.East)) _moves.Remove(RelativePosition.East); }
                if (LastRoomY == ScaleY) { if (_moves.Has(RelativePosition.North)) _moves.Remove(RelativePosition.North); }
                if (LastRoomX == 0) { if (_moves.Has(RelativePosition.West)) _moves.Remove(RelativePosition.West); }
                if (LastRoomY == 0) { if (_moves.Has(RelativePosition.South)) _moves.Remove(RelativePosition.South); }
            }
            else
            {
                if (LastRoomX == ScaleX - 1 || (LastRoomX == ScaleX - 1 && Layer[LastRoomX + 1, LastRoomY] != null))
                {
                    _moves.Remove(RelativePosition.East);
                }
                if (LastRoomX == 1 || (LastRoomX == 1 && Layer[LastRoomX - 1, LastRoomY] != null))
                {
                    _moves.Remove(RelativePosition.West); ;
                }
                if (LastRoomY == ScaleY - 1 || (LastRoomY == ScaleY - 1 && Layer[LastRoomX, LastRoomY + 1] != null))
                {
                    _moves.Remove(RelativePosition.North);
                }
                if (LastRoomY == 1 || (LastRoomY == 1 && Layer[LastRoomX, LastRoomY - 1] != null))
                {
                    _moves.Remove(RelativePosition.South);
                }
            }

            if (_moves.Count == 0)
            {
                Mapable_Info _platform = new Mapable_Info(LastRoomX, LastRoomY);
                _platform.IsPlatform = true;

                Layer[LastRoomX, LastRoomY] = _platform;
                break;
            }
            else
            {
                RelativePosition Direction = _moves[Random.Range(0, _moves.Count)];

                if (Direction == RelativePosition.North) LastRoomY++;
                if (Direction == RelativePosition.South) LastRoomY--;
                if (Direction == RelativePosition.East) LastRoomX++;
                if (Direction == RelativePosition.West) LastRoomX--;

                Layer[LastRoomX, LastRoomY] = new Mapable_Info(LastRoomY, LastRoomX);
            
            
                if (LastRoomX == _starter.Map_X && LastRoomY == _starter.Map_Y)
                {
                    print("Over");
                }
            }
        }

        return Layer;
    }
}
