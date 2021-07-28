﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public bool[] traversable = new bool[4];

    public BoxCollider[] jumpTriggers = new BoxCollider[4];

    public Vector3[] jumpLandings = new Vector3[4];

    public BoxCollider[] colliders = new BoxCollider[5];


    // Start is called before the first frame update
    void Start()
    {
        jumpTriggers = transform.GetChild(0).GetComponents<BoxCollider>();
        Debug.Log(jumpTriggers.Length);
        Debug.Log(jumpTriggers[3].center);
        colliders = GetComponents<BoxCollider>();


        for (int i = 0; i < traversable.Length; i++)
        {
            if (!traversable[i])
            {
                colliders[i].enabled = true;

                RaycastHit hit;

                Debug.DrawRay(transform.parent.position, colliders[i].center - transform.up);

                if(Physics.Raycast(transform.parent.position, colliders[i].center - transform.up, out hit, 2))
                {
                    if (hit.distance > 1)
                    {
                        jumpTriggers[i].enabled = true;
                        jumpLandings[i] = hit.transform.position + transform.up;
                    }
                }



            }
        }
    }

    // Update is called once per frame
    public void BlockUpdate()
    {
        for (int i = 0; i < traversable.Length; i++)
        {
            if (!traversable[i])
            {
                colliders[i].enabled = true;

                RaycastHit hit;

                if (Physics.Raycast(transform.parent.position, colliders[i].center, out hit, 2))
                {
                    if (hit.distance > 1)
                    {
                        jumpTriggers[i].enabled = true;
                    }
                }



            }
        }
    }
}
