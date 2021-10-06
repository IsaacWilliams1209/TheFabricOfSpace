using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballSheep : MonoBehaviour
{
    bool currentlyMoving;
    Vector3 direction;


    private void Update()
    {
        if (!currentlyMoving)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                direction = transform.parent.forward;
                currentlyMoving = true;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                direction = -transform.parent.right;
                currentlyMoving = true;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                direction = -transform.parent.forward;
                currentlyMoving = true;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                direction = transform.parent.right;
                currentlyMoving = true;
            }
        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + (transform.up * 0.45f), direction, 0.5f))
            {
                if (Physics.Raycast(transform.position + (transform.up * 0.45f), direction - (transform.up * 0.45f), out hit, 1.0f, 1 << 4))
                {
                    hit.transform.gameObject.layer = 0;
                    hit.transform.GetComponentInChildren<Block>().BlockUpdate();
                }
                else
                {

                }
                currentlyMoving = false;
            }
            else
                transform.position += GetComponent<SheepController>().speed * direction * Time.deltaTime;
        }
    }

}
