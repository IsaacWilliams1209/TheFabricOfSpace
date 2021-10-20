using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Geyser : MonoBehaviour
{
    [HideInInspector]
    public Sheep sheep;

    Block blockLow;
    Block blockHigh;

    GameObject platform;

    Vector3 to;

    Vector3 from;

    float timer;

    bool isMoving = false;

    bool active = false;

    // Start is called before the first frame update
    void Start()
    {
        blockLow = transform.GetChild(0).GetChild(1).GetComponent<Block>();
        blockHigh = transform.GetChild(0).GetChild(2).GetComponent<Block>();
        platform = transform.GetChild(0).GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (sheep != null)
        {
            if (sheep.sheepType == SheepType.Slab && sheep.poweredUp)
            {
                if (!(isMoving || active))
                {
                    isMoving = true;
                    for (int i = 0; i < 4; i++)
                    {
                        blockLow.traversable[i] = false;
                        blockLow.colliders[i].enabled = true;
                    }
                    // Play animation
                    transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;
                    to = platform.transform.position + transform.up;
                    from = platform.transform.position;
                }
            }
            else if (active && !isMoving)
            {
                isMoving = true;
                GetComponent<BoxCollider>().enabled = false;
                to = platform.transform.position - transform.up;
                from = platform.transform.position;
                blockLow.BlockUpdate();
                blockHigh.gameObject.SetActive(false);
            }
        }
        else if (active && !isMoving)
        {
            Debug.Log("Sheep is null");
            isMoving = true;
            GetComponent<BoxCollider>().enabled = false;
            to = platform.transform.position - transform.up;
            from = platform.transform.position;
            blockLow.BlockUpdate();
        }
        if (active /* and animation is complete*/)
        {
            transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
            transform.GetChild(0).GetChild(1).gameObject.GetComponent<Block>().BlockUpdate();
            GetComponent<BoxCollider>().enabled = true;
            RaycastHit[] hits = new RaycastHit[4];
            Vector3[] directions = new Vector3[4];
            directions[0] = platform.transform.forward;
            directions[1] = platform.transform.right;
            directions[2] = -platform.transform.forward;
            directions[3] = -platform.transform.right;
            for (int i = 0; i < 4; i++)
            {
                transform.GetChild(0).GetChild(0).gameObject.layer = 2;
                //Debug.DrawRay(transform.position + transform.up, directions[i], Color.blue, 6.0f);
                if (Physics.Raycast(transform.position + transform.up, directions[i], out hits[i], 2.0f))
                {
                    Debug.Log(hits[i].transform.name);
                    if (hits[i].transform.tag == "Block" || hits[i].transform.tag == "Sheep")
                    {
                        transform.GetChild(0).GetChild(0).gameObject.layer = 0;
                        // Update nearby blocks
                        hits[i].transform.GetComponentInChildren<Block>().BlockUpdate();
                    }

                }
            }
        }
        if (isMoving)
        {
            // timer = animationTime/ (animationTime - Time.deltaTime);
            timer += Time.deltaTime;
            GetComponent<BoxCollider>().enabled = false;

            platform.transform.position = Vector3.Lerp(from, to, timer);
            if (platform.transform.position == to)
            {
                isMoving = false;
                active = !active;
                timer = 0;
            }
        }
    }
}
