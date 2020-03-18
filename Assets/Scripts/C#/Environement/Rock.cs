using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public GameObject Rock_;
    public GameObject Ore;

    private float X;
    public int OreIndex;

    private void Start()
    {
        /*

        WorldPalet p = WorldSetting.Singleton.WorldPalet;

        this.Rock_.GetComponent<Renderer>().material.color = p.RockColor;
    
        */
    }
    private void Update()
    {
        /*

        WorldPalet p = WorldSetting.Singleton.WorldPalet;
        Color ActualColor = p.RockSecondColor[this.OreIndex];

        if (p.WaveColor)
        {
            X++;

            float y = p.ColorMultiplier * Mathf.Abs(Mathf.Sin(X / p.ColorFrequency)) + p.ColorYOffest;

            float r = ActualColor.r * y;
            float g = ActualColor.g * y;
            float b = ActualColor.b * y;

            Color c = new Color(r, g, b, p.ColorIntensity);

            this.Ore.GetComponent<Renderer>().material.color = c;
            this.Ore.GetComponent<Renderer>().material.SetColor("_EmissionColor", c);
        }
        else
        {
            this.Ore.GetComponent<Renderer>().material.color = ActualColor;
            this.Ore.GetComponent<Renderer>().material.SetColor("_EmissionColor", ActualColor);
        }

        */
    }
}
