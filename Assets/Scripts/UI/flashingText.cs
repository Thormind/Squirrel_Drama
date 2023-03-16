using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class flashingText : MonoBehaviour
{
    public GameObject objectToFlash;
    public bool flashing;
    public float flashInterval = 0.5f;

    private IEnumerator Flash()
    {
        objectToFlash.SetActive(true);

        while (flashing)
        {
            objectToFlash.SetActive(!objectToFlash.activeSelf);

            float t = 0;

            while (t < 1f)
            {

                t += Time.deltaTime / flashInterval;
                yield return null;
            }

            yield return new WaitForEndOfFrame();
        }

        objectToFlash.SetActive(false);
    }

    public void StartFlashing(GameObject objToFlash = null)
    {
        if (objToFlash != null)
        {
            objectToFlash = objToFlash;
        }

        flashing = true;
        StartCoroutine(Flash());
    }

    public void StopFlashing(GameObject objToFlash = null)
    {
        if (objToFlash != null)
        {
            objectToFlash = objToFlash;
        }

        flashing = false;
        StopCoroutine(Flash());
    }
}
