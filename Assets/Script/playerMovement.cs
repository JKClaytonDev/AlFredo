using System;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
public class playerMovement : MonoBehaviour
{
    Rigidbody mainBody;
    public float stamina;
    public bool running;
    public float speed;
    public int health = 10;
    public GameObject sword;
    public float swordWarmup;
    bool swordSwing;
    public bool hasRed;
    public bool hasGreen;
    public bool hasBlue;
    Vector3 dir;
    private void Start()
    {
        health = 10;
        mainBody = GetComponent<Rigidbody>();
    }
    public void killPlayer()
    {

    }
    public void hurtPlayer()
    {
        health--;
    }
    void Update()
    {
        if (health < 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        if (Input.GetKey(KeyCode.W))
        {
            dir.y = 1;
            dir.x = -1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            dir.x = -1;
            dir.y = -1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            dir.y = -1;
            dir.x = 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            dir.x = 1;
            dir.y = 1;
        }
        mainBody.velocity = speed * (dir.x * new Vector3(1, 0, -1) + (dir.y * new Vector3(1, 0, 1)));
        //mainBody.velocity = speed * ((Input.GetAxis("Horizontal") * new Vector3(1, 0, -1)) + (Input.GetAxis("Vertical")) * new Vector3(1, 0, 1));

        if (Vector3.Distance(mainBody.velocity, new Vector3()) > 0.1f)
        {
            
            //if (Mathf.Abs(Input.GetAxis("Horizontal")) != Mathf.Abs(Input.GetAxis("Vertical")))
                transform.LookAt(mainBody.velocity + transform.position);
            //transform.eulerAngles = new Vector3(transform.eulerAngles.x, Mathf.Round(transform.eulerAngles.y / 45) * 45, transform.eulerAngles.z);
        }
        
        if (stamina > 90 && !running)
        {
            if (Input.GetKey(KeyCode.LeftShift))
                running = true;
        }
        if (running)
        {
            speed = 3;
            stamina -= Time.deltaTime*10;
        }
        else
        {
            stamina += Time.deltaTime*50;
            if (stamina < 25)
            {
                speed = 0.5f;
            }
            else
                speed = 1;
        }
        if (stamina < 5 || !Input.GetKey(KeyCode.LeftShift))
            running = false;
        if (stamina > 100)
            stamina = 100;
        FindObjectOfType<canvasManager>().stamina.text = "Stamina: " + Mathf.RoundToInt(stamina);
        FindObjectOfType<canvasManager>().health.text = "Health: " + Mathf.RoundToInt(health);
    }
}
