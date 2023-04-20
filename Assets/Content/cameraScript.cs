using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour
{
    playerMovement p;
    // Start is called before the first frame update
    void Start()
    {
        p = FindObjectOfType<playerMovement>();
        Vector3 pos = p.transform.position;
        pos.x = Mathf.Round((pos.x + 6) / 13) * 13 - 6;
        pos.z = Mathf.Round((pos.z + 6) / 13) * 13 - 6;
        pos.y = transform.position.y;
        Vector3 pos2 = new Vector3(13, transform.position.y, 0);
        pos = (pos * 2 + pos2) * 0.33f;
        pos.y = 8;
        transform.position = pos;
        lastSetPos = transform.position;
    }
    Vector3 lastSetPos;
    // Update is called once per frame
    void Update()
    {
        transform.position = lastSetPos;
        Vector3 pos = p.transform.position;
        pos.x = Mathf.Round((pos.x+6) / 13)*13-6;
        pos.z = Mathf.Round((pos.z + 6) / 13)*13-6;
        pos.y = transform.position.y;
        Vector3 pos2 = new Vector3(13, transform.position.y, 0);
        pos = (pos*2 + pos2) * 0.33f;
        pos.y = 8;
        transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime*15);
        lastSetPos = transform.position;
        transform.position += (p.transform.position - transform.position) * 0.2f;
    }
}
