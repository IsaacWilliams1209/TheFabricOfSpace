using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Geyser : MonoBehaviour
{
    [HideInInspector]
    public Sheep sheep;

    bool active = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (sheep != null && sheep.voxel && sheep.poweredUp)
        {
            active = true;
            // Play animation
        }
        if (active /* and animation is complete*/)
        {
            transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        }
    }
}
