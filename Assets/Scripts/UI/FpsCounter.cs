using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class FpsCounter : MonoBehaviour
{
    public TMP_Text fpsCounterText;

    private int frameIndex;
    private float[] frameDTs;
    public bool isFpsShown;

    private void Start()
    {
        fpsCounterText = gameObject.GetComponent<TMP_Text>();
        frameIndex = 0;
        frameDTs = new float[60];
        isFpsShown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFpsShown)
        {
            frameDTs[frameIndex] = 1f / Time.deltaTime;
            Debug.Log(frameDTs[frameIndex].ToString());
            frameIndex = (frameIndex + 1) % frameDTs.Length;

            int fps = CalculFps();

            fpsCounterText.SetText("FPS " + CalculFps().ToString());
        }
    }

    private int CalculFps()
    {
        float fps = 0f;

        foreach (float dt in frameDTs)
        {
            fps += dt;
        }

        return Mathf.RoundToInt(fps / frameDTs.Length);
    }
}
