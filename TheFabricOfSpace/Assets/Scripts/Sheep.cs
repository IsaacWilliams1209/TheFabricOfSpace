using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour
{
    // Speed of the sheep
    public float speed;

    // Keeps the awake, asleep and active materials for the sheep
    public Material[] sheepMaterials = new Material[3];

    // Sheep index inside the shepherd's sheep array
    [HideInInspector]
    public int index;

    // Is the sheep a voxel/slab sheep
    public bool voxel = false;

    // Is the sheep currently using it's powerup
    public bool poweredUp = false;

    // List of awake sheep on the face
    List<GameObject> awakeSheep = new List<GameObject>();

    // Array of sheep on the face
    GameObject[] sheep = new GameObject[1];

    // Is the sheep awake
    [SerializeField]
    bool awake = false;

    // Is the Sheep active
    [SerializeField]
    bool active = false;

    // Will the sheep be swapped this frame
    bool swap = false;

    // Is the sheep able to jump
    public bool canJump;

    // Is the sheep currently jumping
    bool isJumping;

    // Can the sheep move
    bool canMove = true;

    // Index of eaten berry in the Shepherd's Berry array, -1 means no berry eaten
    int berryIndex = -1;
    
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
    CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        // Initalising variables
        shepherd = transform.parent.GetComponent<Shepherd>();
        sheep = shepherd.sheep;
        awakeSheep = shepherd.awakeSheep;
        matChanger = GetComponent<Renderer>();
        controller = GetComponent<CharacterController>();

        // Set apropriate materials for the sheep
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
            berryIndex = -2;
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
                // Move the player forward/backward
                movement += transform.parent.forward * Input.GetAxis("Vertical") * speed * Time.deltaTime;

                // Move the player left/right
                movement += transform.parent.right * Input.GetAxis("Horizontal") * speed * Time.deltaTime;
                                
                RaycastHit hit;
                
                // Raycast down, on miss move the sheep down
                if (!Physics.Raycast(transform.position, -transform.parent.up, out hit, 1.0f))
                {
                    movement += transform.parent.up * Physics.gravity.y * Time.deltaTime;
                }
                // If the raycast hits and is less than .4m move it up
                else if (hit.distance < 0.4f && !hit.collider.isTrigger)
                {
                    movement += transform.parent.up * 0.01f;


                }
                // If the raycast hits and is greater than .5m move it down
                else if (hit.distance > 0.5f && !hit.collider.isTrigger)
                {
                    movement -= transform.parent.up * 0.01f;

                }
                // If the raycast hits a voxel sheep set it's powered up state to true, this prevents the sheep being moved while another sheep in ontop of them
                else if (hit.transform.tag == "Sheep" && hit.transform.GetComponent<Sheep>().voxel)
                {
                    hit.transform.GetComponent<Sheep>().poweredUp = true;
                }


                controller.Move(movement);
            }
            else
            {
                // If the player can't move and is jump cycle through the jumpFrames
                if (isJumping)
                {
                    jumpTime += Time.deltaTime;
                    float percentDone = jumpTime/ Time.deltaTime * 0.25f;
                    transform.position = Vector3.Lerp(jumpFrames[jumpIndex], jumpFrames[jumpIndex + 1], percentDone);
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
            // On spacebar press awaken nearby sheep or eat nearby berry
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
                if (berryIndex == -1)
                {
                    for (int i = 0; i < shepherd.berries.Length; i++)
                    {
                        if (Vector3.SqrMagnitude(shepherd.berries[i].transform.position - transform.position) < 1.0f)
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
            // On R press activate the sheep powerup
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (berryIndex != -1)
                {
                    poweredUp = !poweredUp;
                    ActivatePowerUp();
                }
            }
            // On G press start jumping
            if (canJump && Input.GetKeyDown(KeyCode.G) && !voxel)
            {
                DoDaJump();
            }
            // On left shift press, swap to the next active sheep
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
            // Raycast down to detect if a voxel sheep is below and prevent it from moving with another sheep
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
        // Swap to the next sheep
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

    // Checks for jump triggers and wini triggers
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Finish")
        {
            // you win! activate world rotation
            Debug.Log("Good Job!");
        }
        else if (other.gameObject.tag == "Jump")
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
        else if (other.gameObject.tag == "Geyser")
        {
            other.gameObject.GetComponent<Geyser>().sheep = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Remove jump ability when leaving the trigger
        if (other.gameObject.tag == "Jump")
            canJump = false;
        else if (other.gameObject.tag == "Geyser")
        {
            other.gameObject.GetComponent<Geyser>().sheep = null;
        }
    }

    private void ActivatePowerUp()
    {
        // Voxel/slab sheep power up/down
        if (voxel)
        {
            if (poweredUp)
            {
                // Move to ignore raycast layer
                gameObject.layer = 2;               

                // Prevent movement and lock to tile
                canMove = false;                
                Vector3 temp = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z));

                transform.position = temp;
                RaycastHit[] hits = new RaycastHit[4];
                Vector3[] directions = new Vector3[4];
                directions[0] = transform.parent.forward;
                directions[1] = transform.parent.right;
                directions[2] = -transform.parent.forward;
                directions[3] = -transform.parent.right;
                for (int i = 0; i < hits.Length; i++)
                {
                    Debug.DrawRay(transform.position, directions[i], Color.blue, 6.0f);
                    if (Physics.Raycast(transform.position, directions[i], out hits[i], 2.0f))
                    {
                        
                        if (hits[i].transform.tag == "Block" || hits[i].transform.tag == "Sheep")
                        {
                            // Update nearby blocks
                            gameObject.layer = 0;
                            hits[i].transform.GetComponentInChildren<Block>().BlockUpdate();
                            gameObject.layer = 2;
                        }
                            
                    }
                }
                // Enlarge sheep
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                // Activate block on slab sheep
                transform.GetChild(0).gameObject.SetActive(true);
                // Update block for the on the slab sheep
                transform.GetComponentInChildren<Block>().BlockUpdate();
                gameObject.layer = 0;
            }
            else
            {
                gameObject.layer = 2;
                // Set block on slab sheep to inactive
                transform.GetChild(0).gameObject.SetActive(false);
                RaycastHit[] hits = new RaycastHit[4];
                Vector3[] directions = new Vector3[4];
                directions[0] = transform.parent.forward;
                directions[1] = transform.parent.right;
                directions[2] = -transform.parent.forward;
                directions[3] = -transform.parent.right;
                for (int i = 0; i < hits.Length; i++)
                {
                    Debug.DrawRay(transform.position, directions[i], Color.blue, 6.0f);
                    if (Physics.Raycast(transform.position, directions[i], out hits[i], 2.0f))
                    {
                        if (hits[i].transform.tag == "Block" || hits[i].transform.tag == "Sheep")
                        {
                            // Update nearby blocks
                            hits[i].transform.GetComponentInChildren<Block>().BlockUpdate();
                        }
                    }
                }              
                // Release movement
                canMove = true;
                // Shrink sheep
                transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
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
        startingPos.x = MaskVector(transform.position, transform.right);
        startingPos.y = MaskVector(transform.position, transform.up);
        startingPos.z = MaskVector(transform.position, transform.forward);
        
        // Lock to z-axis
        Vector3 stP = new Vector3(0, startingPos.y, startingPos.z);

        // Convert position from forward, up and right, to x,y,z
        Vector3 arrivingPos;
        arrivingPos.x = MaskVector(jumpLanding, transform.right);
        arrivingPos.y = MaskVector(jumpLanding, transform.up);
        arrivingPos.z = MaskVector(jumpLanding, transform.forward);

        // Lock to z-axis
        Vector3 arP = new Vector3(0, arrivingPos.y, arrivingPos.z);


        ////////////////////// THIS IS NOT MY CODE, ORIGIONAL CODE FOUND AT https://gamedev.stackexchange.com/questions/133794/parabolic-movement-of-a-gameobject-in-unity ////////////////

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
        ////////////////////// END OF BORROWED CODE //////////////////////////
    }

    // Masks a vector so only the desired elements are carried on,
    // for example data may be (2.4, 4, 1) and mask may be (0,1,0)
    // the resulting vector would be (0,4,0)
    float MaskVector(Vector3 data, Vector3 mask)
    {
        Vector3 temp;
        temp.x = data.x * mask.x;
        temp.y = data.y * mask.y;
        temp.z = data.z * mask.z;
        return temp.x + temp.y + temp.z;
    }
}
