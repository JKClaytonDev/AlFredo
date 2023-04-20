using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pellet : MonoBehaviour
{
    public bool red;
    public bool blue;
    public bool sausage;
    public bool moving;
    public int playerIndex = -1;
    public AudioClip pickUpSound;
    public AudioClip sausagePickUpSound;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(Mathf.Round(transform.position.x), transform.position.y, Mathf.Round(transform.position.z));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<playerMovement>())
        {
            playerIndex = other.gameObject.GetComponent<playerMovement>().playerIndex;
            if (sausage)
                Debug.Log("SAUSAGE INDEX IS " + playerIndex);
            Destroy(GetComponent<SphereCollider>());
            moving = true;
            if (sausage)
                GetComponent<AudioSource>().PlayOneShot(sausagePickUpSound);
            else
                GetComponent<AudioSource>().PlayOneShot(pickUpSound);
        }
            
    }
    private void Update()
    {
        if (moving)
        {
            transform.position = Vector3.MoveTowards(transform.position, FindObjectOfType<pelletParticles>().transform.position + Vector3.up * 2, 10 * Time.deltaTime);
            if (Vector3.Distance(transform.position, FindObjectOfType<pelletParticles>().transform.position+Vector3.up*2) < 0.02f)
            {
                if (sausage)
                    FindObjectOfType<playerStatusManager>().PlayAnimation("GetItem", playerIndex);
                else
                    FindObjectOfType<playerStatusManager>().PlayAnimation("GetSausage", playerIndex);
                Destroy(gameObject);
                if (playerIndex == 0)
                {
                    if (red)
                        FindObjectOfType<pelletParticles>().RedParticles1++;
                    if (blue)
                        FindObjectOfType<pelletParticles>().BlueParticles1++;
                    if (sausage)
                        FindObjectOfType<pelletParticles>().sausageParticles1++;
                    
                }
                if (playerIndex == 1)
                {
                    if (red)
                        FindObjectOfType<pelletParticles>().RedParticles2++;
                    if (blue)
                        FindObjectOfType<pelletParticles>().BlueParticles2++;
                    if (sausage)
                        FindObjectOfType<pelletParticles>().sausageParticles2++;
                }

                if (playerIndex == 0)
                {
                    if (red)
                        FindObjectOfType<pelletParticles>().score1++;
                    if (blue)
                        FindObjectOfType<pelletParticles>().score1++;
                    if (sausage)
                        FindObjectOfType<pelletParticles>().score1 *= 2;

                }
                if (playerIndex == 1)
                {
                    if (red)
                        FindObjectOfType<pelletParticles>().score2++;
                    if (blue)
                        FindObjectOfType<pelletParticles>().score2++;
                    if (sausage)
                        FindObjectOfType<pelletParticles>().score2 *= 2;
                }
            }
        }
    }
}
