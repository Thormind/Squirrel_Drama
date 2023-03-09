using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ElevatorBallMod : MonoBehaviour
{
    public ElevatorController elevatorControllerRef;

    public TMP_Text elevatorSpeedTxt;
    public Slider elevatorSpeedSlider;

    public Rigidbody2D ballRB;

    [SerializeField] private float ballMass = 1f;
    public TMP_Text ballMassTxt;
    public Slider ballMassSlider;
    [SerializeField] private float ballLinear = 0.2f;
    public TMP_Text ballLinearTxt;
    public Slider ballLinearSlider;
    [SerializeField] private float ballAngular = 0.05f;
    public TMP_Text ballAngularTxt;
    public Slider ballAngularSlider;
    [SerializeField] private float ballGravityScale = 4.5f;
    public TMP_Text ballGravityScaleTxt;
    public Slider ballGravityScaleSlider;

    public Button resetButton;


    // Start is called before the first frame update
    void Start()
    {

        // Get the current event system
        EventSystem currentEventSystem = EventSystem.current;

        // Disable gamepad input for UI navigation
        currentEventSystem.sendNavigationEvents = false;

        resetButton.onClick.AddListener(() => HandleResetButton());

        elevatorSpeedSlider.onValueChanged.AddListener(delegate { HandleElevatorSpeedInputData(elevatorSpeedSlider.value); });
        elevatorSpeedSlider.value = elevatorControllerRef.movementSpeed;

        ballMassSlider.onValueChanged.AddListener(delegate { HandleBallMassInputData(ballMassSlider.value); });
        ballLinearSlider.onValueChanged.AddListener(delegate { HandleBallLinearInputData(ballLinearSlider.value); });
        ballAngularSlider.onValueChanged.AddListener(delegate { HandleBallAngularInputData(ballAngularSlider.value); });
        ballGravityScaleSlider.onValueChanged.AddListener(delegate { HandleBallGravityScaleInputData(ballGravityScaleSlider.value); });

        ballRB.mass = ballMass;
        ballRB.drag = ballLinear;
        ballRB.angularDrag = ballAngular;
        ballRB.gravityScale = ballGravityScale;

        ballMassSlider.value = ballRB.mass;
        ballLinearSlider.value = ballRB.drag;
        ballAngularSlider.value = ballRB.angularDrag;
        ballGravityScaleSlider.value = ballRB.gravityScale;

        elevatorControllerRef.ResetStartPositionFunction();

    }

    public void HandleElevatorSpeedInputData(float speed)
    {
        elevatorControllerRef.movementSpeed = speed;
        elevatorSpeedTxt.text = speed.ToString();
    }

    public void HandleBallMassInputData(float mass)
    {
        ballRB.mass = mass;
        ballMassTxt.text = mass.ToString();
    }

    public void HandleBallLinearInputData(float linear)
    {
        ballRB.drag = linear;
        ballLinearTxt.text = linear.ToString();
    }

    public void HandleBallAngularInputData(float angular)
    {
        ballRB.angularDrag = angular;
        ballAngularTxt.text = angular.ToString();
    }

    public void HandleBallGravityScaleInputData(float gravityScale)
    {
        ballRB.gravityScale = gravityScale;
        ballGravityScaleTxt.text = gravityScale.ToString();
    }

    public void HandleResetButton()
    {
        elevatorControllerRef.ResetBottomPositionFunction();
    }

}
