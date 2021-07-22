using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary> This scripts purpose is to test different ways to tackle the camera rotating around the cube. In its current state it is very
/// much boiler plater code and will be cleaned up/optimized at a later date. 
/// </summary>
public class Cube_Rotation : MonoBehaviour
{
    /// <summary> TODO:
    ///             .Open up variables to the desingers so they can play with rotation speed, clamp values, etc.
    ///             .Find a way to cut down on the amount of 'if' checks.
    /// </summary>
    [SerializeField]
    GameObject cubeFace_01;

    [SerializeField]
    GameObject cubeFace_02;

    [SerializeField]
    GameObject cubeFace_03;

    [SerializeField]
    GameObject cubeFace_04;

    [SerializeField]
    GameObject cubeFace_05;

    [SerializeField]
    GameObject cubeFace_06;

    private GameObject currFace; //Tracks what the current face of the planet the camera is pointing towards is
    private GameObject targetFace; //This tells the cube manager what face we want to rotate towards depending on current face and user key input.

    [SerializeField]
    private float rotSpeed = 15.0f; //Gives the designer a way to speed up or slow down the rotation speed of the camera.

    bool isCubeRotating = false; //Allows for us to have a single key press happen and than the world rotates until it is done uninteruppted.

    public void Start()
    {
        currFace = cubeFace_01;
    }

    public void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.W) && !isCubeRotating)
        {
            targetFace = CurrentFaceCheck(currFace);
            isCubeRotating = true;
            DebugCubeFace(currFace, targetFace);
            CubeRotation(targetFace);
        }
        if (Input.GetKeyDown(KeyCode.A) && !isCubeRotating)
        {
            targetFace = CurrentFaceCheck(currFace);
            isCubeRotating = true;
            DebugCubeFace(currFace, targetFace);
            CubeRotation(targetFace);
        }
        if (Input.GetKeyDown(KeyCode.S) && !isCubeRotating)
        {
            targetFace = CurrentFaceCheck(currFace);
            isCubeRotating = true;
            DebugCubeFace(currFace, targetFace);
            CubeRotation(targetFace);
        }
        if (Input.GetKeyDown(KeyCode.D) && !isCubeRotating)
        {
            targetFace = CurrentFaceCheck(currFace);
            isCubeRotating = true;
            DebugCubeFace(currFace, targetFace);
            CubeRotation(targetFace);
        }

        else if (isCubeRotating)
        {
            CubeRotation(targetFace);
        }
    }

    private void DebugCubeFace(GameObject a_currFace, GameObject a_targetFace)
    {
        Debug.ClearDeveloperConsole();
        Debug.Log(a_currFace);
        Debug.Log(a_targetFace);
    }

    /// <summary> This function works by passing in the gameObject containing the correct rotation and position for the camera to 
    /// slerp towards.
    /// Will need to be updated to check if the rotation/position is X range from its destination and if true clamp the current value to the
    /// destination values.
    /// </summary>
    private void CubeRotation(GameObject targetFace)
    {

        float angle = Quaternion.Angle(transform.rotation, Quaternion.Euler(targetFace.transform.eulerAngles));
        float timeToComplete = angle / rotSpeed;
        float donePercentage = Mathf.Min(1f, Time.deltaTime / timeToComplete);

        //This should interpolate towards the rotation of the cubeFace_02 x,y,z rotation components.
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(targetFace.transform.eulerAngles), donePercentage);
        //This should interpolate towards the position of the cubeFace_02 x,y,z position components.
        transform.position = Vector3.Slerp(transform.position, targetFace.transform.position, donePercentage);

        if(donePercentage >= 1.0f)
        {
            currFace = targetFace;
            isCubeRotating = false;
        }

    }


    /// <summary> Works by checking what face of the cube the camera is currently looking at. Than dependent on what that face is the 
    /// code will figure out which target face to feed back to the rotation function dependent on user input. E.G If the current face is
    /// cubeFace_01 and the user wants to rotate the cube downwards it will give back the target cubeFace_06.
    /// 
    /// Currently is working through far to many 'if' checks. Will find a way to clean this up once I get the cube rotating.
    /// </summary>
    private GameObject CurrentFaceCheck(GameObject currFace)
    {
        if (currFace == cubeFace_01)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                targetFace = cubeFace_05;
                return targetFace;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                targetFace = cubeFace_02;
                return targetFace;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                targetFace = cubeFace_06;
                return targetFace;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                targetFace = cubeFace_04;
                return targetFace;
            }
        }
        if (currFace == cubeFace_02)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                targetFace = cubeFace_01;
                return targetFace;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                targetFace = cubeFace_05;
                return targetFace;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                targetFace = cubeFace_03;
                return targetFace;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                targetFace = cubeFace_06;
                return targetFace;
            }
        }
        if (currFace == cubeFace_03)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                targetFace = cubeFace_06;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                targetFace = cubeFace_04;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                targetFace = cubeFace_05;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                targetFace = cubeFace_02;
            }
        }
        if (currFace == cubeFace_04)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                targetFace = cubeFace_01;
                return targetFace;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                targetFace = cubeFace_06;
                return targetFace;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                targetFace = cubeFace_03;
                return targetFace;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                targetFace = cubeFace_05;
                return targetFace;
            }
        }
        if (currFace == cubeFace_05)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                targetFace = cubeFace_01;
                return targetFace;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                targetFace = cubeFace_04;
                return targetFace;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                targetFace = cubeFace_03;
                return targetFace;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                targetFace = cubeFace_02;
                return targetFace;
            }
        }
        if (currFace == cubeFace_06)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                targetFace = cubeFace_01;
                return targetFace;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                targetFace = cubeFace_02;
                return targetFace;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                targetFace = cubeFace_03;
                return targetFace;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                targetFace = cubeFace_04;
                return targetFace;
            }
        }
        return targetFace;
    }

}
