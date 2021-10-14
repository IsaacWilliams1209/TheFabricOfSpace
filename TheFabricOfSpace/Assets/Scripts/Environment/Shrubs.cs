using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrubs : MonoBehaviour
{
    // Has the shrub been eaten
    public bool eaten = false;

    [SerializeField]
    SheepType shrubType = SheepType.Slab;

    GameObject berry;

    // Holds the berries index in the shepherd
    [HideInInspector]
    public int index;

    void Start()
    {
        switch (shrubType)
        {
            case SheepType.Slab:
                berry = transform.GetChild(2).gameObject;
                break;
            case SheepType.Snowball:
                berry = transform.GetChild(0).gameObject;
                break;
            case SheepType.Static:
                berry = transform.GetChild(3).gameObject;
                break;
            default:
                Debug.Log("Something went wrong L40 shrub.cs");
                break;
        }

    }

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
        berry.SetActive(true);
    }

    // Activates the shrubs power up
    public void GrantPowerUp(GameObject sheep)
    {
        switch (shrubType)
        {
            case SheepType.Slab:
                transform.parent.GetComponent<Shepherd>().AddPowerToSheep(SheepType.Slab);
                break;
            case SheepType.Snowball:
                transform.parent.GetComponent<Shepherd>().AddPowerToSheep(SheepType.Snowball);
                break;
            case SheepType.Static:
                transform.parent.GetComponent<Shepherd>().AddPowerToSheep(SheepType.Static);
                break;
            default:
                Debug.Log("Something went wrong L47 shrub.cs");
                break;
        }
        berry.SetActive(false);
    }
}
