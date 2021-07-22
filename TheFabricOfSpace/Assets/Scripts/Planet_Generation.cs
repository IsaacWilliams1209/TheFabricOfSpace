using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet_Generation : MonoBehaviour
{
    /// <summary> Array that will hold 6 prefabs created by the designers and than will spawn a planet by iterating through each prefab spawing it into the scene
    /// </summary>
    private GameObject[] planetFaces;

    /// <summary> These faces are the game prefabs that the designers feed there levels into and the code uses them to create a new planet. 
    ///</summary>
    [SerializeField]
    private GameObject face01;
    [SerializeField]
    private GameObject face02;
    [SerializeField]
    private GameObject face03;
    [SerializeField]
    private GameObject face04;
    [SerializeField]
    private GameObject face05;
    [SerializeField]
    private GameObject face06;
    [SerializeField]
    private Vector3 spawnPosition;
    [SerializeField]
    private Vector3 spawnRotation;

    private int maxFaceAmount = 6;
    private int currFace = 0;

    private void Start()
    {
        planetFaces = new GameObject[] { face01, face02, face03, face04, face05, face06 };
        //GameObject newFace = Instantiate(face01, planetOrigin, Quaternion.Euler(faceSpawnRotation)) as GameObject;
        PlanetSpawn();
    }

    private void PlanetSpawn()
    {
        for (currFace = 0; currFace < maxFaceAmount; currFace++)
        {
            SpawnManager(ref spawnPosition, ref spawnRotation);
            GameObject newFace = Instantiate(planetFaces[currFace], spawnPosition, Quaternion.Euler(spawnRotation)) as GameObject;
        }
    }

    private void SpawnManager(ref Vector3 spawnPosition, ref Vector3 spawnRotation)
    {
        //Planet face 1
        if(currFace == 0)
        {
            Debug.Log("0");
            FaceReset(ref spawnPosition, ref spawnRotation);
            spawnPosition.y = 6;
        }
        //Planet face 2
        if (currFace == 1)
        {
            Debug.Log("1");
            FaceReset(ref spawnPosition, ref spawnRotation);
            spawnRotation.z = 90;
        }
        //Planet face 3
        if (currFace == 2)
        {
            Debug.Log("2");
            FaceReset(ref spawnPosition, ref spawnRotation);
            spawnPosition.z = -6;
            spawnRotation.x = 180;
        }
        //Planet face 4
        if (currFace == 3)
        {
            Debug.Log("3");
            FaceReset(ref spawnPosition, ref spawnRotation);
            spawnPosition.x = 6;
            spawnPosition.y = 6;
            spawnRotation.z = -90;
        }
        //Planet face 5
        if (currFace == 4)
        {
            Debug.Log("4");
            FaceReset(ref spawnPosition, ref spawnRotation);
            spawnRotation.x = 90;
        }
        //Planet face 6
        if (currFace == 5)
        {
            Debug.Log("5");
            FaceReset(ref spawnPosition, ref spawnRotation);
            spawnPosition.y = 6;
            spawnPosition.z = -6;
            spawnRotation.x = -90;
        }
    }

    /// <summary> Reset the 3 components in the Vector3 variables for the 'spawnPosition' and 'spawnRotation' before making the necessary changes for the correct
    /// planet face spawn positon/roation.
    /// </summary>
    private void FaceReset(ref Vector3 spawnPosition, ref Vector3 spawnRotation)
    {
        spawnPosition.x = 0;
        spawnPosition.y = 0;
        spawnPosition.z = 0;

        spawnRotation.x = 0;
        spawnRotation.y = 0;
        spawnRotation.z = 0;
    }
}
