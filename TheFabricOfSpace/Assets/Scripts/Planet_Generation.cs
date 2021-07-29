using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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
    private Vector3 spawnOrigin;
    [SerializeField]
    private int gridSize;

    private Vector3 spawnPosition;
    private Vector3 spawnRotation;

    private int maxFaceAmount = 6;
    private int currFace = 0;

    private void Start()
    {
        planetFaces = new GameObject[] { face01, face02, face03, face04, face05, face06 };
        PlanetSpawn();
    }

    private void PlanetSpawn()
    {
        for (currFace = 0; currFace < maxFaceAmount; currFace++)
        {
            SpawnManager(ref spawnPosition, ref spawnRotation);
            GameObject temp = Instantiate(planetFaces[currFace], transform.position + spawnPosition, Quaternion.Euler(spawnRotation));
            temp.name = "Planet Face " + (currFace + 1);
            temp.transform.parent = gameObject.transform;
        }
    }

    private void SpawnManager(ref Vector3 spawnPosition, ref Vector3 spawnRotation)
    {
        FaceReset(ref spawnPosition, ref spawnRotation);
        switch (currFace)
        {
            case 0:
                spawnPosition.y = (gridSize - 1) / 2;
                break;
            case 1:
                spawnPosition.x -= (gridSize - 1) / 2;
                spawnRotation.z = 90;
                break;
            case 2:
                spawnPosition.y -= (gridSize - 1) / 2;
                spawnRotation.x = 180;
                break;
            case 3:
                spawnPosition.x = (gridSize - 1) / 2;
                spawnRotation.z = -90;
                break;
            case 4:
                spawnPosition.z = (gridSize - 1) / 2;
                spawnRotation.x = 90;
                break;
            case 5:
                spawnPosition.z -= (gridSize - 1) / 2;
                spawnRotation.x = -90;
                break;
            default:
                break;
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
