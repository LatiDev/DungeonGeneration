using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerController : MonoBehaviour
{
    public Transform Rooms;

    [Range(0, 30)]
    public int Actual_Layer = 0;
    public PlatformRenderingType RenderType = PlatformRenderingType.Each;

    public void Each()
    {
        this.RenderType = PlatformRenderingType.Each;
        this.UpdateLayer(a => a == this.Actual_Layer);
    }
    public void Both()
    {
        this.RenderType = PlatformRenderingType.Both;
        this.UpdateLayer(a => a == this.Actual_Layer || a == this.Actual_Layer+1);
    }
    public void UpdateLayer(Func<int, bool> Func)
    {
        for (int a=0; a < this.Rooms.childCount; a++)
        {
            GameObject g = this.Rooms.GetChild(a).gameObject;
            g.SetActive(false);
        }
        for (int i=0; i < this.Rooms.childCount; i++)
        {
            if (Func(i))
            {
                GameObject g = this.Rooms.GetChild(i).gameObject;
                g.SetActive(true);
            }
        }
    }
}