using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Singleton;

    private void Awake() { if (Singleton) { Singleton = this; } }
}
