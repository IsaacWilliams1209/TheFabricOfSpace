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
    public List<GameObject> awakeSheep = new List<GameObject>();

    // Array of sheep on the face
    GameObject[] sheep = new GameObject[1];

    GameObject closestSheep;

    // Is the sheep awake
    public bool awake = false;

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

    float jumpLength = 0.7857143f + 0.7413793f - 0.25f;

    // Refers to the shepherd of this face
    [HideInInspector]
    public Shepherd shepherd;

    // Used to cahnge materials for the sheep being awake/asleep/active
    [HideInInspector]
    public SkinnedMeshRenderer matChanger;

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

    [HideInInspector]
    public Mesh defaultMesh;

    [HideInInspector]
    public Animator animator;

    public List<Mesh> meshes = new List<Mesh>();

    public bool staticHoldingSheep = false;

    [HideInInspector]
    public Material[] materialHolder;

    [HideInInspector]
    public TutorialPromt promtChanger;

    bool isEating;

    public GUI_Manager sheepIcons;

    [HideInInspector]
    public Vector3 cameraPos;

    float lerpTimer = 0.75f;

    float timer = 0;

    [HideInInspector]
    public bool isSwapping;

    // Start is called before the first frame update
    void Start()
    {
        // Initalising variables
        animator = GetComponent<Animator>();
        matChanger = transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();
        defaultMesh = matChanger.sharedMesh;
        shepherd = transform.parent.GetComponent<Shepherd>();
        sheep = shepherd.sheep;
        awakeSheep = shepherd.awakeSheep;        
        controller = GetComponent<SheepController>();
        mainCollider = GetComponents<BoxCollider>()[1];
        wakingTrigger = GetComponents<BoxCollider>()[0];
        promtChanger = GameObject.Find("/GameObject").GetComponent<TutorialPromt>();


        // Set apropriate materials for the sheep
        if (active)
        {

            animator.SetBool("IsAwake", true);
            shepherd.activeSheep = gameObject;
            wakingTrigger.enabled = false;
            shepherd.SwapCams();
            Debug.Log("CamsSwapped");
            promtChanger.UpdateText(this);
        }
        else if (awake)
        {
            animator.SetBool("IsAwake", true);
            awakeSheep.Add(gameObject);
            wakingTrigger.enabled = false;
        }
        if(sheepType != SheepType.Sheared)
        {
            berryIndex = -2;
            switch (sheepType)
            {
                case SheepType.Slab:
                    materialHolder = matChanger.materials;
                    materialHolder[1] = sheepMaterials[0];
                    materialHolder[2] = sheepMaterials[0];
                    materialHolder[0] = sheepMaterials[0];
                    matChanger.materials = materialHolder;
                    matChanger.sharedMesh = meshes[0];

                    if (poweredUp)
                    {
                        GetComponent<SlabSheep>().ActivatePowerUp(this);
                    }
                    break;
                case SheepType.Snowball:
                    animator.SetBool("IsSnowball", true);
                    materialHolder = matChanger.materials;
                    materialHolder[1] = sheepMaterials[3];
                    materialHolder[2] = sheepMaterials[3];
                    materialHolder[0] = sheepMaterials[3];
                    matChanger.materials = materialHolder;
                    matChanger.sharedMesh = meshes[2];
                    break;
                case SheepType.Static:
                    materialHolder = matChanger.materials;
                    materialHolder[1] = sheepMaterials[4];
                    materialHolder[2] = sheepMaterials[4];
                    materialHolder[0] = sheepMaterials[4];
                    matChanger.materials = materialHolder;
                    matChanger.sharedMesh = meshes[3];
                    break;
                default:
                    break;


            }
        }        
    }

    // Update is called once per frame
    void Update()
    {
        if (isSwapping)
        {
            CamLerp(awakeSheep[awakeSheep.Count - 1].transform.GetChild(2).GetChild(1).position, cameraPos);
            if (timer > lerpTimer)
            {
                isSwapping = false;
                timer = 0;
            }
            else
            {
                return;
            }

        }

        if (isEating)
        {
            jumpTime += Time.deltaTime;
            if (jumpTime < 2.66f)
            {
                canMove = false;
            }
            else
            {
                isEating = false;
                jumpTime = 0;
                canMove = true;
            }
        }
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
                    jumpTime += Time.deltaTime / jumpLength * jumpFrames.Length;

                    transform.position = Vector3.Lerp(jumpFrames[jumpIndex], jumpFrames[jumpIndex + 1], jumpTime);

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

                            poweredUp = false;
                            canMove = true;

                            jumpIndex = 0;

                            transform.position = transform.position + transform.forward * 0.1f;
                        }
                    }
                }
            }
            if (Input.GetButtonDown("Jump"))
            {
                if (canWake)
                {
                    closestSheep.GetComponent<Sheep>().awake = true;
                    sheepIcons.guiNeedsUpdate = true;
                    canWake = false;
                    //closestSheep.transform.GetChild(1).GetComponent<Renderer>().material = sheepMaterials[0];
                    closestSheep.GetComponent<Sheep>().wakingTrigger.enabled = false;
                    awakeSheep.Insert(0, closestSheep);
                    swap = true;

                }
                if (canEat && shepherd.berries[berryIndex].GetComponent<Shrubs>().Eat())
                {
                    shepherd.berries[berryIndex].GetComponent<Shrubs>().GrantPowerUp(gameObject);
                    animator.SetTrigger("IsEating");
                    animator.SetBool("IsWalking", false);
                    isEating = true;
                    switch (sheepType)
                    {
                        case SheepType.Slab:
                            matChanger.sharedMesh = meshes[0];
                            materialHolder = matChanger.materials;
                            materialHolder[1] = sheepMaterials[0];
                            materialHolder[2] = sheepMaterials[0];
                            materialHolder[0] = sheepMaterials[0];
                            matChanger.materials = materialHolder;
                            break;
                        case SheepType.Snowball:
                            materialHolder = matChanger.materials;
                            materialHolder[1] = sheepMaterials[3];
                            materialHolder[2] = sheepMaterials[3];
                            materialHolder[0] = sheepMaterials[3];
                            matChanger.materials = materialHolder;
                            matChanger.sharedMesh = meshes[2];
                            break;
                        case SheepType.Static:
                            materialHolder = matChanger.materials;
                            materialHolder[1] = sheepMaterials[4];
                            materialHolder[2] = sheepMaterials[4];
                            materialHolder[0] = sheepMaterials[4];
                            matChanger.materials = materialHolder;
                            matChanger.sharedMesh = meshes[3];
                            break;
                        default:
                            break;
                    }

                    poweredUp = false;
                }
            }
            // On R press activate the sheep powerup

            if (Input.GetKeyDown(KeyCode.E) && !isJumping)
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
                        if (poweredUp)
                        {
                            GetComponent<StaticSheep>().ActivatePowerUp(this);
                        }
                        else
                        {
                            GetComponent<StaticSheep>().DeActivatePowerUp(this);
                        }
                        break;
                    default:
                        break;
                }

            }
            // On left shift press, swap to the next active sheep
            if (Input.GetKeyUp(KeyCode.LeftShift) && !isJumping && !staticHoldingSheep && (canMove || (sheepType == SheepType.Slab && poweredUp)))
            {
                if (awakeSheep.Count != 0)
                {
                    swap = true;
                }
            }

            RaycastHit hit;
            if (sheepType != SheepType.Sheared && sheepType != SheepType.Snowball && Physics.Raycast(transform.position + transform.up * 0.3f, -transform.up, out hit, 1.0f, 1))
            {
                //Debug.Log(hit.transform.name);
                //Debug.DrawRay(transform.position + transform.up * 0.3f, -transform.up, Color.red, 4);
                if (hit.transform.tag == "Water")
                {
                    IceLily temp;
                    if (hit.transform.TryGetComponent<IceLily>(out temp))
                    {
                        Debug.Log("WalkedOn = true");
                        temp.walkedOn = true;
                    }

                }
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
                    //hit.transform.GetComponent<SlabSheep>().steppedOn = true;
                }
            }
        }
    }

    void LateUpdate()
    {

        // Swap to the next sheep
        if (swap)
        {
            sheepIcons.guiNeedsUpdate = true;
            animator.SetBool("IsWalking", false);
            //shepherd.SwapCams();
            if (shepherd.isSheepFocus)
            {
                awakeSheep[0].transform.GetChild(2).GetChild(1).gameObject.SetActive(true);
                awakeSheep[0].GetComponent<Sheep>().cameraPos = awakeSheep[0].transform.GetChild(2).GetChild(1).position;
                awakeSheep[0].GetComponent<Sheep>().isSwapping = true;
                awakeSheep[0].transform.GetChild(2).GetChild(1).position = transform.GetChild(2).GetChild(1).position;                
                transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
            }
            promtChanger.UpdateText(awakeSheep[0].GetComponent<Sheep>());
            transform.GetComponent<BoxCollider>().enabled = true;
            shepherd.activeSheep = awakeSheep[0];
            awakeSheep[0].GetComponent<Sheep>().animator.SetBool("IsAwake", true);
            //awakeSheep[0].transform.GetChild(1).GetComponent<Renderer>().material = sheepMaterials[0];
            awakeSheep[0].GetComponent<Sheep>().active = true;
            awakeSheep.RemoveAt(0);
            awakeSheep.Add(gameObject);
            //matChanger.material = sheepMaterials[1];            
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
            transition.Activate(this);
            return;
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
            if (sheepType == SheepType.Slab && other.transform.parent.GetComponent<Geyser>().sheep == null)
            {
                other.transform.parent.GetComponent<Geyser>().sheep = this;
            }
        }
        if (other.gameObject.tag == "Sheep" && !other.GetComponent<Sheep>().awake)
        {
            canWake = true;

            closestSheep = other.gameObject;
        }
        if (other.gameObject.tag == "Reg" && berryIndex == -1)
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
            if (other.transform.parent.GetComponent<Geyser>().sheep == this)
                other.transform.parent.GetComponent<Geyser>().sheep = null;

        if (other.gameObject.tag == "Sheep")
        {
            canWake = false;

            closestSheep = null;
        }

        if (other.gameObject.tag == "Reg")
        {
            canEat = false;

            if (sheepType == SheepType.Sheared)
            {

                berryIndex = -1;

            }
        }
    }

    // Calculates the frames the jump
    private void DoDaJump()
    {

        animator.SetTrigger("IsJumping");

        // Disable movement and jump ability

        canMove = false;

        canJump = false;

        // Activate jumping bool
        isJumping = true;

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

        if (arrivingPos.z == startingPos.z)
        {
            startingPos.z += 0.1f;
            stP.z += 0.1f;
        }

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

        float z_dist = (arrivingPos.z - startingPos.z) / numFrames;
        float x_dist = (arrivingPos.x - startingPos.x) / numFrames;

        float A = (x3 * (y2 - y1) + x2 * (y1 - y3) + x1 * (y3 - y2)) / denom;
        float B = ((x3 * x3) * (y1 - y2) + (x2 *x2) * (y3 - y1) + (x1* x1) * (y2 - y3)) / denom;
        float C = (x2 * x3 * (x2 - x3) * y1 + x3 * x1 * (x3 - x1) * y2 + x1 * x2 * (x1 - x2) * y3) / denom;

        float newX = startingPos.z;
        float newZ = startingPos.x;

        float yTest = 0;
        for (int i = 0; i < numFrames; i++)
        {
            newX += z_dist;
            newZ += x_dist;

            float yToBeFound = A * (newX * newX) + B * newX + C;
            yTest = yToBeFound;
            Vector3 temp = transform.parent.right * newZ + transform.parent.up * yToBeFound + transform.parent.forward * newX;
            jumpFrames[i] = temp;
        }
        if (yTest - MaskVectorAsFloat(transform.position, transform.up) > 0.1f)
        {
            for (int i = 0; i< numFrames; i++)
            {
                jumpFrames[i] -= transform.up * (yTest - MaskVectorAsFloat(transform.position, transform.up));
            }
        }
        Debug.Log("Jump complete");
        ////////////////////// END OF BORROWED CODE //////////////////////////
    }

    // Masks a vector so only the desired elements are carried on,
    // for example data may be (2.4, 4, 1) and mask may be (0,1,0)
    // the resulting float would be 4
    static public float MaskVectorAsFloat(Vector3 data, Vector3 mask)
    {
        Vector3 temp;
        temp.x = data.x * (mask.x);
        temp.y = data.y * (mask.y);
        temp.z = data.z * (mask.z);
        return temp.x + temp.y + temp.z;
    }

    // Masks a vector so only the desired elements are carried on,
    // for example data may be (2.4, 4, 1) and mask may be (0,0,1)
    // the resulting Vector3 would be (0, 0, 1)
    static public Vector3 MaskVector(Vector3 data, Vector3 mask)
    {
        Vector3 temp;
        temp.x = data.x * Mathf.Abs(mask.x);
        temp.y = data.y * Mathf.Abs(mask.y);
        temp.z = data.z * Mathf.Abs(mask.z);
        return temp;
    }

    void CamLerp(Vector3 from, Vector3 to)
    {
        timer += Time.deltaTime;
        transform.GetChild(2).GetChild(1).position = Vector3.Lerp(from, to, timer / lerpTimer);
    }


    void OnDrawGizmos()
    {
        if (jumpFrames[0] != null)
        {
            Gizmos.color = Color.yellow;
            foreach (Vector3 point in jumpFrames)
            {
                Gizmos.DrawSphere(point, .1f);
            }
        }
    }

}
