using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballSheep : MonoBehaviour
{
    bool currentlyMoving;
    Vector3 direction;

    Sheep sheep;

    void Start()
    {
        sheep = GetComponent<Sheep>();
    }

    private void Update()
    {
        if (!currentlyMoving && sheep.active)
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
            if (Physics.Raycast(transform.position + (transform.up * 0.45f), direction, out hit, 0.5f) && !hit.collider.isTrigger)
            {
                if (hit.transform.tag == "Tree")
                {
                    hit.transform.parent.GetComponent<OldTree>().Fall(direction);
                    sheep.sheepType = SheepType.Sheared;
                    sheep.berryIndex = -1;
                    Destroy(this);
                }
                if (hit.transform.tag == "Sheep")
                {
                    sheep.sheepType = SheepType.Sheared;
                    sheep.berryIndex = -1;
                    Destroy(this);
                }
                if (Physics.Raycast(transform.position + (transform.up * 0.45f), direction - (transform.up * 0.45f), out hit, 1.5f, 1 << 4))
                {
                    hit.transform.gameObject.AddComponent<IceLily>();
                    hit.transform.gameObject.layer = 0;
                    hit.transform.GetChild(0).GetComponent<Block>().BlockUpdate();

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
                                Debug.DrawRay(origin, directions[i] * 2, Color.red, 2.0f);
                                // Update nearby blocks                                
                                hit.transform.GetComponentInChildren<Block>().BlockUpdate();
                                Debug.Log(hit.transform.name);
                            }
                        }
                    }
                }
                else
                {
                    currentlyMoving = false;
                }                
            }
            else
                transform.position += GetComponent<SheepController>().speed * direction * Time.deltaTime;
        }
    }
}
