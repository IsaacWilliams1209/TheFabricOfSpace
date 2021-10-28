using UnityEngine;

public class StaticSheep : Sheep
{

    Sheep grabbedSheep;
    Sheep additonalSheep;
    bool hasSheep = false;
    Vector3 attachedSide;
    Vector3 frontSide, rightSide, backSide, leftSide;
    bool updateFrontSide, updateRightSide, updateBackSide, updateLeftSide;

    private void SideUpdate()
    {
        frontSide = transform.GetChild(0).forward;
        rightSide = transform.GetChild(0).right;
        backSide  = -transform.GetChild(0).forward;
        leftSide  = -transform.GetChild(0).right;

        if (updateFrontSide) { attachedSide = frontSide; }
        else if (updateRightSide) { attachedSide = rightSide * 0.5f; }
        else if (updateBackSide) { attachedSide = backSide; }
        else if (updateLeftSide) { attachedSide = leftSide * 0.5f; }
    }

    private Vector3 AttachSheep()
    {

        SideUpdate();

        Vector3 displacementToSheep = grabbedSheep.transform.position - transform.position;
        displacementToSheep = Vector3.Normalize(displacementToSheep);

        float frontValue, rightValue, backValue, leftValue;

        frontValue = Vector3.Dot(frontSide, displacementToSheep);
        rightValue = Vector3.Dot(rightSide, displacementToSheep);
        backValue = Vector3.Dot(backSide, displacementToSheep);
        leftValue = Vector3.Dot(leftSide, displacementToSheep);

        updateFrontSide = false;
        updateRightSide = false;
        updateBackSide = false;
        updateLeftSide = false;

        if (frontValue > rightValue && frontValue > backValue && frontValue > leftValue)      { updateFrontSide = true;  return frontSide;  }
        else if (rightValue > frontValue && rightValue > backValue && rightValue > leftValue) { updateLeftSide  = true;  return rightSide;  }
        else if (backValue > frontValue && backValue > rightValue && backValue > leftValue)   { updateBackSide  = true;  return backSide;   }
        else if (leftValue > frontValue && leftValue > rightValue && leftValue > backValue)   { updateLeftSide  = true;  return leftSide;   }

        return frontSide;

    }

    private void Update()
    {
        if (grabbedSheep != null)
        {
            if (hasSheep)
            {
                grabbedSheep.transform.position += GetComponent<SheepController>().movementVector;
                grabbedSheep.transform.GetChild(0).rotation = transform.GetChild(0).rotation;
                SideUpdate();
                grabbedSheep.transform.position = transform.position + attachedSide;
                if(additonalSheep != null)
                {
                    additonalSheep.transform.position = grabbedSheep.transform.position + grabbedSheep.transform.up;
                    additonalSheep.transform.GetChild(0).rotation = grabbedSheep.transform.GetChild(0).rotation;
                }
            }
        }
    }

    public void ActivatePowerUp(Sheep sheep)
    {

        if (grabbedSheep == null)
        {
            sheep.poweredUp = false;
            sheep.staticHoldingSheep = false;
            return;
        }

        else if (grabbedSheep.awake == false)
        {
            sheep.poweredUp = false;
            sheep.staticHoldingSheep = false;
            return;
        }

        attachedSide = AttachSheep();
        grabbedSheep.transform.position = sheep.transform.position + attachedSide;
        hasSheep = true;
        sheep.staticHoldingSheep = true;

        if (grabbedSheep.sheepType == SheepType.Slab)
        {
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(grabbedSheep.transform.position, grabbedSheep.transform.up, out hit, 3.0f, 1 << 2))
            {
                additonalSheep = hit.transform.GetComponent<Sheep>();
            }
        }

        grabbedSheep.gameObject.layer = 2;

    }


    public void DeActivatePowerUp(Sheep sheep)
    {
        if (sheep.staticHoldingSheep == false)
        {
            return;
        }

        RaycastHit hit = new RaycastHit();

        Debug.DrawRay(grabbedSheep.transform.position, -grabbedSheep.transform.up, Color.white, 3.0f);
        if(Physics.Raycast(grabbedSheep.transform.position, -grabbedSheep.transform.up, out hit, 2.0f))
        {
            if(hit.transform.tag == "Water")
            {
                Debug.Log("not safe to place sheep");
            }
            else
            {
                hasSheep = false;
                grabbedSheep.gameObject.layer = 0;
                grabbedSheep = null;
                sheep.staticHoldingSheep = false;
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
