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
        Color pepperColor = new Color(1, -subVar - 0.3f, -subVar - 0.3f, 1);
        p1Sprite.color = (Time.realtimeSinceStartup < p1.pepperTime) ? ((Color.white + pepperColor) * 0.5f) : Color.white;
        p2Sprite.color = (Time.realtimeSinceStartup < p2.pepperTime) ? ((Color.white + pepperColor) * 0.5f) : Color.white;
        p1Sprite.color *= (p1.onion) ? ((Color.white + pepperColor) * 0.5f) : Color.white;
        p2Sprite.color *= (p2.onion) ? ((Color.white + pepperColor) * 0.5f) : Color.white;
        p1Sprite.sprite = (p1.dead) ? p1DeadSprite : ((p.score2 + 10 < p.score1) ? p1WinSprite : ((p.score1 == p.score2) ? p1TieSprite : p1Sprite.sprite));
        p2Sprite.sprite = (p2.dead) ? p2DeadSprite : ((p.score1 + 10 < p.score2) ? p2WinSprite : ((p.score1 == p.score2) ? p2TieSprite : p2Sprite.sprite));
        int currentWinnerIndex = (p.score1 > p.score2) ? 0 : ((p.score1 < p.score2) ? 1 : -1);
        if (lastWinnerIndex != currentWinnerIndex)
        {
            lastWinnerIndex = currentWinnerIndex;
            PlayAnimation((currentWinnerIndex == 0) ? "RankDown" : "RankUp", 0);
            PlayAnimation((currentWinnerIndex == 1) ? "RankDown" : "RankUp", 1);
        }
    }
}
