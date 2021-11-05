using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballSheep : MonoBehaviour
{
    bool currentlyMoving;
    Vector3 direction;

    Vector3[] debugPoints = new Vector3[4];

    Sheep sheep;

    void Awake()
    {
        sheep = GetComponent<Sheep>();
        gameObject.layer = 2;
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
        else if (currentlyMoving)
        {
            GetComponent<Animator>().SetBool("IsRolling", true);
            RaycastHit hit;

            int mask = (1 << 8) | (1 << 0);
            Debug.DrawRay(transform.position + (transform.up * 0.45f), direction, Color.blue, 5.0f);
            if (Physics.Raycast(transform.position + (transform.up * 0.45f), direction, out hit, 0.5f, mask) && !hit.collider.isTrigger)
            {
                Debug.Log(hit.transform.name);
                debugPoints[0] = hit.point;
                if (hit.transform.tag == "Tree")
                {
                    hit.transform.parent.GetComponent<OldTree>().Fall(direction);
                    sheep.sheepType = SheepType.Sheared;
                    if (!(sheep.berryIndex < 0))
                        sheep.shepherd.berries[sheep.berryIndex].GetComponent<Shrubs>().Restore();
                    sheep.berryIndex = -1;
                    gameObject.layer = 8;
                    Destroy(this);
                }
                if (hit.transform.tag == "Sheep")
                {
                    if (hit.transform.TryGetComponent<StaticSheep>(out StaticSheep staticSheep))
                    {
                        currentlyMoving = false;
                        GetComponent<Animator>().SetBool("IsRolling", false);
                    }
                    else
                    {
                        sheep.sheepType = SheepType.Sheared;
                        if (!(sheep.berryIndex < 0))
                            sheep.shepherd.berries[sheep.berryIndex].GetComponent<Shrubs>().Restore();
                        sheep.berryIndex = -1;
                        gameObject.layer = 8;
                        GetComponent<Animator>().SetBool("IsRolling", false);
                        GetComponent<Animator>().SetBool("IsSnowball", false);
                        Destroy(this);
                    }
                    
                }
                if (Physics.Raycast(transform.position + (transform.up * 0.45f), direction - (transform.up * 0.45f), out hit, 1.5f, 1 << 4))
                {
                    hit.transform.gameObject.AddComponent<IceLily>();
                    try { hit.transform.GetChild(3).gameObject.SetActive(true); }
                    catch { Debug.Log(hit.transform.name); }
                    
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
                            if (hit.transform.tag == "Sheep" || hit.transform.tag == "Water")
                            {
                                Debug.DrawRay(origin, directions[i] * 2, Color.red, 2.0f);
                                // Update nearby blocks                                
                                hit.transform.GetComponentInChildren<Block>().BlockUpdate();
                                Debug.Log(hit.transform.name);
                            }
                            if (hit.transform.tag == "Block" && hit.transform.GetChild(hit.transform.childCount - 1).TryGetComponent<Block>(out Block block))
                            {
                                Debug.DrawRay(origin, directions[i] * 2, Color.red, 2.0f);
                                // Update nearby blocks                                
                                hit.transform.GetComponentInChildren<Block>().BlockUpdateDebug();
                                Debug.Log(hit.transform.name);
                            }
                        }
                    }
                }
                else
                {
                    currentlyMoving = false;
                    GetComponent<Animator>().SetBool("IsRolling", false);
                }                
            }
            else
                transform.position += GetComponent<SheepController>().speed * direction * Time.deltaTime;
        }
    }

    void OnDrawGizmos()
    {
        if (debugPoints[0] != null)
        {
            Gizmos.color = Color.blue;
            foreach (Vector3 point in debugPoints)
            {
                Gizmos.DrawCube(point, new Vector3(0.1f, 0.1f, 0.1f));
            }
        }

    }
}
