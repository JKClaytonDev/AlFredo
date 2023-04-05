using System;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
public class playerMovement : MonoBehaviour
{
    public GameObject deadDog;
    Rigidbody mainBody;
    public Material[] directionSprites;
    public Renderer directionSprite;
    public float frameTime = 0.3f;
    public int health = 10;
    float lineTime;
    public void drawLine(Vector3 target)
    {
        GetComponent<LineRenderer>().SetPosition(0, transform.position);
        GetComponent<LineRenderer>().SetPosition(1, target);
        lineTime = Time.realtimeSinceStartup + 0.1f;
    }
    public void killPlayer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void Start()
    {
        direction = (Vector3.forward);
        targetPos = realtimeTargetPos;
        health = 10;
        mainBody = GetComponent<Rigidbody>();
        realtimeTargetPos = transform.position;
    }
    float moveTime;
    public Vector3 targetPos;
    public Vector3 realtimeTargetPos;
    Vector3 direction;
    bool keyPressed;
    void Update()
    {
        GetComponent<LineRenderer>().enabled = Time.realtimeSinceStartup < lineTime;
        transform.position = Vector3.MoveTowards(transform.position, realtimeTargetPos, Time.deltaTime / frameTime);
        if (Input.GetKeyDown(KeyCode.W))
        {
            direction = (Vector3.forward);
            directionSprite.material = directionSprites[0];
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            direction = (Vector3.left);
            directionSprite.material = directionSprites[1];
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            direction = (Vector3.back);
            directionSprite.material = directionSprites[2];
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            direction = (Vector3.right);
            directionSprite.material = directionSprites[3];
        }
        if (Time.realtimeSinceStartup > moveTime)
        {
            keyPressed = true;
            moveTime = Time.realtimeSinceStartup + frameTime;
        }
        RaycastHit target;
        if (keyPressed)
        {
            FindObjectOfType<aiManagerTool>().MoveAll();
            keyPressed = false;
            if (!Physics.Raycast(realtimeTargetPos, direction, out target, 1))
                realtimeTargetPos = new Vector3(Mathf.Round((realtimeTargetPos + direction).x), realtimeTargetPos.y, Mathf.Round((realtimeTargetPos + direction).z));
            else if (target.transform.gameObject.GetComponent<AIScript>())
                killPlayer();
        }
        
        {
            RaycastHit hit;
            Physics.Raycast(realtimeTargetPos, direction, out hit);
            if (hit.transform.gameObject.GetComponent<AIScript>())
            {
                if (direction == hit.transform.gameObject.GetComponent<AIScript>().direction)
                {
                    drawLine(hit.point);
                    GameObject newCheck = Instantiate(deadDog);
                    newCheck.transform.position = hit.transform.gameObject.transform.position;
                    newCheck.transform.parent = null;
                    Destroy(hit.transform.gameObject);
                }
            }
        }
    }
}
