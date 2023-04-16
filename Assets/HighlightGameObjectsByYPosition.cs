using UnityEngine;

[ExecuteInEditMode]
public class HighlightGameObjectsByYPosition : MonoBehaviour
{
    public bool highlightObjects;
    public float yPositionToHighlight;

    private void Update()
    {
        if (highlightObjects)
        {
            HighlightObjects();
        }
    }

    private void HighlightObjects()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.transform.position.y == yPositionToHighlight)
            {
                // Change the color or material of the object to highlight it
                // For example:
                obj.GetComponent<Renderer>().material.color = Color.yellow;
            }
        }
    }
}