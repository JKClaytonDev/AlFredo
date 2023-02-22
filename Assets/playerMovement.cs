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
    void Update()
    {
        mainBody.velocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"))*10;
    }
}
