using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepController : MonoBehaviour
{
    public float speed;

    public float rotationSpeed;

    public float gravity;

    public Vector3 sensorPos;

    Vector3 currentGravity;

    bool grounded;
    Vector3 movementVector;

    public void Move()
    {
        Gravity();
        SimpleMove();
        FinalMovement();
    }

    void Gravity()
    {
        Vector3 boxPos = transform.position;
        Vector3 boxSize = Vector3.one * 0.5f;
        Ray ray = new Ray(transform.TransformPoint(sensorPos), -transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Debug.Log("Hit distance: " + hit.distance);
            if (hit.distance > 0.41f)
                grounded = false;
            else
                grounded = true;
        }

        if (grounded)
        {
            transform.position = VectorMask(transform.position, transform.parent.forward) + VectorMask(hit.point, transform.parent.up) + VectorMask(transform.position, transform.parent.right);
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
        
       // Vector3 rotation = VectorMask(movement, transform.parent.right) + VectorMask(movement, transform.parent.up) + VectorMask(movement, transform.parent.forward);
       //
       // if (rotation != Vector3.zero)
       // {
       //     Quaternion lookRotaion = Quaternion.LookRotation(rotation, transform.parent.up);
       //
       //     transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotaion, rotationSpeed * Time.deltaTime);
       // }
    }

    void FinalMovement()
    {
        transform.position += transform.TransformDirection(movementVector + currentGravity);
    }

    Vector3 CollisionCheck(Vector3 dir)
    {
        Vector3 d = transform.TransformDirection(dir);
        Vector3 l = transform.TransformPoint(sensorPos);

        Ray ray = new Ray(l, d);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 2))
        {
            if (hit.distance < 0.7f)
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

        if (Physics.Raycast(ray, out hit, 0.2f))
        {
            return hit;
        }
        return hit;
    }

    Vector3 VectorMask(Vector3 data, Vector3 mask)
    {
        data.x *= mask.x;
        data.y *= mask.y;
        data.z *= mask.z;
        return data;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.TransformPoint(sensorPos), 0.1f);
    }
}
