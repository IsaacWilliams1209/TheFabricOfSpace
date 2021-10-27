using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticSheep : Sheep
{

    Sheep grabbedSheep;
    bool hasSheep = false;
    Vector3 front , right, back, left;
    Vector3 displacementToSheep;
    float frontSide , rightSide, backSide, leftSide;
   

    private void AttachSheep()
    {
        front = transform.GetChild(0).forward;
        right = transform.GetChild(0).right;
        back  = -transform.GetChild(0).forward;
        left  = -transform.GetChild(0).right;

        displacementToSheep = grabbedSheep.transform.position - transform.position;

        frontSide = Vector3.Dot(transform.GetChild(0).forward, transform.position);
        backSide =  Vector3.Dot(-transform.GetChild(0).forward, transform.position);
        rightSide = Vector3.Dot(transform.GetChild(0).forward, transform.position);
        leftSide =  Vector3.Dot(-transform.GetChild(0).right, transform.position);

        Vector3[] testing = new Vector3[5];
        testing[0] = front;
        testing[1] = right;
        testing[2] = back;
        testing[3] = left;
        testing[4] = displacementToSheep;

        float[] debugging = new float[4];
        debugging[0] = frontSide;
        debugging[1] = backSide;
        debugging[2] = rightSide;
        debugging[3] = leftSide;

        for (int i = 0; i < 5; i++)
        {
            Debug.DrawRay(transform.position, testing[i], Color.white, 5.0f);
        }

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

        AttachSheep();

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
        if (sheep.staticHoldingSheep == false)
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
