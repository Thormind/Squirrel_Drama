using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BeesParameters", menuName = "Obstacles Parameters/Bees Parameters")]
public class BeesParametersSO : ScriptableObject
{
    [SerializeField] private int[] _beesQuantity = new int[9];
    [SerializeField] private float[] _beesMinDistance = new float[9];
    [SerializeField] private float[] _beesMovementSpeed = new float[9];


    // =================================== //
    // ============= GETTERS ============= //
    // =================================== //


    public int GetQuantityForLevel(int level)
    {
        if (level >= 1 && level <= _beesQuantity.Length)
        {
            return _beesQuantity[level - 1];
        }
        else
        {
            Debug.LogError($"Invalid level for BeesQuantityParameter: {level}");
            return 0;
        }
    }

    public float GetMinDistanceForLevel(int level)
    {
        if (level >= 1 && level <= _beesMinDistance.Length)
        {
            return _beesMinDistance[level - 1];
        }
        else
        {
            Debug.LogError($"Invalid level for BeesMinDistanceParameter: {level}");
            return 0;
        }
    }

    public float GetMovementSpeedForLevel(int level)
    {
        if (level >= 1 && level <= _beesMovementSpeed.Length)
        {
            return _beesMovementSpeed[level - 1];
        }
        else
        {
            Debug.LogError($"Invalid level for BeesMovementSpeedParameter: {level}");
            return 0;
        }
    }




    // =================================== //
    // ============= SETTERS ============= //
    // =================================== //

    public void SetQuantityForLevel(int level, int value)
    {
        if (level >= 1 && level <= _beesQuantity.Length)
        {
            _beesQuantity[level - 1] = value;
        }
        else
        {
            Debug.LogError($"Invalid level for BeesQuantityParameter: {level}");
        }
    }

    public void SetMinDistanceForLevel(int level, float value)
    {
        if (level >= 1 && level <= _beesMinDistance.Length)
        {
            _beesMinDistance[level - 1] = value;
        }
        else
        {
            Debug.LogError($"Invalid level for BeesMinDistanceParameter: {level}");
        }
    }

    public void SetMovementSpeedForLevel(int level, float value)
    {
        if (level >= 1 && level <= _beesMovementSpeed.Length)
        {
            _beesMovementSpeed[level - 1] = value;
        }
        else
        {
            Debug.LogError($"Invalid level for BeesMovementSpeedParameter: {level}");
        }
    }
}
