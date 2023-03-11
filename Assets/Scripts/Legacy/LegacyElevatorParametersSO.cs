using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LegacyElevatorParameters", menuName = "Elevator Parameters/Legacy Elevator Parameters")]
public class LegacyElevatorParametersSO : ScriptableObject
{
    [SerializeField] private float legacyElevatorMovementSpeed;
    [SerializeField] private float legacyElevatorMaxDifference;
    [SerializeField] private float legacyBallGravityScale;
    [SerializeField] private float legacyBallMinCollisionDistance;
    [SerializeField] private int legacyHolesQuantity;
    [SerializeField] private float legacyHolesMinDistance;


    // =================================== //
    // ============= GETTERS ============= //
    // =================================== //


    public float GetMovementSpeed()
    {
        return legacyElevatorMovementSpeed;
    }

    public float GetMaxDifference()
    {
        return legacyElevatorMaxDifference;
    }

    public float GetBallGravityScale()
    {
        return legacyBallGravityScale;
    }

    public float GetBallMinCollisionDistance()
    {
        return legacyBallMinCollisionDistance;
    }

    public int GetHolesQuantity()
    {
        return legacyHolesQuantity;
    }

    public float GetHolesMinDistance()
    {
        return legacyHolesMinDistance;
    }




    // =================================== //
    // ============= SETTERS ============= //
    // =================================== //

    public void SetMovementSpeed(float value)
    {
        legacyElevatorMovementSpeed = value;
    }

    public void SetMaxDifference(float value)
    {
        legacyElevatorMaxDifference = value;
    }

    public void SetBallGravityScale(float value)
    {
        legacyBallGravityScale = value;
    }

    public void SetBallMinCollisionDistance(float value)
    {
        legacyBallMinCollisionDistance = value;
    }

    public void SetHolesQuantity(int value)
    {
        legacyHolesQuantity = value;
    }

    public void SetHolesMinDistance(float value)
    {
        legacyHolesMinDistance = value;
    }
}
