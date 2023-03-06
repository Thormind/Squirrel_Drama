using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashingText : MonoBehaviour
{
    public GameObject objectToFlash;
    public float flashInterval = 0.5f;

    private void Start()
    {
        StartFlash();
    }

    private void Flash()
    {
        objectToFlash.SetActive(!objectToFlash.activeSelf);
    }

    public void StartFlash()
    {
        InvokeRepeating("Flash", 0, flashInterval);
    }

    public void StopFlash()
    {
        CancelInvoke("Flash");
    }
}
