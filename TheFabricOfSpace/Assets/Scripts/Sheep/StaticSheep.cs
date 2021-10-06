using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticSheep : Sheep
{

    Sheep grabbedSheep;

    public void ActivatePowerUp(Sheep sheep)
    {
        grabbedSheep.transform.position = sheep.transform.position + transform.up;
    }

    public void DeActivatePowerUp(Sheep sheep)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Sheep")
        {
            grabbedSheep = other.GetComponent<Sheep>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Sheep")
        {
            grabbedSheep = null;
        }
    }


}
