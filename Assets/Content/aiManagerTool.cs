using System.Collections.Generic;
using UnityEngine;

public class aiManagerTool : MonoBehaviour
{
    public float ft, mt;
    int idx;
    private void Start() => ft = FindObjectOfType<playerMovement>().frameTime;
    private void Update()
    {
        if (Time.realtimeSinceStartup > mt)
        {
            idx++;
            MoveAll();
            foreach (playerMovement p in FindObjectsOfType<playerMovement>())
                if (idx % 2 == 0 || p.pepper) p.keyPressed = true;
            mt = Time.realtimeSinceStartup + ft / 2;
        }
    }
    void MoveAll()
    {
        var aiScripts = FindObjectsOfType<AIScript>();
        var positions = new Vector3[aiScripts.Length];
        foreach (AIScript i in aiScripts) i.updateAI();
        for (int i = 0; i < positions.Length; i++)
        {
            if (System.Array.Exists(positions, v => v == aiScripts[i].transform.position)) aiScripts[i].updateAI();
            positions[i] = aiScripts[i].transform.position;
        }
    }
}