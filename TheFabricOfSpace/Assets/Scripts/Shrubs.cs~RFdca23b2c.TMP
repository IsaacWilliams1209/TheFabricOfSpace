using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrubs : MonoBehaviour
{
    // Has the shrub been eaten
    public bool eaten = false;

    [HideInInspector]
    public int index;

    public bool Eat()
    {
        // Check the shrub hasn't been eaten, if it hasn't transform to eaten state
        if (!eaten)
        {
            transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
            eaten = true;
            return true;
        }
        return false;
    }

    // Restores the bush to it's un eaten state
    public void Restore()
    {
        transform.localScale = new Vector3(1,1,1);
        eaten = false;
    }

    // Activates the shrubs power up
    public void GrantPowerUp(GameObject sheep)
    {
        if (tag == "Reg")
        {
            //sheep.transform.localScale = new Vector3(0.95f, 0.9f, 0.95f);
            sheep.GetComponent<Sheep>().voxel = true;
        }
    }
}
