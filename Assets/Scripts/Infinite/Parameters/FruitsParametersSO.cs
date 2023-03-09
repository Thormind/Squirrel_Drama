using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FruitsParameters", menuName = "Obstacles Parameters/Fruits Parameters")]
public class FruitsParametersSO : ScriptableObject
{
    [SerializeField] private int[] _fruitsQuantity = new int[9];
    [SerializeField] private float[] _fruitsMinDistance = new float[9];


    // =================================== //
    // ============= GETTERS ============= //
    // =================================== //


    public int GetQuantityForLevel(int level)
    {
        if (level >= 1 && level <= _fruitsQuantity.Length)
        {
            return _fruitsQuantity[level - 1];
        }
        else
        {
            Debug.LogError($"Invalid level for FruitsQuantityParameter: {level}");
            return 0;
        }
    }

    public float GetMinDistanceForLevel(int level)
    {
        if (level >= 1 && level <= _fruitsMinDistance.Length)
        {
            return _fruitsMinDistance[level - 1];
        }
        else
        {
            Debug.LogError($"Invalid level for FruitsMinDistanceParameter: {level}");
            return 0;
        }
    }




    // =================================== //
    // ============= SETTERS ============= //
    // =================================== //

    public void SetQuantityForLevel(int level, int value)
    {
        if (level >= 1 && level <= _fruitsQuantity.Length)
        {
            _fruitsQuantity[level - 1] = value;
        }
        else
        {
            Debug.LogError($"Invalid level for FruitsQuantityParameter: {level}");
        }
    }

    public void SetMinDistanceForLevel(int level, float value)
    {
        if (level >= 1 && level <= _fruitsMinDistance.Length)
        {
            _fruitsMinDistance[level - 1] = value;
        }
        else
        {
            Debug.LogError($"Invalid level for FruitsMinDistanceParameter: {level}");
        }
    }
}
