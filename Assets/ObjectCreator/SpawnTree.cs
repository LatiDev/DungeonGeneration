using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTree : MonoBehaviour
{
    public GameObject Tree;

    /*

    [Range(1, 100)]
    public int Min_X;
    [Range(1, 100)]
    public int Max_X;

    [Range(1, 100)]
    public int Min_Z;
    [Range(1, 100)]
    public int Max_Z;

    */

    [Range(1, 10)]
    public float X;
    [Range(1, 10)]
    public float Y;

    [Range(0, 1)]
    public float Min;

    private void Start()
    {
        foreach(Transform t in this.transform)
        {
            Destroy(t.gameObject);
        }

        Transform LastPosition = this.transform;
        for (float x = 0; x < X; x += 0.1f)
        {
            LastPosition.position = new Vector3(
                LastPosition.position.x + 8,
                LastPosition.position.y,
                LastPosition.position.z);

            for (float y = 0; y < Y; y += 0.1f)
            {
                LastPosition.position = new Vector3(
                    LastPosition.position.x,
                    LastPosition.position.y,
                    LastPosition.position.z + 8);



                float p = Mathf.PerlinNoise(x, y);
               

                if (p >= Min)
                {
                    GameObject g = Instantiate(Tree);
                    g.transform.position = this.transform.position + new Vector3((x * 10), 0, (y * 10));
                    g.transform.SetParent(this.transform);

                    LastPosition = g.transform;
                }

            }
        }
    }
    /*

    // Start is called before the first frame update
    void Update()
    {
        BoxCollider BC = this.GetComponent<BoxCollider>();

        //BoxCollider T_Scale = Tree.GetComponent<BoxCollider>();
        //T_Scale.size = new Vector3(x_scale_c, 0, z_scale_c);

        float x_scale = Random.Range(Min_X, Max_X);
        float z_scale = Random.Range(Min_Z, Max_Z);

        Vector3 BC_scale = BC.size;
        Vector3 new_Scale = BC_scale - new Vector3(x_scale, 0, z_scale);


        float x_pos = Random.Range(-new_Scale.x, new_Scale.x);
        float z_pos = Random.Range(-new_Scale.z, new_Scale.z);
        Vector3 new_pos = new Vector3(x_pos/2, 0, z_pos/2);


        GameObject Tree_I = Instantiate(Tree);
        Tree_I.transform.position = this.transform.position + new_pos;
        Tree_I.transform.SetParent(this.transform);


        BoxCollider Tree_I_Scale = Tree_I.GetComponent<BoxCollider>();
        Tree_I_Scale.size = new Vector3(x_scale, 0, z_scale);

        //////////////////////////////////////////////////////////////////////
        





    }

    */
}
