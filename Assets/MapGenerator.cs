using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject[] prefabs;
    public Vector3 location = new Vector3();
    public Vector3[] addedLocations = new Vector3[1];
    public mapObjectScript[] spawnedPrefabs = new mapObjectScript[1];
    public int mapSize = 50;
    int branches = 3;
    public Vector3[] branchEnds = new Vector3[1];
    // Start is called before the first frame update
    void Start()
    {
        addedLocations = new Vector3[mapSize * branches];
        spawnedPrefabs = new mapObjectScript[mapSize * branches];
        addedLocations[0] = new Vector3(0, 0);
        branchEnds = new Vector3[branches];
        branchEnds[0] = new Vector3(0, 0);
        for (int g = 0; g < 3; g++)
        {
            Vector3[] branchLocations = new Vector3[mapSize];
            branchLocations[0] = new Vector3(0, 0);
            location = new Vector3();
            for (int i = 0; i < mapSize; i++)
            {
                GameObject placedPrefab = Instantiate(prefabs[Random.Range(0, prefabs.Length)], location, new Quaternion(0, 0, 0, 0));
                spawnedPrefabs[g * mapSize + i] = placedPrefab.GetComponent<mapObjectScript>();
                int iterations = 0;
                Vector3 lastLocation = location;
                branchLocations[i] = location;
                while ((checkVectorArrayContains(addedLocations, location) || checkVectorArrayContains(branchLocations, location)) && iterations < 5)
                {
                    location = lastLocation;
                    float randomValue = Random.Range(0f, 1f);
                    if (iterations == 0)
                    {
                        if (Random.Range(0f, 1f) > 0.2f)
                        {
                            if (g == 0)
                            {
                                if (randomValue > 0.5f)
                                    location += new Vector3(10, 0, 0);
                                else
                                    location += new Vector3(0, 0, 10);
                            }
                            if (g == 1)
                            {
                                if (randomValue > 0.5f)
                                    location += new Vector3(-10, 0, 0);
                                else
                                    location += new Vector3(0, 0, 10);
                            }
                            if (g == 2)
                            {
                                if (randomValue > 0.5f)
                                    location += new Vector3(10, 0, 0);
                                else
                                    location += new Vector3(0, 0, -10);
                            }
                        }
                        else
                        {
                            if (randomValue > 0.75f)
                                location += new Vector3(10, 0, 0);
                            else if (randomValue > 0.5f)
                                location += new Vector3(0, 0, 10);
                            else if (randomValue > 0.25f)
                                location += new Vector3(0, 0, -10);
                            else
                                location += new Vector3(-10, 0, 0);
                        }
                    }
                    else if (iterations == 1)
                    {
                        location += new Vector3(10, 0, 0);
                    }
                    else if (iterations == 2)
                    {
                        location += new Vector3(0, 0, 10);
                    }
                    else if (iterations == 3)
                    {
                        location += new Vector3(0, 0, -10);
                    }
                    else
                    {
                        location += new Vector3(-10, 0, 0);
                    }
                    iterations++;
                }
            }
            for (int i = 0; i < branchLocations.Length; i++)
            {
                addedLocations[g * mapSize + i] = branchLocations[i];
            }
        }
        Vector3 spawnPoint = new Vector3();
        for (int i = 0; i < addedLocations.Length; i++)
            spawnPoint += addedLocations[i];
        spawnPoint /= addedLocations.Length;
        Vector3 closestPoint = new Vector3();
        float minDistance = int.MaxValue;
        for (int i = 0; i < addedLocations.Length; i++)
        {
            if (Vector3.Distance(spawnPoint, addedLocations[i]) < minDistance) {
                closestPoint = addedLocations[i];
                minDistance = Vector3.Distance(spawnPoint, addedLocations[i]);
            }
        }
        for (int g = 0; g < branches; g++)
        {
            Vector3 farthestPos = new Vector3();
            float farthestDistance = 0;
            for (int i = 0; i < mapSize; i++)
            {
                if (Vector3.Distance(closestPoint, addedLocations[g * mapSize + i]) > farthestDistance)
                {
                    farthestDistance = Vector3.Distance(closestPoint, addedLocations[g * mapSize + i]);
                    farthestPos = addedLocations[g * mapSize + i];
                }
            }
            branchEnds[g] = farthestPos;
        }
        float playerDist1 = Vector3.Distance(closestPoint, branchEnds[0]);
        float playerDist2 = Vector3.Distance(closestPoint, branchEnds[1]);
        float playerDist3 = Vector3.Distance(closestPoint, branchEnds[2]);
        int index = 0;
        foreach (mapObjectScript m in spawnedPrefabs)
        {

            float red = Vector3.Distance(m.transform.position, branchEnds[0]);
            float green = Vector3.Distance(m.transform.position, branchEnds[1]);
            float blue = Vector3.Distance(m.transform.position, branchEnds[2]);
            if (red == 0 || blue == 0 || green == 0)
            {
                red = 0.01f;
                green = 0.01f;
                blue = 0.01f;
            }
            else
            {
                red = (playerDist1 / red) / 2;
                green = (playerDist2 / green) / 2;
                blue = (playerDist3 / blue) / 2;
            }
            m.spawnEnemies((red + blue + green)/3);
            m.m.material.color = new Color(red, green, blue);
            if (checkVectorArrayContains(addedLocations, m.transform.position + new Vector3(-10, 0, 0)))
                m.wallRight.SetActive(false);
            if (checkVectorArrayContains(addedLocations, m.transform.position + new Vector3(10, 0, 0)))
                m.wallLeft.SetActive(false);
            if (checkVectorArrayContains(addedLocations, m.transform.position + new Vector3(0, 0, 10)))
                m.wallUp.SetActive(false);
            if (checkVectorArrayContains(addedLocations, m.transform.position + new Vector3(0, 0, -10)))
                m.wallDown.SetActive(false);
            index++;
        }
        GameObject Player = Instantiate(playerPrefab, closestPoint + Vector3.up/2, new Quaternion(0, 0, 0, 0));

    }
    public bool checkVectorArrayContains(Vector3[] array, Vector3 pos)
    {
        bool check = false;
        for (int f = 0; f < array.Length; f++)
        {
            if (array[f] == pos)
                check = true;
        }
        return check;
    }
}
