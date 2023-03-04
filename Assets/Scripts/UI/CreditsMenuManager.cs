using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class CreditsMenuManager : MonoBehaviour
{
    public Keyboard keyboard;

    [SerializeField] private Button anyKeyButton;

    // Start is called before the first frame update
    void Start()
    {
        keyboard = Keyboard.current;

        anyKeyButton.onClick.AddListener(() => GlobalUIManager.instance.SetLastMenu());

        GlobalUIManager.instance.es.SetSelectedGameObject(anyKeyButton.gameObject);
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
