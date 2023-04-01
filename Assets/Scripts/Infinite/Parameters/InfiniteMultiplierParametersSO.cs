using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InfiniteMultiplierParameters", menuName = "Multiplier Parameters/Infinite Multiplier Parameters")]
public class InfiniteMultiplierParametersSO : ScriptableObject
{
    [SerializeField] private float[] _multiplierAllowedTimeBetweenPoints = new float[5];


    // =================================== //
    // ============= GETTERS ============= //
    // =================================== //

    public float GetAllowedTimeBetweenPoints(int multiplier)
    {
        if (multiplier >= 1 && multiplier <= _multiplierAllowedTimeBetweenPoints.Length)
        {
            return _multiplierAllowedTimeBetweenPoints[multiplier - 1];
        }
        else
        {
            Debug.LogError($"Invalid multiplier for _multiplierAllowedTimeBetweenPoints: {multiplier}");
            return 0;
        }
    }


    // =================================== //
    // ============= SETTERS ============= //
    // =================================== //

    public void SetAllowedTimeBetweenPoints(int multiplier, float value)
    {
        if (multiplier >= 1 && multiplier <= _multiplierAllowedTimeBetweenPoints.Length)
        {
            _multiplierAllowedTimeBetweenPoints[multiplier - 1] = value;
        }
        else
        {
            Debug.LogError($"Invalid multiplier for _multiplierAllowedTimeBetweenPoints: {multiplier}");
        }
    }

}
