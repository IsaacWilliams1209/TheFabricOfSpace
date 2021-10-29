using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldTree : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fall(Vector3 direction)
    {
        Vector3[] directions = new Vector3[4];

        directions[0] = transform.forward;
        directions[1] = transform.right;
        directions[2] = -transform.forward;
        directions[3] = -transform.right;
     
        for (int i = 0; i< 4; i++)
        {
            Debug.DrawRay(transform.position + transform.up, directions[i], Color.red, 5.0f);
            RaycastHit hit;
            if (Physics.Raycast(transform.position + transform.up, directions[i], out hit, 2.0f, 1))
            {
                Debug.Log(hit.transform.name);
                if (hit.transform.tag == "Block" || hit.transform.tag == "Sheep")
                {
                    hit.transform.GetComponentInChildren<Block>().BlockUpdate();
                }
            }
        }
        

        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(true);
    }
}
