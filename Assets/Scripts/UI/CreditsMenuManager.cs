using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Controls;
using TMPro;

public class CreditsMenuManager : MonoBehaviour
{
    public Keyboard keyboard;

    // Start is called before the first frame update
    void Start()
    {
        keyboard = Keyboard.current;
    }

    // Update is called once per frame
    void Update()
    {
        if (keyboard.anyKey.wasPressedThisFrame)
        {
            GlobalUIManager.instance.SetLastMenu();
        }
    }
}
