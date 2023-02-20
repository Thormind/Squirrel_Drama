using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PrototypePulsatingLight : MonoBehaviour
{
    public float pulsatingSpeed = 5.7f;

    public Image im;
    Color colorRef;

    IEnumerator pulsatingCoroutineReference;


    // Start is called before the first frame update
    void Start()
    {
        colorRef = im.color;
        pulsatingCoroutineReference = Pulsate();
        StartPulsating();
    }

    IEnumerator Pulsate()
    {
        while (true)
        {
            colorRef.a = (Mathf.Cos(Time.time * pulsatingSpeed) * 0.5f + 0.5f) * 0.8f;
            im.color = colorRef;
            yield return new WaitForEndOfFrame();
        }
    }

    public void StartPulsating()
    {
        StartCoroutine(pulsatingCoroutineReference);
    }

    public void EndPulsating()
    {
        StopCoroutine(pulsatingCoroutineReference);
        colorRef.a = 0;
        im.color = colorRef;
    }
}
