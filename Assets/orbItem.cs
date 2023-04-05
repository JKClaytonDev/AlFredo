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

        Destroy(gameObject);
    }
}
