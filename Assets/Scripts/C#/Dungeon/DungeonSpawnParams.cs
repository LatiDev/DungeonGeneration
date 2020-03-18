using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DungeonSpawnParams : MonoBehaviour
{
    public int Rooms = 10;
    public int Layer = 1;

    public InputField Rooms_Input;
    public InputField Layer_Input;

    public Button Apply_B;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        Apply_B.onClick.AddListener(delegate
        {
            this.Apply(Convert.ToInt32(Rooms_Input.text), Convert.ToInt32(Layer_Input.text));
        });
            
    
    }

    public void Apply(int r, int l)
    {
        this.Rooms = r;
        this.Layer = l;

        SceneManager.LoadScene("SampleScene");
    }
}
