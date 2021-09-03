using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepController : MonoBehaviour
{
    public float speed;

    public float rotationSpeed;

    public float gravity;

    public Vector3 sensorPos;

    float currentGravity;

    bool grounded;
    Vector3 movementVector;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Gravity();
        SimpleMove();
        FinalMovement();
    }

    public void Move()
    {

    }

    void Gravity()
    {
        Vector3 boxPos = transform.position;
        Vector3 boxSize = Vector3.one * 0.5f;
        grounded = Physics.CheckBox(boxPos, boxSize * 0.5f);
        if (grounded)
        {
            currentGravity = 0;
        }
        else
        {
            currentGravity = -gravity;
        }
        if (grounded)
        {
            Ray ray = new Ray(transform.position, -transform.up);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.distance < 0.5f)
                {
                    Vector3 temp = VectorMask(transform.position, transform.parent.forward) + VectorMask(hit.point, transform.parent.up) + VectorMask(transform.position, transform.parent.right);
                }
            }
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
        
        Vector3 rotation = VectorMask(movement, transform.parent.right) + VectorMask(movement, transform.parent.up) + VectorMask(movement, transform.parent.forward);

        if (rotation != Vector3.zero)
        {
            Quaternion lookRotaion = Quaternion.LookRotation(rotation, transform.parent.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotaion, rotationSpeed * Time.deltaTime);
        }
    }

    void FinalMovement()
    {
        transform.position = transform.TransformDirection(movementVector);
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
}
