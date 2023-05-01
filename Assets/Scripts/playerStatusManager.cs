using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class playerStatusManager : MonoBehaviour
{
    public pelletParticles p;
    public playerMovement p1, p2;
    public Image p1Sprite, p2Sprite;
    public Animator playerAnim, player2Anim;
    public Sprite p1WinSprite, p2WinSprite, p1LoseSprite, p2LoseSprite, p1TieSprite, p2TieSprite, p1DeadSprite, p2DeadSprite;
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
        float subVar = Mathf.Abs(Mathf.Sin(Time.realtimeSinceStartup * 5)) / 2;
        Color pepperColor = new Color(1, 0.7f - subVar, 0.7f - subVar);
        

        if (Time.realtimeSinceStartup < p1.pepperTime)
            p1Sprite.color = (Color.white + pepperColor) * 0.5f;
        else
            p1Sprite.color = Color.white;
        if (Time.realtimeSinceStartup < p2.pepperTime)
            p2Sprite.color = (Color.white + pepperColor) * 0.5f;
        else
            p2Sprite.color = Color.white;

        
        pepperColor = new Color(1, 1, pepperColor.r);
        p1Sprite.color *= (p1.onion) ? ((Color.white + pepperColor) * 0.5f) : Color.white;
        p2Sprite.color *= (p2.onion) ? ((Color.white + pepperColor) * 0.5f) : Color.white;

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
