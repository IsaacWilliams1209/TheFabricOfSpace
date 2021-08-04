using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour
{

    public float speed;

    public Material[] sheepMaterials = new Material[3];

    [HideInInspector]
    public int index;

    public bool voxel = false;


    public bool poweredUp = false;

    List<GameObject> awakeSheep = new List<GameObject>();

    GameObject[] sheep = new GameObject[1];

    [SerializeField]
    bool awake = false;

    [SerializeField]
    bool active = false;

    bool swap = false;

    public bool canJump;

    bool isJumping;

    bool canMove = true;

    int berryIndex = -1;
    
    Shepherd shepherd;

    Renderer matChanger;

    float jumpTime = 0;

    Vector3 jumpLanding;
    int jumpIndex;

    Vector3[] jumpFrames = new Vector3[1];
    CharacterController controller;
    // Start is called before the first frame update
    void Start()
    {
        shepherd = transform.parent.GetComponent<Shepherd>();
        sheep = shepherd.sheep;
        awakeSheep = shepherd.awakeSheep;
        matChanger = GetComponent<Renderer>();
        controller = GetComponent<CharacterController>();

        if (active)
        {
            matChanger.material = sheepMaterials[0];
            shepherd.activeSheep = gameObject;
        }
        else if (awake)
        {
            matChanger.material = sheepMaterials[1];
            awakeSheep.Add(gameObject);
        }
        else
        {
            matChanger.material = sheepMaterials[2];
        }
        if (voxel)
        {
            berryIndex = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if (canMove)
            {
                Vector3 movement = Vector3.zero;

                movement += transform.parent.forward * Input.GetAxis("Vertical") * speed * Time.deltaTime;
                movement += transform.parent.right * Input.GetAxis("Horizontal") * speed * Time.deltaTime;
                RaycastHit hit;
                if (!Physics.Raycast(transform.position, -transform.parent.up, out hit, 1.0f))
                {
                    movement += transform.parent.up * Physics.gravity.y * Time.deltaTime;
                }
                else if (hit.distance < 0.4f && !hit.collider.isTrigger)
                {
                    movement += transform.parent.up * 0.01f;


                }
                else if (hit.distance > 0.5f && !hit.collider.isTrigger)
                {
                    movement -= transform.parent.up * 0.01f;

                }
                else if (hit.transform.tag == "Sheep" && hit.transform.GetComponent<Sheep>().voxel)
                {
                    hit.transform.GetComponent<Sheep>().poweredUp = true;
                }


                controller.Move(movement);
            }
            else
            {
                if (isJumping)
                {
                    jumpTime += Time.deltaTime;
                    float percentDone = jumpTime/ 0.15f;
                    transform.position = Vector3.Lerp(jumpFrames[jumpIndex], jumpFrames[jumpIndex + 1], 1);
                    if (transform.position == jumpFrames[jumpIndex + 1])
                    {
                        jumpTime = 0;
                        if (jumpIndex < jumpFrames.Length - 2)
                            jumpIndex++;
                        else
                        {
                            isJumping = false;
                            canMove = true;                            
                            jumpIndex = 0;
                        }
                    }
                }
            }
            if (Input.GetButtonDown("Jump"))
            {
                for (int i = 0; i < sheep.Length; i++)
                {
                    if (i != index && Vector3.SqrMagnitude(sheep[i].transform.position - transform.position) < 4.0f && !sheep[i].GetComponent<Sheep>().awake)
                    {
                        sheep[i].GetComponent<Sheep>().awake = true;
                        sheep[i].GetComponent<Renderer>().material = sheepMaterials[1];
                        awakeSheep.Add(sheep[i]);
                    }
                }
                if (berryIndex != -1)
                {
                    for (int i = 0; i < shepherd.berries.Length; i++)
                    {
                        if (Vector3.SqrMagnitude(shepherd.berries[i].transform.position - transform.position) < 4.0f)
                        {
                            if (shepherd.berries[i].GetComponent<Shrubs>().Eat())
                            {
                                shepherd.berries[i].GetComponent<Shrubs>().GrantPowerUp(gameObject);
                                berryIndex = i;

                            }
                        }
                    }
                }

            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (berryIndex != -1)
                {
                    poweredUp = !poweredUp;
                    ActivatePowerUp();
                }
            }
            if (canJump && Input.GetKeyDown(KeyCode.G) && !voxel)
            {
                DoDaJump();
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                if (awakeSheep.Count != 0)
                {
                    swap = true;
                }
            }

            ///////////////////////////////TEMPORARY CODE ////////////////////
            if (Input.GetKeyDown(KeyCode.T))
            {
                shepherd.SwapCams();
            }



        }
        else if (awake)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -transform.parent.up, out hit, 1.0f))
            {
                if (hit.transform.tag == "Sheep" && hit.transform.GetComponent<Sheep>().voxel)
                {
                    hit.transform.GetComponent<Sheep>().poweredUp = false;
                }
            }

        }
    }

    void LateUpdate()
    {
        if (swap)
        {
            if (shepherd.isSheepFocus)
            {
                transform.GetChild(1).gameObject.SetActive(false);
                awakeSheep[0].transform.GetChild(1).gameObject.SetActive(true);
            }
            
            shepherd.activeSheep = awakeSheep[0];
            awakeSheep[0].GetComponent<Renderer>().material = sheepMaterials[0];
            awakeSheep[0].GetComponent<Sheep>().active = true;
            awakeSheep.RemoveAt(0);
            awakeSheep.Add(gameObject);
            matChanger.material = sheepMaterials[1];
            active = false;
            swap = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Finish")
        {
            // you win! activate world rotation
            Debug.Log("Good Job!");
        }
        if (other.gameObject.tag == "Jump")
        {
            Block block = other.gameObject.GetComponentInParent<Block>();
            canJump = true;
            for (int i = 0; i < 4; i++)
            {
                if(block.jumpTriggers[i].bounds == other.bounds)
                {
                    jumpLanding = block.jumpLandings[i];
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Jump")
            canJump = false;
    }

    private void ActivatePowerUp()
    {
        if (voxel)
        {
            if (poweredUp)
            {
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetComponentInChildren<Block>().BlockUpdate();
                
                canMove = false;                
                Vector3 temp = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z));

                transform.position = temp;
                RaycastHit[] hits = new RaycastHit[4];
                Vector3[] directions = new Vector3[4];
                directions[0] = transform.parent.forward;
                directions[1] = transform.parent.right;
                directions[2] = -transform.parent.forward;
                directions[3] = -transform.parent.right;
                gameObject.layer = 0;
                for (int i = 0; i < hits.Length; i++)
                {
                    if (Physics.Raycast(transform.position, directions[i], out hits[i], 2.0f))
                    {
                        hits[i].transform.GetComponentInChildren<Block>().BlockUpdate();
                    }
                }
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
            else
            {
                gameObject.layer = 2;
                transform.GetChild(0).gameObject.SetActive(false);
                RaycastHit[] hits = new RaycastHit[4];
                Vector3[] directions = new Vector3[4];
                directions[0] = transform.parent.forward;
                directions[1] = transform.parent.right;
                directions[2] = -transform.parent.forward;
                directions[3] = -transform.parent.right;
                for (int i = 0; i < hits.Length; i++)
                {
                    if (Physics.Raycast(transform.position, directions[i], out hits[i], 2.0f))
                    {
                        hits[i].transform.GetComponentInChildren<Block>().BlockUpdate();
                    }
                }                
                canMove = true;
                transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }
        }
    }

    private void DoDaJump()
    {
        canMove = false;
        canJump = false;
        isJumping = true;
        int numFrames = 30;
        jumpFrames = new Vector3[numFrames];

        Vector3 startingPos;
        startingPos.x = MaskVector(transform.position, transform.right);
        startingPos.y = MaskVector(transform.position, transform.up);
        startingPos.z = MaskVector(transform.position, transform.forward);
        
        Vector3 stP = new Vector3(0, startingPos.y, startingPos.z);

        Vector3 arrivingPos;
        arrivingPos.x = MaskVector(jumpLanding, transform.right);
        arrivingPos.y = MaskVector(jumpLanding, transform.up);
        arrivingPos.z = MaskVector(jumpLanding, transform.forward);

        Vector3 arP = new Vector3(0, arrivingPos.y, arrivingPos.z);

        Vector3 diff = ((arP - stP) / 2) + new Vector3(0,1,0);
        Vector3 vertex = stP + diff;

        float x1 = startingPos.z;
        float y1 = startingPos.y;
        float x2 = arrivingPos.z;
        float y2 = arrivingPos.y;
        float x3 = vertex.z;
        float y3 = vertex.y;

        float denom = (x1 - x2) * (x1 - x3) * (x2 - x3);

        var z_dist = (arrivingPos.z - startingPos.z) / numFrames;
        var x_dist = (arrivingPos.x - startingPos.x) / numFrames;

        float A = (x3 * (y2 - y1) + x2 * (y1 - y3) + x1 * (y3 - y2)) / denom;
        float B = (float)(System.Math.Pow(x3, 2) * (y1 - y2) + System.Math.Pow(x2, 2) * (y3 - y1) + System.Math.Pow(x1, 2) * (y2 - y3)) / denom;
        float C = (x2 * x3 * (x2 - x3) * y1 + x3 * x1 * (x3 - x1) * y2 + x1 * x2 * (x1 - x2) * y3) / denom;

        float newX = startingPos.z;
        float newZ = startingPos.x;

        for (int i = 0; i < numFrames; i++)
        {
            newX += z_dist;
            newZ += x_dist;
            float yToBeFound = A * (newX * newX) + B * newX + C;
            Vector3 temp = transform.right * newZ + transform.up * yToBeFound + transform.forward * newX;
           jumpFrames[i] = temp;
        }

    }

    float MaskVector(Vector3 data, Vector3 mask)
    {
        Vector3 temp;
        temp.x = data.x * mask.x;
        temp.y = data.y * mask.y;
        temp.z = data.z * mask.z;
        return temp.x + temp.y + temp.z;
    }

    void OnDrawGizmos()
    {
        if (jumpFrames[0] != null)
        {
            Gizmos.color = Color.yellow;
            Vector3[] points = jumpFrames;
            foreach (Vector3 point in points)
            {
                Gizmos.DrawSphere(point, .1f);
            }
        }
    }
}
