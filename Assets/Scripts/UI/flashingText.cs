using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashingText : MonoBehaviour
{
    public GameObject objectToFlash;
    public float flashInterval = 0.5f;

    private void Start()
    {
        InvokeRepeating("Flash", 0, flashInterval);
    }

    private void Flash()
    {
        objectToFlash.SetActive(!objectToFlash.activeSelf);
    }
}
