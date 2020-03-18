using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [Range(0, 3)]
    public float Global_Speed;
    [Range(1, 1000)]
    public float Speed;

    public int IsOnLayer = 0;
    
    public Transform GunHandlePoint;
    public Gun Gun;


    private void Update()
    {
        RaycastHit Hit;
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out Hit);

        Vector3 l = Hit.point;
        l.y = this.transform.position.y;

        this.transform.LookAt(l);

        if (Input.GetKey(KeyCode.UpArrow))
        {
            this.transform.position += (Vector3.forward + Vector3.right) * Global_Speed * Speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            this.transform.position += -(Vector3.forward + Vector3.right) * Global_Speed * Speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.position += (- Vector3.forward + Vector3.right) * Global_Speed * Speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.position += (Vector3.forward - Vector3.right) * Global_Speed * Speed * Time.deltaTime;
        }
    }
}
