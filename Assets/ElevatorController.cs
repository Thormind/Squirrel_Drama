using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    [SerializeField] public Transform leftEnd, rightEnd;
    [SerializeField] public Transform elevatorPosition;

    [SerializeField] public float speed = 1.0f;

    public float newRadZ;
    public float newDegZ;

    public Vector3 initialLeftEndPosition;
    public Vector3 initialRightEndPosition;

    [SerializeField] private float minHeight = 0.0f;
    [SerializeField] private float maxHeight = 30.0f;

    void Start()
    {
        initialLeftEndPosition = leftEnd.position;
        initialRightEndPosition = rightEnd.position;
    }

    void Update()
    {

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


        newLeftY = newLeftY < minHeight ? minHeight : newLeftY;
        newRightY = newRightY < minHeight ? minHeight : newRightY;

        newLeftY = newLeftY > maxHeight ? maxHeight : newLeftY;
        newRightY = newRightY > maxHeight ? maxHeight : newRightY;


        leftEnd.localPosition = new Vector3(leftEnd.localPosition.x, newLeftY, leftEnd.localPosition.z);
        rightEnd.localPosition = new Vector3(rightEnd.localPosition.x, newRightY, rightEnd.localPosition.z);

        translateAfterMove();
        rotateAfterMove();
        scaleAfterMove();
    }

    public void translateAfterMove()
    {
        float newY = (leftEnd.position.y + rightEnd.position.y) / 2;
        elevatorPosition.position = new Vector3(elevatorPosition.position.x, newY, elevatorPosition.position.z);
    }
    public void rotateAfterMove()
    {
        newRadZ = Mathf.Atan((rightEnd.position.y - leftEnd.position.y) / (rightEnd.position.x - leftEnd.position.x));
        newDegZ = Mathf.Rad2Deg * newRadZ;
        elevatorPosition.rotation = Quaternion.Euler(new Vector3(0, 0, newDegZ));
    }
    public void scaleAfterMove()
    {
        float newLength = Mathf.Sqrt(Mathf.Pow((leftEnd.position.x - rightEnd.position.x), 2)
            + Mathf.Pow((leftEnd.position.y - rightEnd.position.y), 2));

        elevatorPosition.localScale = new Vector3(newLength, elevatorPosition.localScale.y, elevatorPosition.localScale.z);
    }

    public GUIStyle style = new GUIStyle();
    private void OnGUI()
    {
        style.fontSize = 20;
        style.normal.textColor = Color.white;

        string data1 = "___________________________________________\n\n";
        data1 += $"Radian Angle: {newRadZ}\n";
        data1 += $"Deg Angle: {newDegZ}\n";
        data1 += "___________________________________________\n\n";
        data1 += $"leftEnd Position: {leftEnd.position}\n";
        data1 += $"rightEnd Position: {rightEnd.position}\n";
        data1 += "___________________________________________\n\n";
        data1 += $"elevator Rotation: {elevatorPosition.rotation}\n";



        GUI.Label(new Rect(10, 150, 400, 1000), data1, style);

    }


}
