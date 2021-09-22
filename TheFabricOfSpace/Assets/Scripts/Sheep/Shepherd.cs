using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SheepType
{
    Sheared,
    Slab,
    Snowball,
    Static
}
public class Shepherd : MonoBehaviour
{
    // List of sheep on the face
    public GameObject[] sheep = new GameObject[1];

    // List of awake sheep on the face
    public List<GameObject> awakeSheep = new List<GameObject>();

    // List of berries on the face
    public GameObject[] berries = new GameObject[1];

    // Main camera in the scene
    [SerializeField]
    Camera mainCamera;
    
    // Tracks the currently active sheep
    [HideInInspector]
    public GameObject activeSheep;
    
    // Tracks whether the sheep or the planet are currently the focus 
    [HideInInspector]
    public bool isSheepFocus = false;

    // Start is called before the first frame update
    void Start()
    {
        // Gives sheep and berries thier index
        for (int i = 0; i < sheep.Length; i++)
        {
            sheep[i].GetComponent<Sheep>().index = i;
        }
        for (int i = 0; i < berries.Length; i++)
        {
            berries[i].GetComponent<Shrubs>().index = i;
        }
        //mainCamera = transform.parent.parent.GetChild(0).GetChild(0).GetComponent<Camera>();
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Swaps camera between main camera to camera attached to the currently active sheep
    public void SwapCams()
    {
        if (isSheepFocus)
        {
            mainCamera.gameObject.SetActive(true);
            activeSheep.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            mainCamera.gameObject.SetActive(false);
            activeSheep.transform.GetChild(1).gameObject.SetActive(true);
        }
        isSheepFocus = !isSheepFocus;
    }
}
