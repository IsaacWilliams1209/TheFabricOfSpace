using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticSheep : Sheep
{

    Sheep grabbedSheep;
    bool hasSheep = false;

    private void Update()
    {

        if (grabbedSheep != null)
        {
            if (hasSheep)
            {
                grabbedSheep.transform.position = transform.position + transform.up;
                grabbedSheep.transform.GetChild(0).rotation = transform.GetChild(0).rotation;
            }
        }

    }

    public void ActivatePowerUp(Sheep sheep)
    {
        if (grabbedSheep == null)
        {
            sheep.poweredUp = false;
            return;
        }

        grabbedSheep.transform.position = sheep.transform.position + transform.up;
        hasSheep = true;
        sheep.staticHoldingSheep = true;
    }


    public void DeActivatePowerUp(Sheep sheep)
    {
        RaycastHit hit = new RaycastHit();

        //Debug.DrawRay(transform.position + transform.up + transform.GetChild(0).transform.forward * 0.8f, 
        //    transform.GetChild(0).transform.forward - transform.up * 20.0f, Color.red, 3.0f);

        if (Physics.Raycast(transform.position + transform.up + transform.GetChild(0).transform.forward * 0.8f, 
            transform.GetChild(0).transform.forward - transform.up * 30.0f, out hit, 2.0f))
        {
            Debug.Log(hit.transform.name);

            if (hit.transform.tag == "Block" || hit.transform.parent.tag == "Block")
            {
                if(!Physics.Raycast(hit.point, transform.up))
                {
                    grabbedSheep.transform.position = transform.position + transform.GetChild(0).transform.forward * 1.0f;
                    hasSheep = false;
                    grabbedSheep = null;
                    sheep.staticHoldingSheep = false;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Sheep")
        {
            grabbedSheep = other.GetComponent<Sheep>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Sheep" && hasSheep == false)
        {
            grabbedSheep = null;
        }
    }

}
