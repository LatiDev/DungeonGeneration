using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public Transform Player;
    void Update()
    {
        this.transform.position = Player.transform.position;
    }
}
