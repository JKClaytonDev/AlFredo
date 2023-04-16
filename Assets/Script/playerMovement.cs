using System;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
public class playerMovement : MonoBehaviour
{
    public int playerIndex;
    public GameObject deadDog;
    Rigidbody mainBody;
    public SpriteRenderer directionSprite;
    public float frameTime = 0.3f;
    public int health = 10;
    float lineTime;
    public Sprite[] chefSprites;
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
    public float moveTime;
    public Vector3 targetPos;
    public Vector3 realtimeTargetPos;
    Vector3 direction;
    public bool keyPressed;
    int directionSpriteIndex;
    void Update()
    {
        GetComponent<LineRenderer>().enabled = Time.realtimeSinceStartup < lineTime;
        transform.position = Vector3.MoveTowards(transform.position, realtimeTargetPos, Time.deltaTime / frameTime);
        if ((Input.GetKeyDown(KeyCode.W) && playerIndex == 0) || (Input.GetKeyDown(KeyCode.UpArrow) && playerIndex == 1))
        {
            direction = (Vector3.forward);
            directionSpriteIndex = 15;
        }
        if ((Input.GetKeyDown(KeyCode.A) && playerIndex == 0) || (Input.GetKeyDown(KeyCode.LeftArrow) && playerIndex == 1))
        {
            direction = (Vector3.left);
            directionSpriteIndex = 5;
        }
        if ((Input.GetKeyDown(KeyCode.S) && playerIndex == 0) || (Input.GetKeyDown(KeyCode.DownArrow) && playerIndex == 1))
        {
            direction = (Vector3.back);
            directionSpriteIndex = 10;
            
        }
        if ((Input.GetKeyDown(KeyCode.D) && playerIndex == 0) || (Input.GetKeyDown(KeyCode.RightArrow) && playerIndex == 1))
        {
            direction = (Vector3.right);
            directionSpriteIndex = 0;
        }
        float spriteFrameTime = Time.realtimeSinceStartup * 10;
        directionSprite.sprite = chefSprites[directionSpriteIndex+(int)(spriteFrameTime - (MathF.Floor(spriteFrameTime / 4)*4))];

        RaycastHit target;
        if (keyPressed)
        {
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
