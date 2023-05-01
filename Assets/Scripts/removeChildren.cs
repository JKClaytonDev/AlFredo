using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class removeChildren : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.DetachChildren();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
