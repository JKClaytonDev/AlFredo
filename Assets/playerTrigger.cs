using System;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine;

public class playerTrigger : MonoBehaviour
{
    public UnityEngine.Object[] activateList;
    public UnityEvent[] activatedEvents;

    void OnTriggerEnter(Collider other)
    {
        foreach (var componentType in activateList)
        {
            if (other.TryGetComponent(componentType.GetType(), out Component component))
            {
                int index = Array.IndexOf(activateList, componentType);
                if (index >= 0 && index < activatedEvents.Length)
                {
                    activatedEvents[index]?.Invoke();
                }
            }
        }
    }
}
