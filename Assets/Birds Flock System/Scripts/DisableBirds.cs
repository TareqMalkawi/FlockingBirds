using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableBirds : MonoBehaviour
{
    private GameObject[] birds;
    void Start()
    {
        birds = GameObject.FindGameObjectsWithTag("Bird");
        foreach (var bird in birds)
        {
            bird.SetActive(false);
        }
    }
}
