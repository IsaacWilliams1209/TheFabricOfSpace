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

    

    bool canMove = true;

    int berryIndex = -1;

    Shepherd shepherd;

    Renderer matChanger;

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
                else if (hit.distance < 0.4f)
                {
                    movement += transform.parent.up * 0.01f;
                    

                }
                else if (hit.distance > 0.5f)
                {
                    movement -= transform.parent.up * 0.01f;
                    
                }
                else if (hit.transform.tag == "Sheep" && hit.transform.GetComponent<Sheep>().voxel)
                {
                    hit.transform.GetComponent<Sheep>().poweredUp = true;
                }


                controller.Move(movement);
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
                if (!poweredUp)
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

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                if (awakeSheep.Count != 0)
                {
                    swap = true;
                    
                }
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
    }

    private void ActivatePowerUp()
    {
        if (voxel)
        {
            if (poweredUp)
            {
                canMove = false;
                
                Vector3 temp = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z));

                transform.position =  temp;
                RaycastHit[] hits = new RaycastHit[4];
                Vector3[] directions = new Vector3[4];
                directions[0] = transform.parent.forward;
                directions[1] = transform.parent.right;
                directions[2] = -transform.parent.forward;
                directions[3] = -transform.parent.right;
                for (int i = 0; i < hits.Length; i++)
                {
                    if (Physics.Raycast(transform.position, directions[i], out hits[i], 1.0f))
                    {
                        if (hits[i].transform.tag == "Block" || hits[i].transform.tag == "Slope Upper")
                        {
                            hits[i].transform.GetChild(0).GetComponent<Block>().colliders[(i + 2) % 4].enabled = false;
                        }
                    }
                }
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
            else
            {
                RaycastHit[] hits = new RaycastHit[4];
                Vector3[] directions = new Vector3[4];
                directions[0] = transform.parent.forward;
                directions[1] = transform.parent.right;
                directions[2] = -transform.parent.forward;
                directions[3] = -transform.parent.right;
                for (int i = 0; i < hits.Length; i++)
                {
                    if (Physics.Raycast(transform.position, directions[i], out hits[i], 1.0f))
                    {
                        if (hits[i].transform.tag == "Block" || hits[i].transform.tag == "Slope Upper")
                        {
                            hits[i].transform.GetChild(0).GetComponent<Block>().colliders[(i + 2) % 4].enabled = true;
                        }
                    }
                }
                canMove = true;
                transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }
        }
    }
}
