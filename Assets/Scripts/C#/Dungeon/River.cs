using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class River
{
    public List<RiverData[,]> CreateRiver(int ScaleX, int ScaleY, int layers, int rivermax, int rivermaxlength) 
    {
        List<RiverData[,]> Data = new List<RiverData[,]>();
        for (int i = 0; i < layers; i++)
        {
            RiverData[,] CurrentRiver = new RiverData[ScaleX + 1, ScaleY + 1];

            //int rivercount = Random.Range(0, rivermax);
            int rivercount = rivermax;
            for (int r = 0; r < rivercount; r++)
            {
                int StarterX = Random.Range(0, ScaleX);
                int StarterY = Random.Range(0, ScaleY);

                RiverData Starter = new RiverData();
                this.SetupRandomBlock(Starter);
                //Debug.Log($"Starter:{Starter}");
                CurrentRiver[StarterX, StarterY] = Starter;


                int LastX = StarterX;
                int LastY = StarterY;
                RiverData LastRiverData = Starter;

                //int riverlength = Random.Range(0, rivermaxlength);
                int riverlength = rivermaxlength;
                for (int b = 0; b < riverlength; b++)
                {

                    List<Vector3> dirs = new List<Vector3>
                    { Vector3.right, Vector3.forward,
                    Vector3.left, Vector3.back };


                    if (LastX == ScaleX)
                    {
                        if (dirs.IndexOf(Vector3.right) != -1)
                        {
                            dirs.Remove(Vector3.right);
                        }
                    }

                    if (LastY == ScaleY)
                    {
                        if (dirs.IndexOf(Vector3.forward) != -1)
                        {
                            dirs.Remove(Vector3.forward);
                        }
                    }
                    if (LastX == 0)
                    {
                        if (dirs.Has(Vector3.left))
                        {
                            dirs.Remove(Vector3.left);
                        }
                    }
                    if (LastY == 0)
                    {
                        if (dirs.IndexOf(Vector3.back) != -1)
                        {
                            dirs.Remove(Vector3.back);
                        }
                    }

                    if (dirs.Count == 0)
                    {
                        break;
                    }
                    else
                    {
                        Vector3 Direction = dirs[Random.Range(0, dirs.Count)];

                      
                        if (Direction == Vector3.right) 
                        { 
                            LastX++;
                        }
                        else if (Direction == Vector3.forward) 
                        { 
                            LastY++;
                        }
                        else if (Direction == Vector3.left) 
                        { 
                            LastX--;
                        }
                        else if (Direction == Vector3.back) 
                        { 
                            LastY--;
                        }

                        RiverData RiverData = new RiverData();
                        this.SetupBlock(RiverData, LastRiverData);

                        CurrentRiver[LastX, LastY] = RiverData;

                        Data.Add(CurrentRiver);

                        LastRiverData = RiverData;
                    }
                }
            }


        }
        Debug.Log(Data[0].Length);
        return Data;
    }
    private void SetupRandomBlock(RiverData r)
    {
        List<GameObject> Rivers = GameManager.WorldSetting.Rivers;

        GameObject rdb = Rivers[0];
        r.Setup(rdb);
    }
    private GameObject GetRandomBlock()
    {
        List<GameObject> r = GameManager.WorldSetting.Rivers;
        return r[Random.Range(0, r.Count)];
    }
    private void SetupBlock(RiverData r, RiverData Last)
    {
        List<GameObject> GoodOne = new List<GameObject>();
        foreach (GameObject r_ in GameManager.WorldSetting.Rivers)
        {
            RiverDataBlock _r = r_.GetComponent<RiverDataBlock>();            
            if (_r.Before == Last.DataBlock.After)
            {
                GoodOne.Add(r_);
            }
        }

        GameObject random_river = default(GameObject);
        if (GoodOne.Count == 0)
        {
            random_river = this.GetRandomBlock();
        }
        else
        {
            random_river = GoodOne[Random.Range(0, GoodOne.Count)];
        }
        r.Setup(random_river);
    }
}
