using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HoleIndicator : MonoBehaviour
{
    public float pulsatingSpeed = 5.7f;
    public float flashInterval = 0.5f;

    public Image im;
    Color colorRef;

    IEnumerator pulsatingCoroutineReference;
    IEnumerator flashingCoroutineReference;

    public TMP_Text holeNumberText;

    void Start()
    {
        colorRef = im.color;
        pulsatingCoroutineReference = Pulsate();
        flashingCoroutineReference = Flash();
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

    IEnumerator Flash()
    {
        while (true)
        {
            im.gameObject.SetActive(!im.gameObject.activeSelf);

            float t = 0;

            while (t < 1f)
            {

                t += Time.deltaTime / flashInterval;
                yield return null;
            }

            yield return new WaitForEndOfFrame();


        }
    }

    public void StartFlashing()
    {
        colorRef.a = 255;
        im.color = colorRef;
        StartCoroutine(flashingCoroutineReference);
    }

    public void StopFlashing()
    {
        StopCoroutine(flashingCoroutineReference);
        colorRef.a = 0;
        im.color = colorRef;
    }
}
