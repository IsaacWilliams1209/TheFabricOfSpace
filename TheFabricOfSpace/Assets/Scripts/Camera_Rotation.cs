using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class FaceInformation
{
    public int upDir;
    public int leftDir;
    public int downDir;
    public int rightDir;
    public Vector3 camOrientation;
}

public class Camera_Rotation : MonoBehaviour
{   
    List<FaceInformation> planetFaces = new List<FaceInformation>(6);

    /// <summary>Allows for the designer to play around with how fair they want the camera from the planet and the code will adapt to the given value </summary>
    [SerializeField]
    private float distFromPlanet;

    /// <summary>Allows for the designer to play around with what angle they want the camera and the code will adapt to the given value </summary>
    [SerializeField]
    private float cameraRotation;

    /// <summary> Will change the speed of how fast the camera interpolates between planet faces. The higher it is the faster it is</summary>
    [SerializeField]
    private float rotSpeed = 50.0f;

    ///<summary> Tells the update method to rotate/move the camera around the planet on request until interpolation is finished.</summary>
    private bool isCubeRotating = false;

    ///<summary> Tells the update method to rotate/move the camera around the planet on request until interpolation is finished.</summary>
    private int currFace;
    private int targetFace;

    private Vector3 camPos = new Vector3(0, 0, 0);
    private Vector3 camRot = new Vector3(0, 0, 0);
    private Vector3 camStartPos = new Vector3(0, 0, 0);
    private Vector3 camStartRot = new Vector3(0, 0, 0);

    private void Start()
    {
        camStartPos.Set(gameObject.transform.parent.position.x, gameObject.transform.parent.position.y + distFromPlanet, gameObject.transform.parent.position.z);
        camStartRot.Set(0, -cameraRotation, 0);
        transform.position = camStartPos;
        transform.parent.rotation = Quaternion.Euler(camStartRot);
        currFace = 0;
        FaceSetUp();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && !isCubeRotating)
        {
            isCubeRotating = true;
            targetFace = planetFaces[currFace].upDir;
            camRot = planetFaces[targetFace].camOrientation;
            currFace = targetFace;
        }
        else if (Input.GetKeyDown(KeyCode.A) && !isCubeRotating)
        {
            isCubeRotating = true;
            targetFace = planetFaces[currFace].leftDir;
            camRot = planetFaces[targetFace].camOrientation;
            currFace = targetFace;
        }
        else if (Input.GetKeyDown(KeyCode.S) && !isCubeRotating)
        {
            isCubeRotating = true;
            targetFace = planetFaces[currFace].downDir;
            camRot = planetFaces[targetFace].camOrientation;
            currFace = targetFace;
        }
        else if (Input.GetKeyDown(KeyCode.D) && !isCubeRotating)
        {
            isCubeRotating = true;
            targetFace = planetFaces[currFace].rightDir;
            camRot = planetFaces[targetFace].camOrientation;
            currFace = targetFace;
        }

        if (isCubeRotating)
        {
            PlanetRotation(camRot, camPos);
        }
    }

    /// <summary> Will rotate and position the camera to the correct face of the planet dependent on user request by Slerping between the 
    /// cameras current rotation/position and the target rotation/position
    /// </summary>
    private void PlanetRotation(Vector3 camRotation, Vector3 camPosition)
    {

        float angle = Quaternion.Angle(transform.parent.rotation, Quaternion.Euler(camRotation));
        float timeToComplete = angle / rotSpeed;
        float donePercentage = Mathf.Min(1.0f, Time.deltaTime / timeToComplete);

        transform.parent.rotation = Quaternion.Slerp(transform.parent.rotation, Quaternion.Euler(camRotation), donePercentage);

        if (donePercentage >= 1.0f)
        {
            isCubeRotating = false;
        }
    }


    private void FaceSetUp()
    {
        for (int i = 0; i < 6; i++)
        {
            FaceInformation newFace = new FaceInformation();
            planetFaces.Add(newFace);
        }


        /////<summary> direction key: x = up , y = left , z = down , w = right 
        /////          Make sure that when you are entering the direction face that you -1 as we are indexing into a list.</summary>
        planetFaces[0].upDir = 4;
        planetFaces[0].leftDir = 1;
        planetFaces[0].downDir = 5;
        planetFaces[0].rightDir = 3;
        planetFaces[0].camOrientation.Set(0, -cameraRotation, 0);

        planetFaces[1].upDir = 0;
        planetFaces[1].leftDir = 4;
        planetFaces[1].downDir = 2;
        planetFaces[1].rightDir = 5;
        planetFaces[1].camOrientation.Set(-cameraRotation, 0, 0);

        planetFaces[2].upDir = 5;
        planetFaces[2].leftDir = 1;
        planetFaces[2].downDir = 4;
        planetFaces[2].rightDir = 3;
        planetFaces[2].camOrientation.Set(-cameraRotation * 2, -cameraRotation, 0);

        planetFaces[3].upDir = 0;
        planetFaces[3].leftDir = 5;
        planetFaces[3].downDir = 2;
        planetFaces[3].rightDir = 4;
        planetFaces[3].camOrientation.Set(-cameraRotation, 0, cameraRotation * 2);

        planetFaces[4].upDir = 0;
        planetFaces[4].leftDir = 3;
        planetFaces[4].downDir = 2;
        planetFaces[4].rightDir = 1;
        planetFaces[4].camOrientation.Set(-cameraRotation, 0, cameraRotation);

        planetFaces[5].upDir = 0;
        planetFaces[5].leftDir = 1;
        planetFaces[5].downDir = 2;
        planetFaces[5].rightDir = 3;
        planetFaces[5].camOrientation.Set(-cameraRotation, 0, 0 - cameraRotation);

    }


}
