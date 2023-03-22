using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class Worm_animation : MonoBehaviour
{
    Animator animator;

    public TMP_Text animationSpeedTxt;
    public Slider animationSpeedSlider;
    public Button startButton;
    public Toggle getBackToggle;
    public GameObject wormModel;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animationSpeedSlider.onValueChanged.AddListener(delegate { HandleSpeedAnimation(animationSpeedSlider.value); });
        animationSpeedSlider.value = 1;
        HandleSpeedAnimation(animationSpeedSlider.value);

        startButton.onClick.AddListener(delegate { StartAnimation(); });
        getBackToggle.onValueChanged.AddListener(delegate { ToggleIsGettingBack(getBackToggle.isOn); });

    }

    void Update()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("inactive"))
        {
            wormModel.SetActive(false);
            Debug.Log("inactive");
        }
    }


    public void StartAnimation()
    {
        wormModel.SetActive(true);
        animator.Play("exitHole");
    }

    public void ToggleIsGettingBack(bool newState)
    {
        animator.SetBool("isGettingBack", newState);
    }

    public void HandleSpeedAnimation(float newSpeed)
    {
        animator.speed = newSpeed;
        animationSpeedTxt.text = newSpeed.ToString();
    }
}
