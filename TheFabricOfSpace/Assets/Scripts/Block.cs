using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    // Shows whether a direction is traverable
    public bool[] traversable = new bool[4];

    // Triggers that determine whether a player can jump in a direction
    public BoxCollider[] jumpTriggers = new BoxCollider[4];

    // End positions of the jumps
    public Vector3[] jumpLandings = new Vector3[4];

    // Colliders that incase each block and prevent passage
    public BoxCollider[] colliders = new BoxCollider[5];


    private void Start()
    {
        Debug.DrawRay(transform.position, transform.up, Color.green, 6.0f);
        //gameObject.layer = 2;
        if (!Physics.Raycast(transform.position, transform.up, 1.0f))
        {
            transform.parent.gameObject.layer = 2;
        }
    }

    void Awake()
    {
        // Sets the Jump triggers and colliders
        jumpTriggers = transform.GetChild(0).GetComponents<BoxCollider>();
        colliders = GetComponents<BoxCollider>();

        // Loop through each direction and check, if the jump triggers and/or the colliders should be active
        for (int i = 0; i < traversable.Length; i++)
        {

            RaycastHit hit;
            Vector3 dir;
            dir.x = (transform.localToWorldMatrix * colliders[i].center).x - transform.up.x;
            dir.y = (transform.localToWorldMatrix * colliders[i].center).y - transform.up.y;
            dir.z = (transform.localToWorldMatrix * colliders[i].center).z - transform.up.z;
            //Debug.DrawRay(transform.position, dir, Color.red, 2.0f);

            if (Physics.Raycast(transform.position, dir, out hit, 2.0f))
            {
                if (hit.distance > 1)
                {
                    // Hit distance is greater than 1 so the player can jump
                    jumpTriggers[i].enabled = true;
                    jumpLandings[i] = hit.transform.position + transform.up;
                    traversable[i] = false;
                    colliders[i].enabled = true;
                }
                else if (hit.transform.gameObject.tag == "Block" || hit.transform.gameObject.tag == "Slope Upper")
                {
                    // Hit a block adjacent to current block so disable colliders
                    traversable[i] = true;
                    colliders[i].enabled = false;
                    jumpTriggers[i].enabled = false;
                }
                else
                {
                    // Hit something adjacent other than a block, enable colliders
                    traversable[i] = false;
                    colliders[i].enabled = true;
                    jumpTriggers[i].enabled = false;
                }
            }
            else
            {
                // Hit nothing, enable colliders
                jumpTriggers[i].enabled = false;
                traversable[i] = false;
                colliders[i].enabled = true;
            }
        }
    }

    // BockUpdate is called when conditions near a block changes, for example slab sheep activation
    // Checks all the things that Awake checks
    public void BlockUpdate()
    {
        for (int i = 0; i < traversable.Length; i++)
        {

            RaycastHit hit;
            Vector3 dir;
            dir.x = (transform.localToWorldMatrix * colliders[i].center).x - transform.up.x;
            dir.y = (transform.localToWorldMatrix * colliders[i].center).y - transform.up.y;
            dir.z = (transform.localToWorldMatrix * colliders[i].center).z - transform.up.z;
            //Debug.DrawRay(transform.position, dir, Color.red, 2.0f);
            

            if (Physics.Raycast(transform.position, dir, out hit, 2.0f))
            {
                Debug.Log(transform.parent.name);
                Debug.Log(hit.transform.name);
                if (hit.distance > 1.3f)
                {
                    jumpTriggers[i].enabled = true;
                    jumpLandings[i] = hit.transform.position + transform.up;
                    traversable[i] = false;
                    colliders[i].enabled = true;
                }
                else
                {
                    traversable[i] = true;
                    colliders[i].enabled = false;
                    jumpTriggers[i].enabled = false;
                }
            }
            else
            {
                traversable[i] = false;
                colliders[i].enabled = true;
                jumpTriggers[i].enabled = false;
            }
        }
    }
}
