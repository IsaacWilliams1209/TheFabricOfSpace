using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour
{
    // Keeps the awake, asleep and active materials for the sheep
    public Material[] sheepMaterials = new Material[3];

    // Sheep index inside the shepherd's sheep array
    [HideInInspector]
    public int index;

    // Is the sheep currently using it's powerup
    public bool poweredUp = false;

    public SheepType sheepType = SheepType.Sheared;

    // List of awake sheep on the face
    List<GameObject> awakeSheep = new List<GameObject>();

    // Array of sheep on the face
    GameObject[] sheep = new GameObject[1];

    GameObject closestSheep;

    // Is the sheep awake
    [SerializeField]
    bool awake = false;

    // Is the Sheep active
    public bool active = false;

    // Will the sheep be swapped this frame
    bool swap = false;

    // Is the sheep able to jump
    public bool canJump;

    public bool canEat;

    public bool canWake;

    // Is the sheep currently jumping
    bool isJumping;

    // Can the sheep move
    [HideInInspector]
    public bool canMove = true;

    // Index of eaten berry in the Shepherd's Berry array, -1 means no berry eaten
    public int berryIndex = -1;



    // Refers to the shepherd of this face
    Shepherd shepherd;

    // Used to cahnge materials for the sheep being awake/asleep/active
    Renderer matChanger;

    // Time taken for the sheep to jump
    float jumpTime = 0;

    // Position the sheep will land when it jumps
    Vector3 jumpLanding;

    // Index into the frames of the jump
    int jumpIndex;

    // Frames containg positions of the jump
    Vector3[] jumpFrames = new Vector3[1];

    // The character controller
    SheepController controller;

    [HideInInspector]
    public BoxCollider mainCollider;

    [HideInInspector]
    public BoxCollider wakingTrigger;

    Mesh defaultMesh;

    [HideInInspector]
    public Animator animator;

    public List<Mesh> meshes = new List<Mesh>();

    // Start is called before the first frame update
    void Start()
    {
        // Initalising variables
        animator = GetComponent<Animator>();
        defaultMesh = transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().sharedMesh;
        shepherd = transform.parent.GetComponent<Shepherd>();
        sheep = shepherd.sheep;
        awakeSheep = shepherd.awakeSheep;
        matChanger = transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();
        controller = GetComponent<SheepController>();
        mainCollider = GetComponents<BoxCollider>()[0];
        wakingTrigger = GetComponents<BoxCollider>()[1];

        // Set apropriate materials for the sheep
        if (active)
        {
            matChanger.materials[2] = sheepMaterials[0];
            shepherd.activeSheep = gameObject;
            wakingTrigger.enabled = false;
            wakingTrigger.enabled = false;
        }
        else if (awake)
        {
            matChanger.materials[2] = sheepMaterials[1];
            awakeSheep.Add(gameObject);
            wakingTrigger.enabled = false;
        }
        else
        {
            matChanger.materials[2] = sheepMaterials[2];
        }
        if (sheepType == SheepType.Slab)
        {
            berryIndex = -2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if (canMove && sheepType != SheepType.Snowball)
            {
                controller.Move();
            }
            else
            {
                // If the player can't move and is jump cycle through the jumpFrames
                if (isJumping)
                {
                    jumpTime += Time.deltaTime;
                    float percentDone = jumpTime * 10;
                    transform.position = Vector3.Lerp(jumpFrames[jumpIndex], jumpFrames[jumpIndex + 1], percentDone);
                    if (transform.position == jumpFrames[jumpIndex + 1])
                    {
                        jumpTime = 0;
                        if (jumpIndex < jumpFrames.Length - 2)
                        {
                            jumpIndex++;
                        }
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
                if (canWake)
                {
                    closestSheep.GetComponent<Sheep>().awake = true;
                    closestSheep.transform.GetChild(1).GetComponent<Renderer>().material = sheepMaterials[0];                    closestSheep.GetComponent<Sheep>().wakingTrigger.enabled = false;
                    awakeSheep.Insert(0, closestSheep);
                    swap = true;
                }
                if (canEat && shepherd.berries[berryIndex].GetComponent<Shrubs>().Eat())
                {
                    shepherd.berries[berryIndex].GetComponent<Shrubs>().GrantPowerUp(gameObject);                    switch (sheepType)
                    {
                        case SheepType.Slab:
                            transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().sharedMesh = meshes[0];
                            break;
                        case SheepType.Snowball:
                            transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().sharedMesh = meshes[2];
                            break;
                        default:
                            break;
                    }
                    poweredUp = false;
                }
            }
            // On R press activate the sheep powerup
            if (Input.GetKeyDown(KeyCode.R))
            {
                poweredUp = !poweredUp;

                switch (sheepType)
                {
                    case SheepType.Slab:
                        GetComponent<SlabSheep>().ActivatePowerUp(this);
                        break;
                    case SheepType.Sheared:
                        if (canJump)
                        {
                            DoDaJump();
                        }
                        break;
                    case SheepType.Snowball:
                        break;
                    case SheepType.Static:
                        break;
                    default:
                        break;
                }
            }
            // On left shift press, swap to the next active sheep
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                if (awakeSheep.Count != 0)
                {
                    swap = true;
                }
            }

            RaycastHit hit;
            if (sheepType != SheepType.Sheared && sheepType != SheepType.Snowball && Physics.Raycast(transform.position + transform.up * 0.3f, -transform.up, out hit, 1.0f, 1))
            {
                Debug.Log(hit.transform.name);
                Debug.DrawRay(transform.position + transform.up * 0.3f, -transform.up, Color.red, 4);
                if (hit.transform.tag == "Water")
                {
                    DestroyIceLily(hit.transform);
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
            // Raycast down to detect if a voxel sheep is below and prevent it from moving with another sheep
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -transform.parent.up, out hit, 1.0f))
            {
                if (hit.transform.tag == "Sheep" && hit.transform.GetComponent<Sheep>().sheepType == SheepType.Slab)
                {
                    hit.transform.GetComponent<Sheep>().poweredUp = false;
                }
            }
        }
    }

    void LateUpdate()
    {
        // Swap to the next sheep
        if (swap)
        {
            if (shepherd.isSheepFocus)
            {
                transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
                awakeSheep[0].transform.GetChild(2).GetChild(1).gameObject.SetActive(true);
            }

            shepherd.activeSheep = awakeSheep[0];
            awakeSheep[0].transform.GetChild(1).GetComponent<Renderer>().material = sheepMaterials[0];
            awakeSheep[0].GetComponent<Sheep>().active = true;
            awakeSheep.RemoveAt(0);
            awakeSheep.Add(gameObject);
            matChanger.material = sheepMaterials[1];
            active = false;
            swap = false;
        }
    }

    // Checks for jump triggers and wini triggers
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Finish")
        {
            Transition transition = other.GetComponent<Transition>();
            //Shepherd tempShepherd = GameObject.Find("Planet Face 6").transform.GetChild(3).GetComponent<Shepherd>();
            transition.Activate();
            return;
            // you win! activate world rotation
            //tempShepherd.awakeSheep[0].GetComponent<Sheep>().active = true;
            //tempShepherd.awakeSheep[0].GetComponent<Renderer>().material = sheepMaterials[0];
            //tempShepherd.activeSheep = tempShepherd.awakeSheep[0];
            //awakeSheep.Insert(0, gameObject);
            //matChanger.material = sheepMaterials[1];
            //active = false;
            //tempShepherd.enabled = true;
            //shepherd.enabled = false;
        }
        if (other.gameObject.tag == "Jump")
        {
            Block block = other.gameObject.GetComponentInParent<Block>();
            canJump = true;
            for (int i = 0; i < 4; i++)
            {
                if (block.jumpTriggers[i].bounds == other.bounds)
                {
                    jumpLanding = block.jumpLandings[i];
                }
            }
        }
        if (other.gameObject.tag == "Geyser")
        {
            Debug.Log("Geyser triggered");
            other.transform.parent.GetComponent<Geyser>().sheep = this;
        }
        if (other.gameObject.tag == "Sheep" && !other.GetComponent<Sheep>().awake)
        {
            canWake = true;

            closestSheep = other.gameObject;
        }
        if (other.gameObject.tag == "Reg")
        {
            canEat = true;

            berryIndex = other.GetComponent<Shrubs>().index;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Remove jump ability when leaving the trigger
        if (other.gameObject.tag == "Jump")
            canJump = false;

        if (other.gameObject.tag == "Geyser")
            other.transform.parent.GetComponent<Geyser>().sheep = null;

        if (other.gameObject.tag == "Sheep")
        {
            canWake = false;

            closestSheep = null;
        }

        if (other.gameObject.tag == "Reg")
        {
            canEat = false;

            if (sheepType != SheepType.Slab)
            {

                berryIndex = -1;

            }
        }
    }

    // Calculates the frames the jump
    private void DoDaJump()
    {
        // Disable movement and jump ability

        canMove = false;

        canJump = false;

        // Activate jumping bool
        isJumping = true;

        /////////////////////////////////   REPLACE WITH ACTUAL FRAME COUNT ///////////////////////////

        int numFrames = 30;

        jumpFrames = new Vector3[numFrames];

        // Convert position from forward, up and right, to x,y,z

        Vector3 startingPos;

        startingPos.x = MaskVectorAsFloat(transform.position, transform.parent.right);
        startingPos.y = MaskVectorAsFloat(transform.position, transform.parent.up);
        startingPos.z = MaskVectorAsFloat(transform.position, transform.parent.forward);

        // Lock to z-axis
        Vector3 stP = new Vector3(0, startingPos.y, startingPos.z);

        // Convert position from forward, up and right, to x,y,z
        Vector3 arrivingPos;

        arrivingPos.x = MaskVectorAsFloat(jumpLanding, transform.parent.right);
        arrivingPos.y = MaskVectorAsFloat(jumpLanding, transform.parent.up);
        arrivingPos.z = MaskVectorAsFloat(jumpLanding, transform.parent.forward);

        // Lock to z-axis
        Vector3 arP = new Vector3(0, arrivingPos.y, arrivingPos.z);

        ////////////////////// THIS IS NOT MY CODE, ORIGIONAL CODE FOUND AT https://gamedev.stackexchange.com/questions/133794/parabolic-movement-of-a-gameobject-in-unity ////////////////

        Vector3 diff = ((arP - stP) / 2) + new Vector3(0, 1, 0);

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
            Vector3 temp = transform.parent.right * newZ + transform.parent.up * yToBeFound + transform.parent.forward * newX;
            jumpFrames[i] = temp;
        }
        ////////////////////// END OF BORROWED CODE //////////////////////////
    }

    // Masks a vector so only the desired elements are carried on,
    // for example data may be (2.4, 4, 1) and mask may be (0,1,0)
    // the resulting float would be 4
    float MaskVectorAsFloat(Vector3 data, Vector3 mask)
    {
        Vector3 temp;

        temp.x = data.x * mask.x;
        temp.y = data.y * mask.y;
        temp.z = data.z * mask.z;
        return temp.x + temp.y + temp.z;
    }

    // Masks a vector so only the desired elements are carried on,
    // for example data may be (2.4, 4, 1) and mask may be (0,0,1)
    // the resulting float would be (0, 4, 0)
    static public Vector3 MaskVector(Vector3 data, Vector3 mask)
    {
        Vector3 temp;
        temp.x = data.x * mask.x;
        temp.y = data.y * mask.y;
        temp.z = data.z * mask.z;
        return temp;
    }  
    
    void DestroyIceLily(Transform lily)
    {
        lily.gameObject.layer = 4;
        Vector3[] directions = new Vector3[4];

        directions[0] = lily.forward;
        directions[1] = lily.right;
        directions[2] = -lily.forward;
        directions[3] = -lily.right;

        RaycastHit hit;

        for (int i = 0; i < 4; i++)
        {
            if (Physics.Raycast(lily.position - lily.up * 0.4f, directions[i], out hit, 1.0f, 1))
            {
                if (hit.transform.tag == "Block" || hit.transform.tag == "Sheep" || hit.transform.tag == "Water")
                {
                    hit.transform.GetComponentInChildren<Block>().BlockUpdate();
                }
            }
        }
    }
}
