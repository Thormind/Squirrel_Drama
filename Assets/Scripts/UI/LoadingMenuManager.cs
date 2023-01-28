using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingMenuManager : MonoBehaviour
{
    [SerializeField] public Slider LoadingProgressBar;

    public void FixedUpdate()
    {
        LoadingProgressBar.value = ScenesManager.instance.loadProgress;
    }
}
