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
    public GameObject[] walls;
    public Transform wallParent;
   
    public void spawnEnemies(float probability)
    {
        walls = new GameObject[wallParent.childCount];
        for (int i = 0; i < walls.Length; i++)
            walls[i] = wallParent.GetChild(i).gameObject;
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
        int activated = 0;
        int startIndex = Random.Range(0, 1);
        while (activated < wallParent.childCount*0.25f)
        {
            for (int i = startIndex; i < walls.Length; i+=3)
            {
                if (!walls[i].activeInHierarchy)
                {
                    bool activate = Random.Range(0.1f, 1f) > 0.8f;
                    if (activate)
                    {
                        activated++;
                        walls[i].SetActive(true);
                    }
                }
            }
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
