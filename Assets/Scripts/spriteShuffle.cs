using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spriteShuffle : MonoBehaviour
{
    Vector3 startAngles;
    public float posMultiplier = 1;
    public float rotMultiplier = 1;
    public float positionPosMultiplier = 10;
    // Start is called before the first frame update
    void Start()
    {
        startAngles = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = startAngles + rotMultiplier * 5 * new Vector3(0, Mathf.Sin(transform.position.x + transform.position.z), 0);
        transform.localPosition = new Vector3(0, 0, Mathf.Sin((transform.parent.position.x + transform.parent.position.z) * positionPosMultiplier) / 15) * posMultiplier;
    }
}