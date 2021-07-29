using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class FaceDirections
{
    public Vector4 directionData;
}

class FaceInformation
{
    public Vector3 camPosition;
    public Vector3 camRotation;
}


public class Camera_Rotation : MonoBehaviour
{   
    List<FaceDirections> planetFace = new List<FaceDirections>(6);
    List<FaceInformation> camInfo = new List<FaceInformation>(6);

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

    private void Start()
    {
        camStartPos.Set(gameObject.transform.parent.position.x, gameObject.transform.parent.position.y + distFromPlanet, gameObject.transform.parent.position.z);
        transform.position = camStartPos;
        currFace = 0;
        FaceSetUp();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && !isCubeRotating)
        {
            isCubeRotating = true;
            targetFace = (int)planetFace[currFace].directionData.x;
            camPos = camInfo[targetFace].camPosition;
            camRot = camInfo[targetFace].camRotation;
            currFace = targetFace;
        }
        else if (Input.GetKeyDown(KeyCode.A) && !isCubeRotating)
        {
            isCubeRotating = true;
            targetFace = (int)planetFace[currFace].directionData.y;
            camPos = camInfo[targetFace].camPosition;
            camRot = camInfo[targetFace].camRotation;
            currFace = targetFace;
        }
        else if (Input.GetKeyDown(KeyCode.S) && !isCubeRotating)
        {
            isCubeRotating = true;
            targetFace = (int)planetFace[currFace].directionData.z;
            camPos = camInfo[targetFace].camPosition;
            camRot = camInfo[targetFace].camRotation;
            currFace = targetFace;
        }
        else if (Input.GetKeyDown(KeyCode.D) && !isCubeRotating)
        {
            isCubeRotating = true;
            targetFace = (int)planetFace[currFace].directionData.w;
            camPos = camInfo[targetFace].camPosition;
            camRot = camInfo[targetFace].camRotation;
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
        float angle = Quaternion.Angle(transform.rotation, Quaternion.Euler(camRotation));
        float timeToComplete = angle / rotSpeed;
        float donePercentage = Mathf.Min(1.0f, Time.deltaTime / timeToComplete);

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(camRotation), donePercentage);
        transform.position = Vector3.Slerp(transform.position, camPosition, donePercentage);

        if (donePercentage >= 1.0f)
        {
            isCubeRotating = false;
        }
    }


    private void FaceSetUp()
    {
        for (int i = 0; i < 6; i++)
        {
            FaceDirections newFaceDir = new FaceDirections();
            FaceInformation newFaceInfo = new FaceInformation(); 
            planetFace.Add(newFaceDir);
            camInfo.Add(newFaceInfo);
        }
        
        ///<summary> direction key: x = up , y = left , z = down , w = right 
        ///          Make sure that when you are entering the direction face that you -1 as we are indexing into a list.</summary>
        planetFace[0].directionData.Set(4, 1, 5, 3); //Face 01 directions
        planetFace[1].directionData.Set(0, 4, 2, 5); //Face 02 directions
        planetFace[2].directionData.Set(5, 1, 4, 3); //Face 03 directions
        planetFace[3].directionData.Set(0, 5, 4, 2); //Face 04 directions
        planetFace[4].directionData.Set(0, 3, 2, 1); //Face 05 directions
        planetFace[5].directionData.Set(0, 1, 2, 3); //Face 06 directions

        //Face 01
        camInfo[0].camPosition.Set(transform.position.x, distFromPlanet, transform.position.z);
        camInfo[0].camRotation.Set(cameraRotation, 0 , 0);

        //Face 02
        camInfo[1].camPosition.Set(-distFromPlanet, transform.position.y - distFromPlanet, transform.position.z);
        camInfo[1].camRotation.Set(0, cameraRotation, 0);

        //Face 03
        camInfo[2].camPosition.Set(transform.position.x, -distFromPlanet, transform.position.z);
        camInfo[2].camRotation.Set(-cameraRotation, 0, 0);

        //Face 04
        camInfo[3].camPosition.Set(distFromPlanet, transform.position.y - distFromPlanet, transform.position.z);
        camInfo[3].camRotation.Set(0, -cameraRotation, 0);

        //Face 05
        camInfo[4].camPosition.Set(transform.position.z, transform.position.y - distFromPlanet, distFromPlanet);
        camInfo[4].camRotation.Set(0, -cameraRotation * 2, 0);

        //Face 06
        camInfo[5].camPosition.Set(transform.position.z, transform.position.y - distFromPlanet, -distFromPlanet);
        camInfo[5].camRotation.Set(0, 0, 0);

    }


}
