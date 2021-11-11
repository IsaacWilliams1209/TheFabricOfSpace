using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{

    public Camera_Rotation cameraRotation;

    public bool up, left, down, right;

    public float transitionTimer;

    bool isComplete = false;

    bool finishedRotating = false;

    bool RENAMELATER;

    float timer;

    Sheep currentSheep;
    
    Quaternion cameraStartingRotation;

    Vector3 cameraStartingPosition;

    void Start()
    {
        cameraRotation = GameObject.Find("/LavishPlanet/Planet_Rotation/Main Camera").GetComponent<Camera_Rotation>();
        Debug.Log("BerrySet");
    }

    void LateUpdate()
    {
        if (timer > transitionTimer && !RENAMELATER)
        {
            finishedRotating = !cameraRotation.isCubeRotating;
            isComplete = false;
        }

        if (isComplete)
        {
            //move camera on sheep towards the main camera
            CameraPan(cameraStartingPosition, cameraRotation.transform.position, cameraStartingRotation, cameraRotation.transform.rotation);

            if (timer > transitionTimer)
            {
                cameraRotation.up = up;
                cameraRotation.left = left;
                cameraRotation.down = down;
                cameraRotation.right = right;
                currentSheep.shepherd.SwapCams();
            }
            
        }
        if (finishedRotating && timer > transitionTimer && !RENAMELATER)
        {
            
            RaycastHit hit;
            Debug.DrawRay(cameraRotation.gameObject.transform.position, -cameraRotation.gameObject.transform.position, Color.red, 5.0f);
            if (Physics.Raycast(cameraRotation.gameObject.transform.position, -cameraRotation.gameObject.transform.position, out hit, Mathf.Infinity))
            {

                Debug.Log(hit.transform.name);
                Shepherd tempShepherd = hit.transform.GetComponentInChildren<Shepherd>();
                tempShepherd.awakeSheep[0].GetComponent<Sheep>().active = true;
                //tempShepherd.awakeSheep[0].transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material = currentSheep.sheepMaterials[0];
                tempShepherd.activeSheep = tempShepherd.awakeSheep[0];
                tempShepherd.awakeSheep.RemoveAt(0);
                currentSheep.awakeSheep.Insert(0, currentSheep.gameObject);
                //currentSheep.matChanger.material = currentSheep.sheepMaterials[1];
                currentSheep.active = false;
                tempShepherd.enabled = true;
                currentSheep.shepherd.enabled = false;
                isComplete = false;
                tempShepherd.SwapCams();
                tempShepherd.activeSheep.GetComponent<Sheep>().promtChanger.UpdateText(tempShepherd.activeSheep.GetComponent<Sheep>());
                currentSheep = tempShepherd.activeSheep.GetComponent<Sheep>();
                cameraStartingRotation = currentSheep.transform.GetChild(2).GetChild(1).rotation;
                cameraStartingPosition = currentSheep.transform.GetChild(2).GetChild(1).position;
                timer = 0;
                RENAMELATER = true;
            }
        }
        if (RENAMELATER && timer < transitionTimer)
        {
            CameraPan(cameraRotation.transform.position, cameraStartingPosition, cameraRotation.transform.rotation, cameraStartingRotation);
        }
    }

    public void Activate(Sheep sheep)
    {
        if (!isComplete)
        {
            
            
            Player player = GameObject.Find("/GameObject").GetComponent<Player>();
            player.sidesCompleted++;
            isComplete = true;
            currentSheep = sheep;
            cameraStartingRotation = currentSheep.transform.GetChild(2).GetChild(1).rotation;
            cameraStartingPosition = currentSheep.transform.GetChild(2).GetChild(1).position;
        }   
    }

    void CameraPan(Vector3 posFrom, Vector3 posTo, Quaternion rotFrom, Quaternion rotTo)
    {
        timer += Time.deltaTime;
        currentSheep.transform.GetChild(2).GetChild(1).position = Vector3.Lerp(posFrom, posTo, timer / transitionTimer);
        currentSheep.transform.GetChild(2).GetChild(1).rotation = Quaternion.Slerp(rotFrom, rotTo, timer / transitionTimer);
    }
}
