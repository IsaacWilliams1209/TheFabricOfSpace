using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public bool[] traversable = new bool[4];

    public BoxCollider[] colliders = new BoxCollider[5];

    public GameObject[] neighbours = new GameObject[4];


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < traversable.Length; i++)
        {
            if (!traversable[i])
            {
                colliders[i].enabled = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
