using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{

    public Camera_Rotation cameraRotation;

    public bool up, left, down, right;

    bool isComplete = false;
    
    void Start()
    {
        //cameraRotation = transform.parent.parent.GetChild(0).GetChild(0).GetComponent<Camera_Rotation>();
        cameraRotation = GameObject.Find("Main Camera").GetComponent<Camera_Rotation>();
    }


    public void Activate()
    {
        if (!isComplete)
        {
            cameraRotation.up = up;
            cameraRotation.left = left;
            cameraRotation.down = down;
            cameraRotation.right = right;
            Player player = GameObject.Find("/GameObject").GetComponent<Player>();
            player.sidesCompleted++;
            isComplete = true;
            
        }        
    }
}
