#define DEBUG

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GenerateMapable
{
    public Mapable_Info[,] Generate<T>(int ScaleX, int ScaleY, int RoomNumber, RelativePosition[] Moves, bool Override)
    {
        Mapable_Info[,] Layer = new Mapable_Info[ScaleX, ScaleY];
        
        
        int LastRoomX = Random.Range(0, ScaleX);
        int LastRoomY = Random.Range(0, ScaleY);

        Layer[LastRoomX, LastRoomY] = new Mapable_Info(LastRoomX, LastRoomY);

        

        for (int _room = 0; _room < RoomNumber; _room++)
        {
#if DEBUG
            Debug.Log($"NEXT ROOM: {LastRoomX}, {LastRoomY}");
#endif
            List<RelativePosition> _moves = Moves.ToList();

            //Override means that it doesnt care about if another room is near it
            if (Override)
            {
                if (LastRoomX == ScaleX)    { if (_moves.Has(RelativePosition.East))    _moves.Remove(RelativePosition.East); }
                if (LastRoomY == ScaleY)    { if (_moves.Has(RelativePosition.North))   _moves.Remove(RelativePosition.North); }
                if (LastRoomX == 0)         { if (_moves.Has(RelativePosition.West))    _moves.Remove(RelativePosition.West); }
                if (LastRoomY == 0)         { if (_moves.Has(RelativePosition.South))   _moves.Remove(RelativePosition.South); }
            }
            else
            {
#if DEBUG
                Debug.Log($"East: {LastRoomX == ScaleX}");
#endif
                if (LastRoomX == ScaleX || Layer[LastRoomX + 1, LastRoomY] != null) 
                {
#if DEBUG
                    Debug.Log("Removed East");
#endif
                    if (_moves.Has(RelativePosition.East))    
                        _moves.Remove(RelativePosition.East); 
                }
#if DEBUG
                Debug.Log($"North: {LastRoomY == ScaleY}");
#endif
                if (LastRoomY == ScaleY || Layer[LastRoomX, LastRoomY + 1] != null) 
                {
#if DEBUG
                    Debug.Log("Removed North");
#endif
                    if (_moves.Has(RelativePosition.North))  
                        _moves.Remove(RelativePosition.North); 
                }
#if DEBUG
                Debug.Log($"West: {LastRoomX == 0}");
#endif
                if (LastRoomX == 0 || Layer[LastRoomX - 1, LastRoomY] != null) 
                {
#if DEBUG
                    Debug.Log("Removed West");
#endif
                    if (_moves.Has(RelativePosition.West))
                        _moves.Remove(RelativePosition.West);
                }
#if DEBUG
                Debug.Log($"South: {LastRoomY - 1 > 0}");
#endif
                if (LastRoomY == 0 || Layer[LastRoomX, LastRoomY - 1] != null) 
                {
#if DEBUG
                    Debug.Log("Removed South");
#endif
                    if (_moves.Has(RelativePosition.South))
                        _moves.Remove(RelativePosition.South);
                }
            }
            
            if (_moves.Count == 0)
            {
                break;
            }
            else
            {
                RelativePosition Direction = _moves[Random.Range(0, _moves.Count)];

                if (Direction == RelativePosition.North)
                {
#if DEBUG
                    Debug.Log($"Chosse North");
#endif
                    LastRoomY++;
                }
                if (Direction == RelativePosition.South)
                {
#if DEBUG
                    Debug.Log($"Chosse South");
#endif
                    LastRoomY--;
                }
                if (Direction == RelativePosition.East)
                {
#if DEBUG
                    Debug.Log($"Chosse East");
#endif
                    LastRoomX++;
                }
                if (Direction == RelativePosition.West)
                {
#if DEBUG
                    Debug.Log($"Chosse West");
#endif
                    LastRoomX--;
                }

#if DEBUG
                Debug.Log($"Room: {LastRoomX}, {LastRoomY}");
#endif
                try
                {
                    Layer[LastRoomX, LastRoomY] = new Mapable_Info(LastRoomY, LastRoomX);
                }
                catch
                {
                    int i = 0;
                }
            }
        }

        return Layer;
    }
}
