using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController3D : MonoBehaviour
{
    public static ElevatorController3D instance;

    [SerializeField] public Transform leftEnd, rightEnd;
    [SerializeField] public Transform elevatorPosition;
    [SerializeField] public Transform ballPosition;

    [SerializeField] public float speed = 1.0f;
    [SerializeField] private float minHeight = 0.0f;
    [SerializeField] private float maxHeight = 60.0f;
    [SerializeField] private float maxDistance = 5.0f;

    public Vector3 initialLeftEndPosition;
    public Vector3 initialRightEndPosition;
    public Vector3 initialBallPosition;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    void Start()
    {
        initialLeftEndPosition = leftEnd.localPosition;
        initialRightEndPosition = rightEnd.localPosition;
        initialBallPosition = ballPosition.position;

        HoleSpawner.instance.SpawnHoles();
        BeeSpawner.instance.SpawnBees();
    }

    void Update()
    {
        MoveEnds();
        TranslateElevatorAfterMove();
        RotateElevatorAfterMove();
        ScaleElevatorAfterMove();

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetElevator();
        }
    }

    private void MoveEnds()
    {
        float newLeftY = leftEnd.localPosition.y;
        float newRightY = rightEnd.localPosition.y;

        if (Input.GetKey(KeyCode.W))
        {
            newLeftY += speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            newLeftY -= speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.I))
        {
            newRightY += speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.K))
        {
            newRightY -= speed * Time.deltaTime;
        }

        newLeftY = Mathf.Clamp(newLeftY, minHeight, maxHeight);
        newRightY = Mathf.Clamp(newRightY, minHeight, maxHeight);

        float distance = Mathf.Abs(newLeftY - newRightY);

        if (distance > maxDistance)
        {
            newLeftY = leftEnd.localPosition.y;
            newRightY = rightEnd.localPosition.y;
        }

        leftEnd.localPosition = new Vector3(leftEnd.localPosition.x, newLeftY, leftEnd.localPosition.z);
        rightEnd.localPosition = new Vector3(rightEnd.localPosition.x, newRightY, rightEnd.localPosition.z);
    }

    public void TranslateElevatorAfterMove()
    {
        float newY = (leftEnd.position.y + rightEnd.position.y) / 2;
        elevatorPosition.position = new Vector3(elevatorPosition.position.x, newY, elevatorPosition.position.z);
    }

    public void RotateElevatorAfterMove()
    {
        float newRadZ = Mathf.Atan((rightEnd.position.y - leftEnd.position.y) / (rightEnd.position.x - leftEnd.position.x));
        float newDegZ = Mathf.Rad2Deg * newRadZ;
        elevatorPosition.rotation = Quaternion.Euler(new Vector3(0, 0, newDegZ));
    }

    public void ScaleElevatorAfterMove()
    {
        float newLength = Vector2.Distance(leftEnd.position, rightEnd.position);
        elevatorPosition.localScale = new Vector3(newLength, elevatorPosition.localScale.y, elevatorPosition.localScale.z);
    }

    public void ResetElevator()
    {
        leftEnd.localPosition = initialLeftEndPosition;
        rightEnd.localPosition = initialRightEndPosition;

        ballPosition.position = initialBallPosition;
        ballPosition.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

        TranslateElevatorAfterMove();
        RotateElevatorAfterMove();
        ScaleElevatorAfterMove();

        HoleSpawner.instance.RemoveHoles();
        HoleSpawner.instance.SpawnHoles();

        BeeSpawner.instance.RemoveBees();
        BeeSpawner.instance.SpawnBees();

        //GameSpawner.instance.RemoveObstacles();
        //GameSpawner.instance.SpawnObstacles();
    }

}

//public GUIStyle style = new GUIStyle();
//private void OnGUI()
//{
//    style.fontSize = 20;
//    style.normal.textColor = Color.white;

//    string data1 = "___________________________________________\n\n";
//    data1 += $"Radian Angle: {newRadZ}\n";
//    data1 += $"Deg Angle: {newDegZ}\n";
//    data1 += "___________________________________________\n\n";
//    data1 += $"leftEnd Position: {leftEnd.position}\n";
//    data1 += $"rightEnd Position: {rightEnd.position}\n";
//    data1 += "___________________________________________\n\n";
//    data1 += $"elevator Rotation: {elevatorPosition.rotation}\n";



//    GUI.Label(new Rect(10, 150, 400, 1000), data1, style);

//}

/*
float initialLeftY = leftEnd.localPosition.y;
float initialRightY = rightEnd.localPosition.y;

float newLeftY = leftEnd.localPosition.y;
float newRightY = rightEnd.localPosition.y;

//LEFT MOVEMENT
if (Input.GetKey(KeyCode.W))
{
    newLeftY += speed * Time.deltaTime;
}
else if (Input.GetKey(KeyCode.S))
{
    newLeftY -= speed * Time.deltaTime;
}

//RIGHT MOVEMENT
if (Input.GetKey(KeyCode.I))
{
    newRightY += speed * Time.deltaTime;
}
else if (Input.GetKey(KeyCode.K))
{
    newRightY -= speed * Time.deltaTime;
}

if (Input.GetKeyDown(KeyCode.R))
{
    Reset();
    return;
}


newLeftY = newLeftY < minHeight ? minHeight : newLeftY;
newRightY = newRightY < minHeight ? minHeight : newRightY;

newLeftY = newLeftY > maxHeight ? maxHeight : newLeftY;
newRightY = newRightY > maxHeight ? maxHeight : newRightY;

float distance = Mathf.Abs(newLeftY - newRightY);

newLeftY = distance > maxDistance ? initialLeftY : newLeftY;
newRightY = distance > maxDistance ? initialRightY : newRightY;

leftEnd.localPosition = new Vector3(leftEnd.localPosition.x, newLeftY, leftEnd.localPosition.z);
rightEnd.localPosition = new Vector3(rightEnd.localPosition.x, newRightY, rightEnd.localPosition.z);

TranslateElevatorAfterMove();
RotateElevatorAfterMove();
ScaleElevatorAfterMove();
*/
