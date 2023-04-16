using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIScript : MonoBehaviour
{
   public Vector3 direction;
    public Vector3 nextMovePos;
    public SpriteRenderer foodSprite;
    public Sprite[] foodSprites;
    public Vector3 realtimeTargetPos;
    float frameTime;
    Vector3 setDir()
    {
        transform.forward = direction;
        transform.Rotate(0, 90, 0);
        Vector3 set = transform.forward;
        transform.eulerAngles = new Vector3();
        if (set == Vector3.forward) {
            foodSprite.sprite = foodSprites[0];
            return (Vector3.forward);
        }
        else if (set == Vector3.left) {
            foodSprite.sprite = foodSprites[1];
            return (Vector3.left);
        }
        else if (set == Vector3.back) {
            foodSprite.sprite = foodSprites[2];
            return (Vector3.back);
        }
        else
        {
            foodSprite.sprite = foodSprites[3];
            return (Vector3.right);
        }  
    }
    private void Start()
    {
        realtimeTargetPos = transform.position;
        frameTime = FindFirstObjectByType<playerMovement>().frameTime;
        nextMovePos = realtimeTargetPos;
        realtimeTargetPos = new Vector3(Mathf.Round((realtimeTargetPos).x), realtimeTargetPos.y, Mathf.Round((realtimeTargetPos).z));
        direction = setDir();
    }
    int turn;
    public void updateAI()
    {
        Vector3 tempMove = nextMovePos;
        realtimeTargetPos = nextMovePos;
        int counter = 0;
        RaycastHit hit;
        
        while (Physics.Raycast(realtimeTargetPos, direction, 1) && counter < 100)
        {
            counter++;
            direction = setDir();
        }
        Debug.Log("MOVING ENEMY " + counter + name);
        if (counter != 100)
            realtimeTargetPos = new Vector3(Mathf.Round((realtimeTargetPos + direction).x), realtimeTargetPos.y, Mathf.Round((realtimeTargetPos + direction).z));
        nextMovePos = realtimeTargetPos;
        realtimeTargetPos = tempMove;
    }
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, realtimeTargetPos, (Time.deltaTime / frameTime));
        foreach (playerMovement p in FindObjectsOfType<playerMovement>())
        {
            if (Vector3.Distance(transform.position, p.transform.position) < 0.5f)
                p.killPlayer();
        }
    }
}
