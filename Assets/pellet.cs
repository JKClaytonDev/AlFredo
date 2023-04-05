using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pellet : MonoBehaviour
{
    public bool red;
    public bool blue;
    public bool sausage;
    public bool moving;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(Mathf.Round(transform.position.x), transform.position.y, Mathf.Round(transform.position.z));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<playerMovement>())
        {
            Destroy(GetComponent<SphereCollider>());
            transform.localScale *= 0.8f;
            moving = true;
        }
            
    }
    private void Update()
    {
        if (moving)
        {
            transform.position = Vector3.MoveTowards(transform.position, FindObjectOfType<pelletParticles>().transform.position + Vector3.up * 2, 10 * Time.deltaTime);
            if (Vector3.Distance(transform.position, FindObjectOfType<pelletParticles>().transform.position+Vector3.up*2) < 0.1f)
            {
                Destroy(gameObject);
                if (red)
                    FindObjectOfType<pelletParticles>().RedParticles++;
                if (blue)
                    FindObjectOfType<pelletParticles>().BlueParticles++;
                if (sausage)
                    FindObjectOfType<pelletParticles>().sausageParticles++;
            }
        }
    }
}
