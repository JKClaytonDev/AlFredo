using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyType : MonoBehaviour
{
    public string enemyName;
    public void setColor()
    {
        Color c = Color.white;
        if (enemyName == "Red")
        {
            c = Color.red;
        }
        if (enemyName == "Blue")
        {
            c = Color.blue;
        }
        if (enemyName == "Green")
        {
            c = Color.green;
        }
        GetComponent<Renderer>().material.color = c;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("Player"))
        {
            if (enemyName == "Red")
            {
                FindObjectOfType<canvasManager>().redCount--;
            }
            if (enemyName == "Blue")
            {
                FindObjectOfType<canvasManager>().blueCount--;
            }
            if (enemyName == "Green")
            {
                FindObjectOfType<canvasManager>().greenCount--;
            }
            Destroy(gameObject);
        }
    }
}
