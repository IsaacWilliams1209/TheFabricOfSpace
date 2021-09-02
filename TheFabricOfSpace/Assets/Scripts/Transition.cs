using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{

    public Camera_Rotation cameraRotation;

    public string nextFace;

    public void Activate()
    {
        cameraRotation.left = true;
    }
}
