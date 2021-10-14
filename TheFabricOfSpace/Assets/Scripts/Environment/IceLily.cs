using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceLily : MonoBehaviour
{
    public bool walkedOn = false;

    void Awake()
    {
        GetComponents<BoxCollider>()[1].enabled = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (walkedOn && !other.isTrigger)
        {
            DestroyIceLily();
        }
    }

    void DestroyIceLily()
    {
        gameObject.layer = 4;
        GetComponents<BoxCollider>()[1].enabled = false;
        Vector3[] directions = new Vector3[4];

        directions[0] = transform.forward;
        directions[1] = transform.right;
        directions[2] = -transform.forward;
        directions[3] = -transform.right;

        RaycastHit hit;

        for (int i = 0; i < 4; i++)
        {
            if (Physics.Raycast(transform.position - transform.up * 0.4f, directions[i], out hit, 1.0f, 1))
            {
                if (hit.transform.tag == "Block" || hit.transform.tag == "Sheep" || hit.transform.tag == "Water")
                {
                    hit.transform.GetComponentInChildren<Block>().BlockUpdate();
                }
            }
        }
        Destroy(this);
    }
}