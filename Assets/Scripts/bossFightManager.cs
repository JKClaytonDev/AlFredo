using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class bossFightManager : MonoBehaviour
{
    public KeyCode key;
    public Text keycodeText;
    public Text healthText;
    float width = 100;
    int health = 10;
    public Image shrinkImage;
    // Start is called before the first frame update
    void Start()
    {
        shrinkImage.GetComponent<RectTransform>().localScale = new Vector3(Random.Range(0.5f, 1f), 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = 0;
        if (health <= 0)
        {
            Time.timeScale = 1;
            Destroy(gameObject);
        }
        keycodeText.text = "Press " + key.ToString() + " to stop";
        healthText.text = health + "/10";
        shrinkImage.transform.localScale = new Vector3(shrinkImage.transform.localScale.x - Time.unscaledDeltaTime/5, 1, 1);
        if (Input.GetKeyDown(key) || shrinkImage.transform.localScale.x < 0.1f)
        {
            if (shrinkImage.transform.localScale.x < 0.3f && shrinkImage.transform.localScale.x > 0.1f)
            {
                health--;
                shrinkImage.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                shrinkImage.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
}
