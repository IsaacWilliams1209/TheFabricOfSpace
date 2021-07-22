using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shepherd : MonoBehaviour
{
    public GameObject[] sheep = new GameObject[1];

    public List<GameObject> awakeSheep = new List<GameObject>();

    public GameObject[] berries = new GameObject[1];

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
}
