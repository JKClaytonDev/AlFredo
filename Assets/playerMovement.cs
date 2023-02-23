using System;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
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
        mainBody.velocity = (Input.GetAxis("Horizontal") * new Vector3(1, 0, -1)) + (Input.GetAxis("Vertical") * new Vector3(1, 0, 1));
    }
}
