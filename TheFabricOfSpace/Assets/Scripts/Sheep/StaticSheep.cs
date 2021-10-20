using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticSheep : Sheep
{

    Sheep grabbedSheep;
    bool hasSheep = false;
    

    private void RadialLocationCheck(Vector3 sheepRotation)
    {
        Debug.Log(sheepRotation.y);
        //Grabbed sheep is connected infront of the sheep
        if(sheepRotation.y >= 316.0f && sheepRotation.y <= 45)
        {
            Debug.Log("Attach to front side of the sheep");
        }
        //Grabbed sheep is connected to right side of sheep
        if (sheepRotation.y >= 46.0f && sheepRotation.y <= 135)
        {
            Debug.Log("Attach to the right side of the sheep");
        }
        //Grabbed sheep is connected behind the sheep
        if (sheepRotation.y >= 136.0f && sheepRotation.y <= 225.0f)
        {
            Debug.Log("Attach to the backside the sheep");
        }
        //Grabbed sheep is connected to left side of sheep
        if (sheepRotation.y >= 226.0f && sheepRotation.y <= 315.0f)
        {
            Debug.Log("Attach to the left side of the sheep");
        }
        //return sheepRotation;
    }

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

        Vector3 temp = transform.GetChild(0).rotation.eulerAngles;
        RadialLocationCheck(temp);

        //RaycastHit[] hits = new RaycastHit[4];

        //Vector3[] directions = new Vector3[4];

        //directions[0] = transform.GetChild(0).forward;
        //directions[1] = transform.GetChild(0).right;
        //directions[2] = -transform.GetChild(0).forward;
        //directions[3] = -transform.GetChild(0).right;

        //for (int i = 0; i < hits.Length; i++)
        //{

        //    Debug.DrawRay(transform.position, directions[i], Color.white, 3.0f);
        //    grabbedSheep.transform.position = sheep.transform.position + transform.up;
        //    hasSheep = true;
        //    sheep.staticHoldingSheep = true;

        //}

        //if(grabbedSheep.sheepType == SheepType.Slab)
        //{
        //    Debug.Log("This is a static sheep");
        //}
        //else
        //{


        //}
    }


    public void DeActivatePowerUp(Sheep sheep)
    {
        if(sheep.staticHoldingSheep == false)
        {
            return;
        }

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
