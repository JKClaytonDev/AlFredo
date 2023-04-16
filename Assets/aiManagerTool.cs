using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aiManagerTool : MonoBehaviour
{
    public void MoveAll()
    {
        foreach (AIScript i in FindObjectsOfType<AIScript>())
            i.updateAI();
        Vector3[] positions = new Vector3[FindObjectsOfType<AIScript>().Length];
        for (int i = 0; i<positions.Length; i++)
        {
            if (checkArrayContains(positions, FindObjectsOfType<AIScript>()[i].transform.position))
                FindObjectsOfType<AIScript>()[i].updateAI();
            positions[i] = FindObjectsOfType<AIScript>()[i].transform.position;
        }
    }
    private void Start()
    {
        frameTime = FindFirstObjectByType<playerMovement>().frameTime;
    }
    float frameTime;
    float moveTime;
    private void Update()
    {
        if (Time.realtimeSinceStartup > moveTime)
        {
            MoveAll();
            foreach (playerMovement p in FindObjectsOfType<playerMovement>())
            {
                p.keyPressed = true;
            }
            moveTime = Time.realtimeSinceStartup + frameTime;
        }
        }
    public bool checkArrayContains(Vector3[] inV3, Vector3 check)
    {
        foreach (Vector3 v in inV3){
            if (v == check)
                return true;
        }
        return false;
    }
}
