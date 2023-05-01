using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class playerStatusManager : MonoBehaviour
{
    public Image p1Sprite;
    public Image p2Sprite;
    public Sprite p1WinSprite;
    public Sprite p2WinSprite;
    public Sprite p1LoseSprite;
    public Sprite p2LoseSprite;
    public Sprite p1TieSprite;
    public Sprite p2TieSprite;
    public Sprite p1DeadSprite;
    public Sprite p2DeadSprite;
    public pelletParticles p;
    public playerMovement p1;
    public playerMovement p2;
    public Animator playerAnim;
    public Animator player2Anim;
    // Start is called before the first frame update

    public void PlayAnimation(string animName, int index)
    {
        if (index == 0)
            playerAnim.Play(animName);
        else if (index == 1)
            player2Anim.Play(animName);
    }
    int winnerIndex;
    int lastWinnerIndex;
    // Update is called once per frame
    void Update()
    {
        Color pepperColor = Color.white;
        float subVar = Mathf.Abs(Mathf.Sin(Time.realtimeSinceStartup * 5))/2;
        pepperColor.g -= subVar+0.3f;
        pepperColor.b -= subVar + 0.3f;
        pepperColor.a = 1;
        if (Time.realtimeSinceStartup < p1.pepperTime)
            p1Sprite.color = (Color.white + pepperColor) * 0.5f;
        else
            p1Sprite.color = Color.white;
        if (Time.realtimeSinceStartup < p2.pepperTime)
            p2Sprite.color = (Color.white + pepperColor) * 0.5f;
        else
            p2Sprite.color = Color.white;

        pepperColor = Color.white;
        subVar = Mathf.Abs(Mathf.Sin(Time.realtimeSinceStartup * 5)) / 2;
        pepperColor.g -= subVar + 0.3f;
        pepperColor.r -= subVar + 0.3f;
        pepperColor.a = 1;
        if (p1.onion)
            p1Sprite.color *= (Color.white + pepperColor) * 0.5f;
        else
            p1Sprite.color *= Color.white;
        if (p2.onion)
            p2Sprite.color *= (Color.white + pepperColor) * 0.5f;
        else
            p2Sprite.color *= Color.white;

        if (p.score2+10 < p.score1)
        {
            winnerIndex = 1;
            p1Sprite.sprite = p1WinSprite;
            p2Sprite.sprite = p2LoseSprite;
        }
        if (p.score1+10 < p.score2)
        {
            winnerIndex = 2;
            p1Sprite.sprite = p1LoseSprite;
            p2Sprite.sprite = p2WinSprite;
        }
        if (p.score1 == p.score2)
        {
            winnerIndex = -1;
            p1Sprite.sprite = p1TieSprite;
            p2Sprite.sprite = p2TieSprite;
        }
        if (p1.dead)
            p1Sprite.sprite = p1DeadSprite;
        if (p2.dead)
            p2Sprite.sprite = p2DeadSprite;
        if (lastWinnerIndex != winnerIndex)
        {
            lastWinnerIndex = winnerIndex;
            if (p.score1 > p.score2)
            {
                PlayAnimation("RankDown", 0);
                PlayAnimation("RankUp", 1);
            }
            if (p.score1 < p.score2)
            {
                PlayAnimation("RankUp", 0);
                PlayAnimation("RankDown", 1);
            }
        }
    }
}
