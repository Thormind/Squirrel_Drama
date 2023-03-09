using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PointsParameters", menuName = "Obstacles Parameters/Points Parameters")]
public class PointsParametersSO : ScriptableObject
{
    [SerializeField] private int[] _pointsQuantity = new int[9];
    [SerializeField] private float[] _pointsMinDistance = new float[9];


    // =================================== //
    // ============= GETTERS ============= //
    // =================================== //


    public int GetQuantityForLevel(int level)
    {
        if (level >= 1 && level <= _pointsQuantity.Length)
        {
            return _pointsQuantity[level - 1];
        }
        else
        {
            Debug.LogError($"Invalid level for PointsQuantityParameter: {level}");
            return 0;
        }
    }

    public float GetMinDistanceForLevel(int level)
    {
        if (level >= 1 && level <= _pointsMinDistance.Length)
        {
            return _pointsMinDistance[level - 1];
        }
        else
        {
            Debug.LogError($"Invalid level for PointsMinDistanceParameter: {level}");
            return 0;
        }
    }




    // =================================== //
    // ============= SETTERS ============= //
    // =================================== //

    public void SetQuantityForLevel(int level, int value)
    {
        if (level >= 1 && level <= _pointsQuantity.Length)
        {
            _pointsQuantity[level - 1] = value;
        }
        else
        {
            Debug.LogError($"Invalid level for PointsQuantityParameter: {level}");
        }
    }

    public void SetMinDistanceForLevel(int level, float value)
    {
        if (level >= 1 && level <= _pointsMinDistance.Length)
        {
            _pointsMinDistance[level - 1] = value;
        }
        else
        {
            Debug.LogError($"Invalid level for PointsMinDistanceParameter: {level}");
        }
    }
}
