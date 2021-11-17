using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlabSheep : MonoBehaviour
{
    [HideInInspector]
    public bool steppedOn = false;

    public void ActivatePowerUp(Sheep sheep)
    {
        RaycastHit hit;
        if (!sheep.poweredUp && Physics.BoxCast(transform.position + transform.up, new Vector3(0.15f, 0.1f, 0.15f), transform.up, out hit, transform.rotation, 1.0f, 1<<8))
        {
            sheep.poweredUp = true;
            return;
        }

        if (sheep.poweredUp)
        {
            // Move to ignore raycast layer
            gameObject.layer = 2;

            sheep.mainCollider.enabled = true;

            // Prevent movement and lock to tile
            sheep.canMove = false;

            Vector3 temp = transform.parent.right + transform.parent.forward;

            

            //Vector3 temp = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z)) + transform.parent.up;
            Vector3 newPos = Sheep.MaskVector(new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z)), temp);

            newPos += Sheep.MaskVector(new Vector3((int)transform.position.x, (int)transform.position.y, (int)transform.position.z), transform.parent.up) + transform.parent.up * 0.5f;

            transform.position = newPos;

            RaycastHit[] hits = new RaycastHit[4];

            Vector3[] directions = new Vector3[4];
            directions[0] = transform.parent.forward;
            directions[1] = transform.parent.right;
            directions[2] = -transform.parent.forward;
            directions[3] = -transform.parent.right;

            for (int i = 0; i < hits.Length; i++)
            {
                Debug.DrawRay(transform.position + transform.up * 0.1f, directions[i], Color.blue, 6.0f);

                int mask = ~((1 << 8) | (1 << 2));

                if (Physics.Raycast(transform.position + transform.up * 0.1f, directions[i], out hits[i], 2.0f, mask))
                {

                    if (hits[i].transform.tag == "Block" || hits[i].transform.tag == "Sheep")
                    {

                        // Update nearby blocks
                        gameObject.layer = 0;
                        Debug.Log(hits[i].transform.parent.name);
                        hits[i].transform.GetComponentInChildren<Block>().BlockUpdate();
                        // Debug.DrawRay(transform.position, directions[i])

                        gameObject.layer = 2;
                    }
                }
            }

            // Activate block on slab sheep
            sheep.transform.GetChild(2).GetChild(0).gameObject.SetActive(true);

            // Update block for the on the slab sheep
            sheep.transform.GetComponentInChildren<Block>().BlockUpdate();

            gameObject.layer = 0;

            sheep.transform.GetChild(2).GetChild(0).gameObject.layer = 0;

            sheep.materialHolder = sheep.matChanger.materials;
            sheep.materialHolder[1] = sheep.sheepMaterials[1];
            sheep.materialHolder[2] = sheep.sheepMaterials[1];
            sheep.materialHolder[0] = sheep.sheepMaterials[1];
            sheep.matChanger.materials = sheep.materialHolder;

            sheep.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().sharedMesh = sheep.meshes[1];
            transform.GetChild(0).rotation = GetComponent<SheepController>().startRotation;
            transform.GetChild(0).Rotate(transform.right, 180f);
            transform.GetChild(0).position += transform.up * 0.5f + transform.forward * 0.2f;

        }
        else
        {

            gameObject.layer = 2;

            transform.GetChild(2).GetChild(0).gameObject.layer = 2;

            // Set block on slab sheep to inactive                

            transform.GetChild(2).GetChild(0).gameObject.SetActive(false);

            RaycastHit[] hits = new RaycastHit[4];

            Vector3[] directions = new Vector3[4];
            directions[0] = transform.parent.forward;
            directions[1] = transform.parent.right;
            directions[2] = -transform.parent.forward;
            directions[3] = -transform.parent.right;

            for (int i = 0; i < hits.Length; i++)
            {

                Debug.DrawRay(transform.position + transform.up * 0.1f, directions[i], Color.blue, 6.0f);

                if (Physics.Raycast(transform.position + transform.up * 0.1f, directions[i], out hits[i], 2.0f, 1))
                {

                    if (hits[i].transform.tag == "Block" || hits[i].transform.tag == "Sheep")
                    {

                        // Update nearby blocks

                        hits[i].transform.GetComponentInChildren<Block>().BlockUpdate();

                    }
                }
            }
            // Release movement

            sheep.canMove = true;

            sheep.materialHolder = sheep.matChanger.materials;
            sheep.materialHolder[1] = sheep.sheepMaterials[0];
            sheep.materialHolder[2] = sheep.sheepMaterials[0];
            sheep.materialHolder[0] = sheep.sheepMaterials[0];
            sheep.matChanger.materials = sheep.materialHolder;

            transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().sharedMesh = sheep.meshes[0];

            transform.GetChild(0).rotation = transform.parent.rotation;
            transform.GetChild(0).position -= transform.up * 0.5f + transform.forward * 0.2f;

    sheep.mainCollider.enabled = false;
        }
    }
}
