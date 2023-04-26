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
            int itemIndex = 0;
            if (other.gameObject.GetComponent<playerMovement>().playerIndex == 0)
                itemIndex = FindObjectOfType<phaseManager>(false).p1ItemIndex;
            if (other.gameObject.GetComponent<playerMovement>().playerIndex == 1)
                itemIndex = FindObjectOfType<phaseManager>(false).p2ItemIndex;
            if (itemIndex == 1)
                other.gameObject.GetComponent<playerMovement>().onionTime = Time.realtimeSinceStartup + 10;
            else
                other.gameObject.GetComponent<playerMovement>().pepperTime = Time.realtimeSinceStartup + 10;
            Destroy(gameObject);
        }
    }
}
