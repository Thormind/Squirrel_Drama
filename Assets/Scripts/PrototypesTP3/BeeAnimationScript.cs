using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BeeAnimationScript : MonoBehaviour
{

    Animator animator;

    public TMP_Text animationSpeedTxt;
    public Slider animationSpeedSlider;

    private Vector3 position;

    public float ySpeed = 0.75f;

    public float yMin = -0.1f;
    public float yMax = 0.1f;

    public float yDirection = 1f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        animationSpeedSlider.onValueChanged.AddListener(delegate { HandleSpeedAnimation(animationSpeedSlider.value); });
        animationSpeedSlider.value = 100;
        HandleSpeedAnimation(animationSpeedSlider.value);

        yMin += transform.position.y;
        yMax += transform.position.y;
    }

    void Update()
    {
        position = transform.position;
        position += new Vector3(position.x, ySpeed * yDirection * Time.deltaTime, 0f);

        position.y = Mathf.Clamp(position.y, yMin, yMax);

        transform.position = position;

        if (transform.position.y >= yMax || transform.position.y <= yMin)
        {
            yDirection *= -1;
        }
    }

    public void ToggleBothWings(bool newState)
    {
        animator.SetBool("isBothWings", newState);
    }

    public void ToggleLeftWing(bool newState)
    {
        animator.SetBool("isLeftWing", newState);
    }


    public void ToggleRightWing(bool newState)
    {
        animator.SetBool("isRightWing", newState);
    }

    public void HandleSpeedAnimation(float newSpeed)
    {
        animator.speed = newSpeed;
        animationSpeedTxt.text = newSpeed.ToString();
    }

}
