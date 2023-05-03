using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class matchSprite : MonoBehaviour
{
    public SpriteRenderer parent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<SpriteRenderer>().sprite = parent.sprite;
    }
}
