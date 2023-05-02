using UnityEngine;

public class AIScript : MonoBehaviour
{
    public bool turnOtherWay;
    public Vector3 direction, nextMovePos, realtimeTargetPos;
    public SpriteRenderer foodSprite;
    public Sprite[] foodSprites;
    float ft;
    Vector3 lastPos;

    Vector3 SetDir()
    {
        transform.forward = direction;
        transform.Rotate(0, 180, 0);
        Vector3 set = transform.forward;
        if (turnOtherWay)
            set = transform.right;
        transform.eulerAngles = new Vector3();
        int idx = set == Vector3.forward ? 0 : set == Vector3.left ? 1 : set == Vector3.back ? 2 : 3;
        foodSprite.sprite = foodSprites[idx];
        return set;
    }

    private void Start()
    {
        realtimeTargetPos = transform.position;
        ft = FindObjectOfType<playerMovement>().frameTime;
        nextMovePos = realtimeTargetPos;
        realtimeTargetPos = new Vector3(Mathf.Round(realtimeTargetPos.x), realtimeTargetPos.y, Mathf.Round(realtimeTargetPos.z));
        direction = SetDir();
    }

    public void updateAI()
    {
        Vector3 tempMove = nextMovePos;
        realtimeTargetPos = nextMovePos;
        int counter = 0;
        while (Physics.Raycast(realtimeTargetPos, direction, 1) && counter < 100)
        {
            counter++;
            direction = SetDir();
        }
        if (counter != 100) realtimeTargetPos = new Vector3(Mathf.Round((realtimeTargetPos + direction).x), realtimeTargetPos.y, Mathf.Round((realtimeTargetPos + direction).z));
        nextMovePos = realtimeTargetPos;
        realtimeTargetPos = tempMove;
    }

    private void Update()
    {
        lastPos = transform.position;
        if (transform.position != lastPos) direction = SetDir();
        transform.position = Vector3.MoveTowards(transform.position, realtimeTargetPos, (Time.deltaTime / ft));
        foreach (playerMovement p in FindObjectsOfType<playerMovement>())
            if (Vector3.Distance(transform.position, p.transform.position) < 0.5f) p.killPlayer();
    }
}