using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{

    public Camera_Rotation cameraRotation;



    public string nextFace;
    
    void Start()
    {
        //cameraRotation = transform.parent.parent.GetChild(0).GetChild(0).GetComponent<Camera_Rotation>();
        cameraRotation = GameObject.Find("Main Camera").GetComponent<Camera_Rotation>();
    }


    public void Activate()
    {
        cameraRotation.down = true;
    }
}
