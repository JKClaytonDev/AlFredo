using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orbItem : MonoBehaviour
{
    public bool giveRed;
    public bool giveGreen;
    public bool giveBlue;
    void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.name.Contains("Player"))
            return;
        if (giveRed)
            FindObjectOfType<playerMovement>().hasRed = true;
        if (giveGreen)
            FindObjectOfType<playerMovement>().hasGreen = true;
        if (giveBlue)
            FindObjectOfType<playerMovement>().hasBlue = true;
        Destroy(gameObject);
    }
}
