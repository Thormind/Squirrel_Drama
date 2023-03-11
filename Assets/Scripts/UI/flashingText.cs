using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashingText : MonoBehaviour
{
    public GameObject objectToFlash;
    public float flashInterval = 0.5f;
    private bool flash;

    private void Start()
    {
        StartFlash();
    }

    private void Flash()
    {
        flash = !flash;
        objectToFlash.SetActive(flash);
    }

    public void StartFlash()
    {
        InvokeRepeating($"Flash", 0, flashInterval);
    }

    public void StopFlash()
    {
        CancelInvoke("Flash");
    }
}
