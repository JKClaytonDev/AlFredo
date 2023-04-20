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
    public void KillEnemy()
    {
        if (enemyName == "Red")
        {


        }
        if (enemyName == "Blue")
        {

        }
        if (enemyName == "Green")
        {

        }
        
    }

}
