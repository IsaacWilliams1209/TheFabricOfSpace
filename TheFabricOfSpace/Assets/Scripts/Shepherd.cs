using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shepherd : MonoBehaviour
{
    public GameObject[] sheep = new GameObject[1];

    public List<GameObject> awakeSheep = new List<GameObject>();

    public GameObject[] berries = new GameObject[1];

    [SerializeField]
    Camera mainCamera;

    [HideInInspector]
    public GameObject activeSheep;
    [HideInInspector]
    public bool isSheepFocus = false;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < sheep.Length; i++)
        {
            sheep[i].GetComponent<Sheep>().index = i;
        }
        for (int i = 0; i < berries.Length; i++)
        {
            berries[i].GetComponent<Shrubs>().index = i;
        }
    }

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
