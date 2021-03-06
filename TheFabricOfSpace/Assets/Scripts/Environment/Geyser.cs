using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Geyser : MonoBehaviour
{
    [HideInInspector]
    public Sheep sheep;

    Vector3[] debugPoints = new Vector3[4];

    Block blockLow;
    Block blockHigh;

    GameObject platform;

    GameObject inputSpout;

    Vector3 to;

    Vector3 from;

    float timer;

    bool isMoving = false;

    bool active = false;

    bool testBool = true;

    bool down = false;

    // Start is called before the first frame update
    void Start()
    {
        blockLow = transform.GetChild(0).GetChild(1).GetComponent<Block>();
        blockHigh = transform.GetChild(0).GetChild(2).GetComponent<Block>();
        platform = transform.GetChild(0).GetChild(0).gameObject;
        inputSpout = transform.GetChild(1).GetChild(1).gameObject;
        blockHigh.gameObject.SetActive(false);
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
                    for (int i =0; i < 4; i++)
                    {
                        blockLow.colliders[i].enabled = false;
                    }

                    // Play animation
                    transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
                    to = platform.transform.position + transform.up;
                    from = platform.transform.position;
                    down = true;
                }
            }
            else if (active && !isMoving)
            {
                transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
                isMoving = true;
                transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;
                blockHigh.gameObject.SetActive(false);
                to = platform.transform.position - transform.up;
                from = platform.transform.position;
                down = false;
                blockLow.BlockUpdate();
            }
        }
        else if (active && !isMoving)
        {
            Debug.Log("Sheep is null");
            transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
            testBool = true;
            isMoving = true;
            blockHigh.gameObject.SetActive(false);
            transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;
            to = platform.transform.position - transform.up;
            from = platform.transform.position;
            down = false;
            blockLow.BlockUpdate();
        }
        if (active  && testBool/* and animation is complete*/)
        {            
            testBool = false;
            transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
            Debug.Log("Block high set active");
            blockHigh.gameObject.SetActive(true);
            transform.GetChild(0).gameObject.layer = 2;
            blockHigh.gameObject.GetComponent<Block>().BlockUpdate();
            transform.GetChild(0).gameObject.layer = 0;
            RaycastHit[] hits = new RaycastHit[4];
            Vector3[] directions = new Vector3[4];
            directions[0] = platform.transform.forward;
            directions[1] = platform.transform.right;
            directions[2] = -platform.transform.forward;
            directions[3] = -platform.transform.right;
            
            transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
            transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            
            for (int i = 0; i < 4; i++)
            {
                transform.GetChild(0).GetChild(0).gameObject.layer = 2;
                Debug.DrawRay(transform.GetChild(0).position + transform.up, directions[i], Color.blue, 6.0f);
                transform.GetChild(0).gameObject.layer = 2;
                if (Physics.Raycast(transform.GetChild(0).position + transform.up, directions[i], out hits[i], 2.0f))
                {
                    transform.GetChild(0).gameObject.layer = 0;
                    Debug.Log(hits[i].transform.name);
                    if (hits[i].transform.tag == "Block")
                    {
                        Debug.Log(hits[i].transform.parent.name);
                        transform.GetChild(0).GetChild(0).gameObject.layer = 0;
                        // Update nearby blocks
                        hits[i].transform.GetComponentInChildren<Block>().BlockUpdateDebug();
                        debugPoints[i] = hits[i].point;
                    }
                    else if (hits[i].transform.tag == "Geyser")
                    {
                        transform.GetChild(0).GetChild(0).gameObject.layer = 0;
                        // Update nearby blocks
                        Block[] blocks = hits[i].transform.GetComponentsInChildren<Block>(); 
                        foreach (Block block in blocks)
                        {
                            block.BlockUpdateDebug();
                        }
                        
                    }
                    else
                    {
                        //try
                        //{
                        //
                        //}
                    }
                    debugPoints[i] = hits[i].point;
                }
            }
            
        }
        if (isMoving)
        {
            // timer = animationTime/ (animationTime - Time.deltaTime);
            timer += Time.deltaTime;
            if (down)
            {
                inputSpout.transform.position = Vector3.Lerp(inputSpout.transform.parent.position, inputSpout.transform.parent.position - transform.up, timer);
            }                
            else
            {
                inputSpout.transform.position = Vector3.Lerp(inputSpout.transform.parent.position - transform.up, inputSpout.transform.parent.position, timer);
            }                
            platform.transform.position = Vector3.Lerp(from, to, timer);
            if (platform.transform.position == to)
            {
                isMoving = false;
                testBool = true;
                active = !active;
                timer = 0;
            }
        }
    }

    void OnDrawGizmos()
    {
        if (debugPoints[0] != null)
        {
            Gizmos.color = Color.red;
            foreach (Vector3 point in debugPoints)
            {
                Gizmos.DrawCube(point, new Vector3(0.1f, 0.1f, 0.1f));
            }
        }      

    }

}
