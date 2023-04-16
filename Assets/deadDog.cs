using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deadDog : MonoBehaviour
{
    Vector3 startAngles;
    Vector3 startScale;
    public GameObject spawnSausage;
    // Start is called before the first frame update
    void Start()
    {
        startAngles = transform.eulerAngles;
        startScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = startAngles + 5 * new Vector3(0, Mathf.Sin(Time.realtimeSinceStartup*5), 0);
        foreach (playerMovement p in FindObjectsOfType<playerMovement>())
        {
            if (Vector3.Distance(transform.position, p.transform.position) < 0.5f)
            {
                Destroy(gameObject);
                GameObject sausage = Instantiate(spawnSausage);
                sausage.transform.position = transform.position;
                sausage.transform.parent = null;
                sausage.GetComponent<pellet>().moving = true;
            }
        }
        transform.localScale = startScale * ((Mathf.Sin(Time.realtimeSinceStartup*2) + 3f)*0.25f);
    }
}
