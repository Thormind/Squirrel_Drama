using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HolesParameters", menuName = "Obstacles Parameters/Holes Parameters")]
public class HolesParametersSO : ScriptableObject
{
    [SerializeField] private int[] _holesQuantity = new int[9];
    [SerializeField] private float[] _holesMinDistance = new float[9];


    // =================================== //
    // ============= GETTERS ============= //
    // =================================== //


    public int GetQuantityForLevel(int level)
    {
        if (level >= 1 && level <= _holesQuantity.Length)
        {
            return _holesQuantity[level - 1];
        }
        else
        {
            Debug.LogError($"Invalid level for HolesQuantityParameter: {level}");
            return 0;
        }
    }

    public float GetMinDistanceForLevel(int level)
    {
        if (level >= 1 && level <= _holesMinDistance.Length)
        {
            return _holesMinDistance[level - 1];
        }
        else
        {
            Debug.LogError($"Invalid level for HolesMinDistanceParameter: {level}");
            return 0;
        }
    }




    // =================================== //
    // ============= SETTERS ============= //
    // =================================== //

    public void SetQuantityForLevel(int level, int value)
    {
        if (level >= 1 && level <= _holesQuantity.Length)
        {
            _holesQuantity[level - 1] = value;
        }
        else
        {
            Debug.LogError($"Invalid level for HolesQuantityParameter: {level}");
        }
    }

    public void SetMinDistanceForLevel(int level, float value)
    {
        if (level >= 1 && level <= _holesMinDistance.Length)
        {
            _holesMinDistance[level - 1] = value;
        }
        else
        {
            Debug.LogError($"Invalid level for HolesMinDistanceParameter: {level}");
        }
    }
}
