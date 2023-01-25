using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HoleIndicator : MonoBehaviour
{
    public float pulsatingSpeed = 5.7f;

    public Image im;
    Color colorRef;

    IEnumerator pulsatingCoroutineReference;

    public TMP_Text holeNumberText;

    void Start()
    {
        colorRef = im.color;
        pulsatingCoroutineReference = Pulsate();
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

    public void SetHoleNumber(int holeNumber)
    {
        holeNumberText.text = holeNumber.ToString();
    }
}
