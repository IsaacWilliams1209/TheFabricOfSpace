using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceLily : MonoBehaviour
{
    Sheep sheep;

    bool walkedOn = false;
    // Start is called before the first frame update
    void Start()
    {
        sheep = GetComponent<Sheep>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.up, out hit, 1.5f, 1 << 2))
        {
            
            if (sheep.sheepType != SheepType.Snowball || sheep.sheepType != SheepType.Sheared)
            {
                walkedOn = true;
                Debug.Log("Sheep found");
            }
        }

        if (walkedOn && Physics.Raycast(transform.position, transform.up, out hit, 1.5f, 1 << 2))
        {
            gameObject.layer = 4;
            Vector3[] directions = new Vector3[4];

            directions[0] = transform.forward;
            directions[1] = transform.right;
            directions[2] = -transform.forward;
            directions[3] = -transform.right;

            Vector3 origin = hit.transform.position - transform.up * 0.4f;

            for (int i = 0; i < 4; i++)
            {
                if (Physics.Raycast(origin, directions[i], out hit, 1.0f, 1))
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
}
