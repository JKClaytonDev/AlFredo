using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class canvasManager : MonoBehaviour
{
    public Text counter;
    public Text red;
    public Text blue;
    public Text green;
    public Text stamina;
    public Text health;
    public int redCount = 20;
    public int blueCount = 20;
    public int greenCount = 20;
    public float stepCount = 300;
    public GameObject boss1;
    public GameObject boss2;
    public GameObject boss3;
    public GameObject winState;
    // Start is called before the first frame update
    public void startBosses()
    {
        if (redCount <= 0 && boss1 != null)
            boss1.SetActive(true);
        if (blueCount <= 0 && boss2 != null)
            boss2.SetActive(true);
        if (greenCount <= 0 && boss3 != null)
            boss3.SetActive(true);
    }
    // Update is called once per frame
    void Update() {
    
    if (Input.GetKey(KeyCode.Escape)){
            Application.Quit();
        }
        if (boss1 == null && boss2 == null && boss3 == null)
        {
            Time.timeScale = 0;
            winState.SetActive(true);
        }
        counter.text = "Time Left: " + Mathf.RoundToInt(stepCount);
        stepCount -= Time.deltaTime;
        red.text = ""+redCount;
        blue.text = "" + blueCount;
        green.text = "" + greenCount;
        if (redCount <= 0)
            red.text = "Completed - Press Fight to Battle";
        if (blueCount <= 0)
            blue.text = "Completed - Press Fight to Battle";
        if (greenCount <= 0)
            green.text = "Completed - Press Fight to Battle";
    }
}
