using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordScript : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        
        Debug.Log("HIT " + collision.transform.gameObject.name);
        if (collision.gameObject.GetComponent<enemyType>())
        {
            collision.gameObject.GetComponent<enemyType>().KillEnemy();
        }
    }
}
