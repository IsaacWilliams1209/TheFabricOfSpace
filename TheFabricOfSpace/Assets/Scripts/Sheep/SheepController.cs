using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepController : MonoBehaviour
{
    public float speed;

    public float rotationSpeed;

    public float gravity;

    public Vector3 sensorPos;

    Quaternion startRotation;

    Transform mesh;

    Vector3 currentGravity;

    bool grounded;
    Vector3 movementVector;

    void Start()
    {
        mesh = transform.GetChild(0);
        startRotation = mesh.rotation;
    }

    public void Move()
    {
        Gravity();
        SimpleMove();
        FinalMovement();
    }

    void Gravity()
    {
        Ray ray = new Ray(transform.TransformPoint(sensorPos), -transform.parent.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.distance < 0.55f  || hit.transform.tag == "Sheep")
                grounded = true;
            else
                grounded = false;
        }

        if (grounded)
        {
            transform.position = Sheep.MaskVector(transform.position, transform.parent.forward) + Sheep.MaskVector(hit.point, transform.parent.up) + Sheep.MaskVector(transform.position, transform.parent.right);
            currentGravity = Vector3.zero;
        }
        else
        {
            currentGravity = transform.parent.up * gravity;
        }

    }

    void SimpleMove()
    {
        Vector3 movement = Vector3.zero;
        // Move the player forward/backward
        movement += transform.parent.forward * Input.GetAxis("Vertical") * speed * Time.deltaTime;

        // Move the player left/right
        movement += transform.parent.right * Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            
        movementVector = CollisionCheck(movement);


    }

    void FinalMovement()
    {
        transform.position += transform.TransformDirection(movementVector + currentGravity);

        Vector3 rotation = Sheep.MaskVector(movementVector, transform.parent.right) + Sheep.MaskVector(movementVector, transform.parent.up) + Sheep.MaskVector(movementVector, transform.parent.forward);
        
        if (rotation != Vector3.zero)
        {
            Quaternion lookRotaion = Quaternion.LookRotation(rotation, transform.parent.up);
            
            mesh.rotation = Quaternion.RotateTowards(mesh.rotation, lookRotaion, rotationSpeed * Time.deltaTime);

            //Quaternion lookRotaion = Quaternion.LookRotation(rotation, transform.parent.up);
            //
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotaion, rotationSpeed * Time.deltaTime);
        }
    }

    Vector3 CollisionCheck(Vector3 dir)
    {
        Vector3 d = transform.TransformDirection(dir);
        Vector3 l = transform.TransformPoint(sensorPos);

        Ray ray = new Ray(l, d);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 2))
        {
            if (hit.distance < 0.4f)
            {
                Vector3 temp = Vector3.Cross(hit.normal, d);
                Vector3 newDir = Vector3.Cross(temp, hit.normal);

                RaycastHit wallCheck = CheckWall(newDir);
                if (wallCheck.transform != null)
                {
                    newDir *= wallCheck.distance * 0.5f;
                }

                transform.position += newDir;
                return Vector3.zero;
            }
        }
        return dir;
    }

    RaycastHit CheckWall(Vector3 dir)
    {
        Vector3 l = transform.TransformPoint(sensorPos);
        Ray ray = new Ray(l, dir);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 0.1f))
        {
            return hit;
        }
        return hit;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.TransformPoint(sensorPos), new Vector3(0.1f, 0.1f, 0.1f));
    }
}
