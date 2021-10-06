﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlabSheep : MonoBehaviour
{
    public void ActivatePowerUp(Sheep sheep)
    {
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
            newPos += Sheep.MaskVector(new Vector3(Mathf.Floor(transform.position.x), Mathf.Floor(transform.position.y), Mathf.Floor(transform.position.z)), transform.parent.up) + transform.parent.up * 0.5f;



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

                if (Physics.Raycast(transform.position + transform.up * 0.1f, directions[i], out hits[i], 2.0f))
                {

                    if (hits[i].transform.tag == "Block" || hits[i].transform.tag == "Sheep")
                    {

                        // Update nearby blocks
                        Debug.Log(hits[i].transform.name);
                        gameObject.layer = 0;

                        hits[i].transform.GetComponentInChildren<Block>().BlockUpdate();
                        // Debug.DrawRay(transform.position, directions[i])

                        gameObject.layer = 2;
                    }
                }
            }

            // Activate block on slab sheep

            sheep.transform.GetChild(0).gameObject.SetActive(true);

            // Update block for the on the slab sheep

            sheep.transform.GetComponentInChildren<Block>().BlockUpdate();

            gameObject.layer = 0;

            sheep.transform.GetChild(0).gameObject.layer = 0;

            sheep.transform.GetChild(2).GetComponent<MeshFilter>().mesh = sheep.meshes[1];

        }
        else
        {

            gameObject.layer = 2;

            transform.GetChild(0).gameObject.layer = 2;

            // Set block on slab sheep to inactive                

            transform.GetChild(0).gameObject.SetActive(false);

            RaycastHit[] hits = new RaycastHit[4];

            Vector3[] directions = new Vector3[4];

            directions[0] = transform.parent.forward;

            directions[1] = transform.parent.right;

            directions[2] = -transform.parent.forward;

            directions[3] = -transform.parent.right;

            for (int i = 0; i < hits.Length; i++)
            {

                Debug.DrawRay(transform.position + transform.up * 0.1f, directions[i], Color.blue, 6.0f);

                if (Physics.Raycast(transform.position + transform.up * 0.1f, directions[i], out hits[i], 2.0f))
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

            transform.GetChild(2).GetComponent<MeshFilter>().mesh = sheep.meshes[0];

            sheep.mainCollider.enabled = false;
        }
    }
}
