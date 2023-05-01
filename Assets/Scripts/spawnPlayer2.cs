using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnPlayer2 : MonoBehaviour
{
    public GameObject player2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
            player2.SetActive(true);
    }
}
