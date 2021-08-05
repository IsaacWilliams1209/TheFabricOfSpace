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
    void Awake()
    {
        jumpTriggers = transform.GetChild(0).GetComponents<BoxCollider>();
        colliders = GetComponents<BoxCollider>();

        for (int i = 0; i < traversable.Length; i++)
        {

            RaycastHit hit;
            Vector3 dir;
            dir.x = (transform.localToWorldMatrix * colliders[i].center).x - transform.up.x;
            dir.y = (transform.localToWorldMatrix * colliders[i].center).y - transform.up.y;
            dir.z = (transform.localToWorldMatrix * colliders[i].center).z - transform.up.z;
            Debug.DrawRay(transform.position, dir, Color.red, 2.0f);

            if (Physics.Raycast(transform.position, dir, out hit, 2.0f))
            {
                if (hit.distance > 1)
                {
                    jumpTriggers[i].enabled = true;
                    jumpLandings[i] = hit.transform.position + transform.up;
                    traversable[i] = false;
                    colliders[i].enabled = true;
                }
                else if (hit.transform.gameObject.tag == "Block" || hit.transform.gameObject.tag == "Slope Upper")
                {
                    traversable[i] = true;
                    colliders[i].enabled = false;
                    jumpTriggers[i].enabled = false;
                }
                else
                {
                    traversable[i] = false;
                    colliders[i].enabled = true;
                    jumpTriggers[i].enabled = false;
                }
            }
            else
            {
                jumpTriggers[i].enabled = false;
                traversable[i] = false;
                colliders[i].enabled = true;
            }
        }
    }

    // Update is called once per frame
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
                else //if (hit.transform.gameObject.tag == "Block" || hit.transform.gameObject.tag == "Slope Upper")
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
