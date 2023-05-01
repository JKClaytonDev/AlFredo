using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameVersionManager : MonoBehaviour
{
    public bool Android;
    public bool p1LeftTap;
    public bool p2LeftTap;
    public bool p1RightTap;
    public bool p2RightTap;
    public bool p1UpTap;
    public bool p2UpTap;
    public bool p1DownTap;
    public bool p2DownTap;
    public bool p1ShootTap;
    public bool p2ShootTap;
    public GameObject mobileUI;
    public Text p1DeadText;
    private void Start()
    {
        if (Android)
        {
            p1DeadText.text = "PRESS < AND >\nTO RETURN";
        }
    }
    private void Update()
    {
        if (Android)
        {
            mobileUI.SetActive(true);
            return;
        }
        mobileUI.SetActive(false);
        p1LeftTap = Input.GetKeyDown(KeyCode.A);
        p1RightTap = Input.GetKeyDown(KeyCode.D);
        p1UpTap = Input.GetKeyDown(KeyCode.W);
        p1DownTap = Input.GetKeyDown(KeyCode.S);
        p1ShootTap = Input.GetKeyDown(KeyCode.Q);

        p2LeftTap = Input.GetKeyDown(KeyCode.LeftArrow);
        p2RightTap = Input.GetKeyDown(KeyCode.RightArrow);
        p2UpTap = Input.GetKeyDown(KeyCode.UpArrow);
        p2DownTap = Input.GetKeyDown(KeyCode.DownArrow);
        p2ShootTap = Input.GetKeyDown(KeyCode.Period);
    }
    public void setP1Input(int index)
    {
        resetP1Bool();
        if (index == 1)
            p1RightTap = true;
        if (index == 2)
            p1LeftTap = true;
        if (index == 3)
            p1UpTap = true;
        if (index == 4)
            p1DownTap = true;
        if (index == 5)
            p1ShootTap = true;
    }
    public void setP2Input(int index)
    {
        resetP2Bool();
        if (index == 1)
            p2RightTap = true;
        if (index == 2)
            p2LeftTap = true;
        if (index == 3)
            p2UpTap = true;
        if (index == 4)
            p2DownTap = true;
        if (index == 5)
            p2ShootTap = true;
    }
    public void resetP1Bool()
    {
        p1LeftTap = false;
        p1RightTap = false;
        p1UpTap = false;
        p1DownTap = false;
        p1ShootTap = false;
    }

    public void resetP2Bool()
    {
        p2LeftTap = false;
        p2RightTap = false;
        p2UpTap = false;
        p2DownTap = false;
        p2ShootTap = false;
    }
}
