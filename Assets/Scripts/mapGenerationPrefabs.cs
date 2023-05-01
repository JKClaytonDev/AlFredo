using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapGenerationPrefabs : MonoBehaviour
{
    public GameObject setupMapPrefabs;
    public string path = "";
    bool checkPathContains(Vector2 pos)
    {
        return path.Contains(getStringPosition(new Vector3(pos.x, 0, pos.y)));
    }
    bool checkPathContainsAround(Vector2 pos) {
        bool contains = false;
        for (int x2 = -1; x2<1; x2++)
        {
            for (int y2 = -1; y2 < 1; y2++)
            {
                if (checkPathContains(new Vector2(pos.x + x2, pos.y + y2)))
                    contains = true;
            }
        }
        return contains;
    }
    // Start is called before the first frame update
    void Start()
    {
        int scale = 50;
        for (int f = 0; f < Random.Range(2, 5); f++)
        {
            Vector2 trail = new Vector2();
            Vector2 dir = new Vector2();
            for (int i = 0; i < Random.Range(300, 500); i++)
            {
                float randomDir = ((float)Random.Range(0, 10)) / 10;
                Debug.Log("RANDOMDIR DECLARED AS " + randomDir);
                Vector3 saveTrail = trail;
                for (int y = 0; y < Random.Range(1, 3); y++)
                {
                    if (randomDir < 0.25f)
                    dir = new Vector2(1, 0);
                else if (randomDir < 0.5f)
                    dir = new Vector2(-1, 0);
                else if (randomDir < 0.75f)
                    dir = new Vector2(0, 1);
                else
                    dir = new Vector2(0, -1);
                Debug.Log("DIR IS " + dir.x + " " + dir.y);
                
                    trail = saveTrail;
                    for (int g = 0; g < Random.Range(1, 5); g++)
                    {
                        string final = getStringPosition(new Vector3(trail.x, 0, trail.y));
                        path = path + final;
                        if (!checkPathContainsAround(trail + dir * 3))
                        {
                            trail.x = Mathf.Min(Mathf.Max(trail.x + dir.x, -scale / 2 + 3), scale / 2 - 3);
                            trail.y = Mathf.Min(Mathf.Max(trail.y + dir.y, -scale / 2 + 3), scale / 2 - 3);
                        }
                    }
                }
            }
        }
        for (int x = 0; x< scale; x++)
        {
            for (int y = 0; y < scale; y++)
            {
                int xPos = (x- (scale/2)) * 1;
                int yPos = (y- (scale / 2)) * 1;
                GameObject newPrefab = Instantiate(setupMapPrefabs);
                newPrefab.transform.position = new Vector3(xPos, 0, yPos);
                Debug.Log("PATH CONTAINS: " + getStringPosition(transform.position));
                newPrefab.GetComponent<mapItemMove>().enable.SetActive(!path.Contains(getStringPosition(newPrefab.transform.position)));
            }
        }
    }
    string getStringPosition(Vector3 position)
    {
        return "{X" + position.x + "Y" + position.y + "Z" + position.z + "}";
    }
}
