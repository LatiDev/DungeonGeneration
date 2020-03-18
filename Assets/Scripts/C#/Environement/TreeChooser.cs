using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeChooser : MonoBehaviour
{
    public int Index;

    private void Start()
    {
        if (Index == 0)
            Index = Random.Range(0, this.transform.childCount);

        for (int c = 0; c < this.transform.childCount; c++)
        {
            if (c != Index)
            {
                this.transform.GetChild(c).gameObject.SetActive(false);
            }
        }
    }
}
