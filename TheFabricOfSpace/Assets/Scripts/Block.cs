using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public bool[] traversable = new bool[4];

    public BoxCollider[] jumpTriggers = new BoxCollider[4];

    public Vector3[] jumpLandings = new Vector3[4];

    public BoxCollider[] colliders = new BoxCollider[5];


    // Start is called before the first frame update
    void Update()
    {
        jumpTriggers = transform.GetChild(0).GetComponents<BoxCollider>();
        colliders = GetComponents<BoxCollider>();


        for (int i = 0; i < traversable.Length; i++)
        {
            if (!traversable[i])
            {
                colliders[i].enabled = true;

                RaycastHit hit;
                Vector3 dir;
                dir.x = (transform.localToWorldMatrix * colliders[i].center).x - transform.up.x;
                dir.y = (transform.localToWorldMatrix * colliders[i].center).y - transform.up.y;
                dir.z = (transform.localToWorldMatrix * colliders[i].center).z - transform.up.z;
                Debug.DrawRay(transform.position, dir);

                if(Physics.Raycast(transform.position, dir, out hit, 2))
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
                RaycastHit hit;
                Vector3 dir;
                dir.x = (transform.localToWorldMatrix * colliders[i].center).x - transform.up.x;
                dir.y = (transform.localToWorldMatrix * colliders[i].center).y - transform.up.y;
                dir.z = (transform.localToWorldMatrix * colliders[i].center).z - transform.up.z;

                if (Physics.Raycast(transform.parent.position, dir, out hit, 2))
                {
                    if (hit.distance > 1)
                    {
                        jumpTriggers[i].enabled = true;
                        jumpLandings[i] = hit.transform.position + transform.up;
                        colliders[i].enabled = true;
                        
                    }
                    else
                    {
                        traversable[i] = true;
                        colliders[i].enabled = false;
                    }
                }
            }
        }
    }
}
