using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    Rigidbody mainBody;
    private void Start()
    {
        mainBody = GetComponent<Rigidbody>();
    }
    Vector3 targetPos;
    void Update()
    {

        RaycastHit hit;
        Vector3 raycastDir = new Vector3();
        if (Input.GetAxis("Horizontal") < 0)
        {
            raycastDir = Vector3.left;
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            raycastDir = Vector3.right;
        }
        if (Input.GetAxis("Vertical") < 0)
        {
            raycastDir = Vector3.back;
        }
        if (Input.GetAxis("Vertical") > 0)
        {
            raycastDir = Vector3.forward;
        }
        if (raycastDir != new Vector3())
        {
            Physics.Raycast(transform.position, raycastDir, out hit);
            targetPos = hit.point;
        }
        targetPos.y = transform.position.y;
        transform.position = (transform.position * (1 - (Time.deltaTime*10)) + targetPos * (Time.deltaTime*10));
    }
}
