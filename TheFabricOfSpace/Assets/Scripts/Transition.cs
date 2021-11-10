using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{

    public Camera_Rotation cameraRotation;

    public bool up, left, down, right;

    bool isComplete = false;

    bool finishedRotating = false;

    Sheep currentSheep;
    
    void Start()
    {
        //cameraRotation = transform.parent.parent.GetChild(0).GetChild(0).GetComponent<Camera_Rotation>();
        cameraRotation = GameObject.Find("/LavishPlanet/Planet_Rotation/Main Camera").GetComponent<Camera_Rotation>();
        Debug.Log("BerrySet");
    }

    void LateUpdate()
    {
        finishedRotating = !cameraRotation.isCubeRotating;
        if (isComplete && finishedRotating)
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
            }
        }
    }

    public void Activate(Sheep sheep)
    {
        if (!isComplete)
        {
            sheep.shepherd.SwapCams();
            cameraRotation.up = up;
            cameraRotation.left = left;
            cameraRotation.down = down;
            cameraRotation.right = right;
            Player player = GameObject.Find("/GameObject").GetComponent<Player>();
            player.sidesCompleted++;
            isComplete = true;
            currentSheep = sheep;
        }   
    }
}
