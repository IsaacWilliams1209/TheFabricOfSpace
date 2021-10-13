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
    }


    public void DeActivatePowerUp(Sheep sheep)
    {
        RaycastHit hit = new RaycastHit();

        Debug.DrawRay(transform.position + transform.up + transform.GetChild(2).transform.forward * 0.5f,
            transform.GetChild(2).transform.forward - transform.GetChild(2).transform.up * 1.2f, Color.red, 3.0f);

        if (Physics.Raycast(transform.position + transform.up + transform.GetChild(2).transform.forward * 0.5f,
            transform.GetChild(2).transform.forward - transform.GetChild(2).transform.up * 1.2f, out hit, 2.0f))
        {
            Debug.Log(hit.transform.tag);
            if (hit.transform.tag == "Block")
            {
                grabbedSheep.transform.position = transform.position + transform.GetChild(2).transform.forward * 1.0f;
                hasSheep = false;
                grabbedSheep = null;
            }
        }
        else
        {
            Debug.Log("Nothing hit");

            //hasSheep = false;
            //grabbedSheep = null;
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
        if (other.gameObject.tag == "Sheep")
        {
            //grabbedSheep = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + transform.GetChild(2).transform.up + transform.GetChild(2).transform.forward * 0.5f, new Vector3(0.1f, 0.1f, 0.1f));
    }

}
