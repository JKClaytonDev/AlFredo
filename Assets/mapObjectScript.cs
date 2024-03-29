using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapObjectScript : MonoBehaviour
{
    public GameObject[] enemies;
    public float enemyProbability;
    public Renderer m;
    public GameObject wallUp;
    public GameObject wallDown;
    public GameObject wallLeft;
    public GameObject wallRight;
    public void spawnEnemies(float probability)
    {
        if (transform.position == new Vector3())
            probability = 0;
        enemyProbability = probability;
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].SetActive(Random.Range(0f, 1f) < probability);
            string enemyType = "";
            float enemyChoice = Random.Range(0, 3);
            if (enemyChoice < 1)
                enemyType = "Red";
            else if (enemyChoice < 2)
                enemyType = "Blue";
            else if (enemyChoice < 3)
                enemyType = "Green";
            enemies[i].GetComponent<enemyType>().enemyName = enemyType;
            enemies[i].GetComponent<enemyType>().setColor();
        }
    }
    public void MoveUp()
    {
        MoveCamera(new Vector3(0, 0, 10));
    }
    public void MoveDown()
    {
        MoveCamera(new Vector3(0, 0, -10));
    }
    public void MoveLeft()
    {
        MoveCamera(new Vector3(10, 0, 0));
    }
    public void MoveRight()
    {
        MoveCamera(new Vector3(-10, 0, 0));
    }
    public void MoveCamera(Vector3 direction)
    {

    }
}
