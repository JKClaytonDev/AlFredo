using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float zoom = 30;
    bool frame1;
    private void Start()
    {
        GetComponent<Camera>().orthographicSize = 30;
    }
    Vector3 orth = new Vector3();
    private void Update()
    {
        transform.position -= orth;
        Vector3 pos1 = FindObjectOfType<playerMovement>().transform.position;
        pos1.x = Mathf.Round(pos1.x / 10) * 10;
        pos1.z = Mathf.Round(pos1.z / 10) * 10;
        
        Vector3 pos2 = FindObjectOfType<playerMovement>().transform.position;
        Vector3 pos = (pos1/2 + pos2) / 1.5f;
        //pos += (new Vector3(3, 0, -3) * (Input.mousePosition.x - Screen.width / 2) / Screen.width) + (new Vector3(3, 0, 3) * (Input.mousePosition.y - Screen.height / 2) / Screen.height);
        pos = Vector3.MoveTowards(pos1, pos, 1);
        pos = Vector3.MoveTowards(pos, pos2, 1);
        pos += new Vector3(-2f, 3.5f, -3.5f);
        
        
        transform.position = Vector3.MoveTowards(transform.position, pos, 100*Time.deltaTime);
        orth = (Vector3.up-Vector3.right) * GetComponent<Camera>().orthographicSize / 2;
        transform.position += orth;
        GetComponent<Camera>().orthographicSize = Mathf.Max(3, Mathf.Round(Vector3.Distance(transform.position, pos)-8))+zoom;
        zoom += Input.mouseScrollDelta.y;
        if (!frame1)
        {
            Time.timeScale = 0;
            frame1 = true;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            zoom = 5;
            Time.timeScale = 1;
        }
    }

}
