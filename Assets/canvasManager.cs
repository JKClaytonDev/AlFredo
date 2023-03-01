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
    public int redCount = 20;
    public int blueCount = 20;
    public int greenCount = 20;
    public float stepCount = 300;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        counter.text = "Time Left: " + Mathf.RoundToInt(stepCount);
        stepCount -= Time.deltaTime;
        red.text = ""+redCount;
        blue.text = "" + blueCount;
        green.text = "" + greenCount;
    }
}
