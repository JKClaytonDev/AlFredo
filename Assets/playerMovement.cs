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
        mainBody.velocity = speed * ((Input.GetAxis("Horizontal") * new Vector3(1, 0, -1)) + (Input.GetAxis("Vertical")) * new Vector3(1, 0, 1));
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
