using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InfiniteElevatorParameters", menuName = "Elevator Parameters/Infinite Elevator Parameters")]
public class InfiniteElevatorParametersSO : ScriptableObject
{
    [SerializeField] private float infiniteElevatorMovementSpeed;
    [SerializeField] private float infiniteElevatorStartMovementSpeed;
    [SerializeField] private float infiniteElevatorMaxDifference;
    [SerializeField] private float infiniteFruitGravityScale;
    [SerializeField] private float infiniteFruitFallingGravityScale;
    [SerializeField] private float infiniteFruitMinCollisionDistance;


    // =================================== //
    // ============= GETTERS ============= //
    // =================================== //


    public float GetMovementSpeed()
    {
        return infiniteElevatorMovementSpeed;
    }

    public float GetStartMovementSpeed()
    {
        return infiniteElevatorStartMovementSpeed;
    }

    public float GetMaxDifference()
    {
        return infiniteElevatorMaxDifference;
    }

    public float GetFruitGravityScale()
    {
        return infiniteFruitGravityScale;
    }

    public float GetFruitFallingGravityScale()
    {
        return infiniteFruitFallingGravityScale;
    }

    public float GetFruitMinCollisionDistance()
    {
        return infiniteFruitMinCollisionDistance;
    }




    // =================================== //
    // ============= SETTERS ============= //
    // =================================== //

    public void SetMovementSpeed(float value)
    {
        infiniteElevatorMovementSpeed = value;
    }

    public void SetStartMovementSpeed(float value)
    {
        infiniteElevatorStartMovementSpeed = value;
    }

    public void SetMaxDifference(float value)
    {
        infiniteElevatorMaxDifference = value;
    }

    public void SetFruitGravityScale(float value)
    {
        infiniteFruitGravityScale = value;
    }

    public void SetFruitFallingGravityScale(float value)
    {
        infiniteFruitFallingGravityScale = value;
    }

    public void SetFruitMinCollisionDistance(float value)
    {
        infiniteFruitMinCollisionDistance = value;
    }
}
