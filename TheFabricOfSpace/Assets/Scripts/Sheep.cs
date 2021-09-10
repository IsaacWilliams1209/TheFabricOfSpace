﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour
{
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

    GameObject closestSheep;

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

    public bool canEat;

    public bool canWake;

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
    SheepController controller;

    BoxCollider mainCollider;

    BoxCollider wakingTrigger;

    Mesh defaultMesh;

    [SerializeField]
    List<Mesh> meshes = new List<Mesh>();

    // Start is called before the first frame update
    void Start()
    {
        // Initalising variables
        defaultMesh = transform.GetChild(2).GetComponent<MeshFilter>().mesh;
        shepherd = transform.parent.GetComponent<Shepherd>();
        sheep = shepherd.sheep;
        awakeSheep = shepherd.awakeSheep;
        matChanger = transform.GetChild(2).GetComponent<Renderer>();
        controller = GetComponent<SheepController>();
        mainCollider = GetComponents<BoxCollider>()[0];
        wakingTrigger = GetComponents<BoxCollider>()[1];

        // Set apropriate materials for the sheep
        if (active)
        {
            matChanger.material = sheepMaterials[0];
            shepherd.activeSheep = gameObject;
            wakingTrigger.enabled = false;
        }
        else if (awake)
        {
            matChanger.material = sheepMaterials[1];
            awakeSheep.Add(gameObject);
            wakingTrigger.enabled = false;
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

                controller.Move();
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
            if (canEat && shepherd.berries[berryIndex].GetComponent<Shrubs>().Eat())
            {
                shepherd.berries[berryIndex].GetComponent<Shrubs>().GrantPowerUp(gameObject);
                transform.GetChild(2).GetComponent<MeshFilter>().mesh = meshes[0];
                poweredUp = false;
            }

            if (Input.GetButtonDown("Jump"))
            {
                closestSheep.GetComponent<Sheep>().awake = true;
                closestSheep.transform.GetChild(2).GetComponent<Renderer>().material = sheepMaterials[0];
                awakeSheep.Insert(0, closestSheep);
                swap = true;
            }



            // On R press activate the sheep powerup
            if (Input.GetKeyDown(KeyCode.R))
            {
                    poweredUp = !poweredUp;
                    ActivatePowerUp();
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
            awakeSheep[0].transform.GetChild(2).GetComponent<Renderer>().material = sheepMaterials[0];
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
            Shepherd tempShepherd = GameObject.Find("Test").GetComponent<Shepherd>();
            // you win! activate world rotation
            tempShepherd.awakeSheep[0].GetComponent<Sheep>().active = true;
            tempShepherd.awakeSheep[0].GetComponent<Renderer>().material = sheepMaterials[0];
            tempShepherd.activeSheep = tempShepherd.awakeSheep[0];
            awakeSheep.Insert(0, gameObject);
            matChanger.material = sheepMaterials[1];
            active = false;
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
        if (other.gameObject.tag == "Geyser")
        {
            Debug.Log("Geyser triggered");
            other.transform.parent.GetComponent<Geyser>().sheep = this;
        }
        if (other.gameObject.tag == "Sheep")
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

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Sheep" && active)
        {
            if (Input.GetButtonDown("Jump"))
            {
                other.gameObject.GetComponent<Sheep>().awake = true;
                other.gameObject.transform.GetChild(2).GetComponent<Renderer>().material = sheepMaterials[0];
                awakeSheep.Insert(0, other.gameObject);
                swap = true;
            }
        }
        //else if (other.gameObject.tag == "Reg" && active)
        //{
        //    if (Input.GetButtonDown("Jump") && other.gameObject.GetComponent<Shrubs>().Eat())
        //    {
        //        other.gameObject.GetComponent<Shrubs>().GrantPowerUp(gameObject);
        //        berryIndex = other.GetComponent<Shrubs>().index;
        //        transform.GetChild(2).GetComponent<MeshFilter>().mesh = meshes[0];
        //        poweredUp = false;
        //    }
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        // Remove jump ability when leaving the trigger
        if (other.gameObject.tag == "Jump")
            canJump = false;
        if (other.gameObject.tag == "Geyser")
        {
            
            other.transform.parent.GetComponent<Geyser>().sheep = null;
        }
        if (other.gameObject.tag == "Sheep")
        {
            canWake = false;
            closestSheep = null;
        }
        if (other.gameObject.tag == "Reg")
        {
            canEat = false;
            if (!voxel)
            {
                berryIndex = -1;
            }
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
                mainCollider.enabled = true;

                // Prevent movement and lock to tile
                canMove = false;                
                Vector3 temp = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z)) - transform.parent.up * 0.45f;

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
                // Activate block on slab sheep
                transform.GetChild(0).gameObject.SetActive(true);
                // Update block for the on the slab sheep
                transform.GetComponentInChildren<Block>().BlockUpdate();
                gameObject.layer = 0;
                transform.GetChild(0).gameObject.layer = 0;
                transform.GetChild(2).GetComponent<MeshFilter>().mesh = meshes[1];
            }
            else
            {
                gameObject.layer = 2;
                transform.GetChild(0).gameObject.layer = 2;
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
                transform.GetChild(2).GetComponent<MeshFilter>().mesh = meshes[0];
                mainCollider.enabled = false;
            }
        }
        else
        {
            if (canJump)
            {
                DoDaJump();
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
        startingPos.x = MaskVector(transform.position, transform.parent.right);
        startingPos.y = MaskVector(transform.position, transform.parent.up);
        startingPos.z = MaskVector(transform.position, transform.parent.forward);
        Debug.Log("StartPos: " + startingPos.y);
        Debug.Log("Position: " + transform.position.y);
        Debug.Log("Up: " +transform.parent.up);
        
        // Lock to z-axis
        Vector3 stP = new Vector3(0, startingPos.y, startingPos.z);

        // Convert position from forward, up and right, to x,y,z
        Vector3 arrivingPos;
        arrivingPos.x = MaskVector(jumpLanding, transform.parent.right);
        arrivingPos.y = MaskVector(jumpLanding, transform.parent.up);
        arrivingPos.z = MaskVector(jumpLanding, transform.parent.forward);
        Debug.Log("ArrivePos: " + arrivingPos.y);
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
            Vector3 temp = transform.parent.right * newZ + transform.parent.up * yToBeFound + transform.parent.forward * newX;
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
    void OnDrawGizmos()
    {
        if (jumpFrames[0] != null)
        {
            Gizmos.color = Color.yellow;
            foreach (Vector3 point in jumpFrames)
            {
                Gizmos.DrawSphere(point, .1f);
            }
            Vector3 temp = transform.position;
            temp += MaskVector2(new Vector3(-0.05f,-0.05f,-0.05f), transform.forward);
            Gizmos.DrawWireCube(temp, Vector3.one * 0.5f);
        }
    }

    Vector3 MaskVector2(Vector3 data, Vector3 mask)
    {
        Vector3 temp;
        temp.x = data.x * mask.x;
        temp.y = data.y * mask.y;
        temp.z = data.z * mask.z;
        return temp;
    }
}
