using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dogManager : MonoBehaviour
{
    public GameObject dog;
    public void addDog()
    {
        if (dog.activeInHierarchy == false)
        {
            dog.SetActive(true);
        }
        else
        {
            GameObject newDog = Instantiate(dog, dog.transform.parent);
            dog = newDog;
            dog.transform.localPosition -= Vector3.right * 30;
        }
    }
}
