using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> These vectors will be open for the designers to change to their desire. The camera rotation/position changes work 
/// by having the script track the current face and the target face the user wants to swap to and than passes in the rotation/position of that 
/// target face for the camera to Slerp towards.
/// 
/// As a rule of thumb the positon should be a consistent number with just the x,y,z component being changed with the number as a positive/negative.
/// </summary>
public class Camera_Rotation : MonoBehaviour
{
    [SerializeField]
    private Vector3 camRot01 = new Vector3(90, 0, 0);

    [SerializeField]
    private Vector3 camPos01 = new Vector3(0, 25, 0);

    [SerializeField]
    private Vector3 camRot02 = new Vector3(0, 90, 0);

    [SerializeField]
    private Vector3 camPos02 = new Vector3(-25, 0, 0);

    [SerializeField]
    private Vector3 camRot03 = new Vector3(-90, 0, 0);

    [SerializeField]
    private Vector3 camPos03 = new Vector3(0, -25, 0);

    [SerializeField]
    private Vector3 camRot04 = new Vector3(0, -90, 0);

    [SerializeField]
    private Vector3 camPos04 = new Vector3(0, 25, 0);

    [SerializeField]
    private Vector3 camRot05 = new Vector3(0, -180, 0);

    [SerializeField]
    private Vector3 camPos05 = new Vector3(0, 0, 25);

    [SerializeField]
    private Vector3 camRot06 = new Vector3(0, 0, 0);

    [SerializeField]
    private Vector3 camPos06 = new Vector3(0, 0, -25);

    [SerializeField]
    private float rotSpeed = 50.0f;

    ///<summary> Tells the update method to rotate/move the camera around the planet on request until interpolation is finished.</summary>
    private bool isCubeRotating = false;
    /// <summary> Allows for the script to track the correct camera position/rotation for the current planet face and the target planet face.
    /// </summary>
    private Vector3 targRotation;
    private Vector3 targPosition;
    private Vector3 currRotation;
    private Vector3 currPosition;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && !isCubeRotating)
        {
            Debug.Log("W Press");
            isCubeRotating = true;
            CheckCurrentFace(ref targRotation, ref targPosition);
            PlanetRotation(targRotation, targPosition);
        }

        if (Input.GetKeyDown(KeyCode.A) && !isCubeRotating)
        {
            isCubeRotating = true;
            CheckCurrentFace(ref targRotation, ref targPosition);
            PlanetRotation(targRotation, targPosition);
        }
        if (Input.GetKeyDown(KeyCode.S) && !isCubeRotating)
        {
            isCubeRotating = true;
            CheckCurrentFace(ref targRotation, ref targPosition);
            PlanetRotation(targRotation, targPosition);
        }
        if (Input.GetKeyDown(KeyCode.D) && !isCubeRotating)
        {
            isCubeRotating = true;
            CheckCurrentFace(ref targRotation, ref targPosition);
            PlanetRotation(targRotation, targPosition);
        }

        if (isCubeRotating)
        {
            PlanetRotation(targRotation, targPosition);
        }
    }

    /// <summary> Will rotate and position the camera to the correct face of the planet dependent on user request by Slerping between the 
    /// cameras current rotation/position and the target rotation/position
    /// </summary>
    private void PlanetRotation(Vector3 targetRot, Vector3 targetPos)
    {
        float angle = Quaternion.Angle(transform.rotation, Quaternion.Euler(targetRot));
        float timeToComplete = angle / rotSpeed;
        float donePercentage = Mathf.Min(1.0f, Time.deltaTime / timeToComplete);

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(targetRot), donePercentage);
        transform.position = Vector3.Slerp(transform.position, targetPos, donePercentage);

        if (donePercentage >= 1.0f)
        {
            currRotation = targRotation;
            currPosition = targPosition;
            isCubeRotating = false;
        }
    }
    private void CheckCurrentFace(ref Vector3 targRot, ref Vector3 targPos)
    {
        if (currRotation == camRot01)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                targRot = camRot05;
                targPos = camPos05;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                targRot = camRot02;
                targPos = camPos02;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                targRot = camRot06;
                targPos = camPos06;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                targRot = camRot04;
                targPos = camPos04;
            }
        }
        if (currRotation == camRot02)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                targRot = camRot01;
                targPos = camPos01;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                targRot = camRot05;
                targPos = camPos05;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                targRot = camRot03;
                targPos = camPos03;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                targRot = camRot06;
                targPos = camPos06;
            }
        }
        if (currRotation == camRot03)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                targRot = camRot06;
                targPos = camPos06;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                targRot = camRot04;
                targPos = camPos04;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                targRot = camRot05;
                targPos = camPos05;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                targRot = camRot02;
                targPos = camPos02;
            }
        }
        if (currRotation == camRot04)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                targRot = camRot01;
                targPos = camPos01;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                targRot = camRot06;
                targPos = camPos06;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                targRot = camRot03;
                targPos = camPos03;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                targRot = camRot05;
                targPos = camPos05;
            }
        }
        if (currRotation == camRot05)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                targRot = camRot01;
                targPos = camPos01;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                targRot = camRot04;
                targPos = camPos04;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                targRot = camRot03;
                targPos = camPos03;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                targRot = camRot02;
                targPos = camPos02;
            }
        }
        if (currRotation == camRot06)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                targRot = camRot01;
                targPos = camPos01;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                targRot = camRot02;
                targPos = camPos02;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                targRot = camRot03;
                targPos = camPos03;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                targRot = camRot04;
                targPos = camPos04;
            }
        }
        targRot = targRotation;
        targPos = targPosition;
    }
}
