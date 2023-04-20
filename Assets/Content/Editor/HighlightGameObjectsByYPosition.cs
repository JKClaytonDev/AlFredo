using UnityEngine;
using UnityEditor;

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

        // Create a list to hold the game objects to select
        var objectsToSelect = new System.Collections.Generic.List<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.transform.position.y == yPositionToHighlight && obj.transform.lossyScale == transform.lossyScale && obj.transform.childCount == 0)
            {
                // Add the object to the list of objects to select
                objectsToSelect.Add(obj);
            }
        }

        // Select the game objects in the editor
        Selection.objects = objectsToSelect.ToArray();
    }
}