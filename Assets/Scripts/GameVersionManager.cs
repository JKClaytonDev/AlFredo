using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameVersionManager : MonoBehaviour
{
    public bool Android, p1LeftTap, p2LeftTap, p1RightTap, p2RightTap, p1UpTap, p2UpTap, p1DownTap, p2DownTap, p1ShootTap, p2ShootTap;
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
        // Check if the platform is Android
        switch (Android)
        {
            // If it is, enable the mobile UI and exit the method
            case true:
                mobileUI.SetActive(true);
                return;
            // If it's not Android, disable the mobile UI and check for keyboard input
            default:
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
                break;
        }
    }

    public void setP1Input(int index)
    {
        // Reset all the boolean variables for player 1
        resetP1Bool();

        // Check the value of the index and set the corresponding boolean variable to true
        switch (index)
        {
            case 1:
                p1RightTap = true;
                break;
            case 2:
                p1LeftTap = true;
                break;
            case 3:
                p1UpTap = true;
                break;
            case 4:
                p1DownTap = true;
                break;
            case 5:
                p1ShootTap = true;
                break;
        }
    }
    public void setP2Input(int index)
    {
        // Reset all the boolean variables for player 2
        resetP2Bool();

        // Check the value of the index and set the corresponding boolean variable to true
        switch (index)
        {
            case 1:
                p2RightTap = true;
                break;
            case 2:
                p2LeftTap = true;
                break;
            case 3:
                p2UpTap = true;
                break;
            case 4:
                p2DownTap = true;
                break;
            case 5:
                p2ShootTap = true;
                break;
        }
    }
    public void resetP1Bool()
    {
        // Reset all the boolean variables for player 1
        p1LeftTap = false;
        p1RightTap = false;
        p1UpTap = false;
        p1DownTap = false;
        p1ShootTap = false;
    }

    public void resetP2Bool()
    {
        // Reset all the boolean variables for player 2
        p2LeftTap = false;
        p2RightTap = false;
        p2UpTap = false;
        p2DownTap = false;
        p2ShootTap = false;
    }
}
