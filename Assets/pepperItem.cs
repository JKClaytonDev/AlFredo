using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pepperItem : MonoBehaviour
{
    public bool onion;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<playerMovement>())
        {
            if (onion)
                other.gameObject.GetComponent<playerMovement>().onionTime = Time.realtimeSinceStartup + 10;
            else
                other.gameObject.GetComponent<playerMovement>().pepperTime = Time.realtimeSinceStartup + 10;
            Destroy(gameObject);
        }
    }
}
