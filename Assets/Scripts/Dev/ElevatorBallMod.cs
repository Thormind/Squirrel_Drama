using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ElevatorBallMod : MonoBehaviour
{
    public MetalBarController elevatorControllerRef;

    [SerializeField] private float elevatorSpeed = 1.75f;
    public TMP_Text elevatorSpeedTxt;
    public Slider elevatorSpeedSlider;

    /*
    public Rigidbody2D elevatorRB;
    [SerializeField] private float elevatorMass = 1f;
    public TMP_Text elevatorMassTxt;
    public Slider elevatorMassSlider;
    [SerializeField] private float elevatorLinear = 0f;
    public TMP_Text elevatorLinearTxt;
    public Slider elevatorLinearSlider;
    [SerializeField] private float elevatorAngular = 0.05f;
    public TMP_Text elevatorAngularTxt;
    public Slider elevatorAngularSlider;
    [SerializeField] private float elevatorGravityScale = 6f;
    public TMP_Text elevatorGravityScaleTxt;
    public Slider elevatorGravityScaleSlider;
    */

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


    // Start is called before the first frame update
    void Start()
    {
        /*
        elevatorMassSlider.onValueChanged.AddListener(delegate { HandleElevatorMassInputData(elevatorMassSlider.value); });
        elevatorLinearSlider.onValueChanged.AddListener(delegate { HandleElevatorLinearInputData(elevatorLinearSlider.value); });
        elevatorAngularSlider.onValueChanged.AddListener(delegate { HandleElevatorAngularInputData(elevatorAngularSlider.value); });
        elevatorGravityScaleSlider.onValueChanged.AddListener(delegate { HandleElevatorGravityScaleInputData(elevatorGravityScaleSlider.value); });

        elevatorRB.mass = elevatorMass;
        elevatorRB.drag = elevatorLinear;
        elevatorRB.angularDrag = elevatorAngular;
        elevatorRB.gravityScale = elevatorGravityScale;

        elevatorMassSlider.value = elevatorRB.mass;
        elevatorLinearSlider.value = elevatorRB.drag;
        elevatorAngularSlider.value = elevatorRB.angularDrag;
        elevatorGravityScaleSlider.value = elevatorRB.gravityScale;
        */

        elevatorSpeedSlider.onValueChanged.AddListener(delegate { HandleElevatorSpeedInputData(elevatorSpeedSlider.value); });
        elevatorControllerRef.movementSpeed = elevatorSpeed;
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

        elevatorControllerRef.MoveBarToStartPositionFunction();

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

    /*
    public void HandleElevatorMassInputData(float mass)
    {
        elevatorRB.mass = mass;
        elevatorMassTxt.text = mass.ToString();
    }

    public void HandleElevatorLinearInputData(float linear)
    {
        elevatorRB.drag = linear;
        elevatorLinearTxt.text = linear.ToString();
    }

    public void HandleElevatorAngularInputData(float angular)
    {
        elevatorRB.angularDrag = angular;
        elevatorAngularTxt.text = angular.ToString();
    }

    public void HandleElevatorGravityScaleInputData(float gravityScale)
    {
        elevatorRB.gravityScale = gravityScale;
        elevatorGravityScaleTxt.text = gravityScale.ToString();
    }
    */
}
