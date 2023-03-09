using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BearParameters", menuName = "Obstacles Parameters/Bear Parameters")]
public class BearParametersSO : ScriptableObject
{
    [SerializeField] private float[] _bearMinDistance = new float[9]; 
    [SerializeField] private float[] _bearMaxDistance = new float[9]; 
    [SerializeField] private float[] _bearMinSpawningInterval = new float[9]; 
    [SerializeField] private float[] _bearMaxSpawningInterval = new float[9]; 
    [SerializeField] private float[] _bearSpawnProbability = new float[9]; 
    [SerializeField] private float[] _bearWarnAnimationTime = new float[9]; 
    [SerializeField] private float[] _bearImpactRange = new float[9];


    // =================================== //
    // ============= GETTERS ============= //
    // =================================== //

    public float GetMinDistanceForLevel(int level)
    {
        if (level >= 1 && level <= _bearMinDistance.Length)
        {
            return _bearMinDistance[level - 1];
        }
        else
        {
            Debug.LogError($"Invalid level for _bearMinDistance Parameter: {level}");
            return 0;
        }
    }

    public float GetMaxDistanceForLevel(int level)
    {
        if (level >= 1 && level <= _bearMaxDistance.Length)
        {
            return _bearMaxDistance[level - 1];
        }
        else
        {
            Debug.LogError($"Invalid level for _bearMaxDistance Parameter: {level}");
            return 0;
        }
    }

    public float GetMinSpawningIntervalForLevel(int level)
    {
        if (level >= 1 && level <= _bearMinSpawningInterval.Length)
        {
            return _bearMinSpawningInterval[level - 1];
        }
        else
        {
            Debug.LogError($"Invalid level for _bearMinSpawningInterval Parameter: {level}");
            return 0;
        }
    }

    public float GetMaxSpawningIntervalForLevel(int level)
    {
        if (level >= 1 && level <= _bearMaxSpawningInterval.Length)
        {
            return _bearMaxSpawningInterval[level - 1];
        }
        else
        {
            Debug.LogError($"Invalid level for _bearMaxSpawningInterval Parameter: {level}");
            return 0;
        }
    }

    public float GetSpawnProbabilityForLevel(int level)
    {
        if (level >= 1 && level <= _bearSpawnProbability.Length)
        {
            return _bearSpawnProbability[level - 1];
        }
        else
        {
            Debug.LogError($"Invalid level for _bearSpawnProbability Parameter: {level}");
            return 0;
        }
    }

    public float GetWarnAnimationTimeForLevel(int level)
    {
        if (level >= 1 && level <= _bearWarnAnimationTime.Length)
        {
            return _bearWarnAnimationTime[level - 1];
        }
        else
        {
            Debug.LogError($"Invalid level for _bearWarnAnimationTime Parameter: {level}");
            return 0;
        }
    }

    public float GetImpactRangeForLevel(int level)
    {
        if (level >= 1 && level <= _bearImpactRange.Length)
        {
            return _bearImpactRange[level - 1];
        }
        else
        {
            Debug.LogError($"Invalid level for _bearImpactRange Parameter: {level}");
            return 0;
        }
    }





    // =================================== //
    // ============= SETTERS ============= //
    // =================================== //

    public void SetMinDistanceForLevel(int level, float value)
    {
        if (level >= 1 && level <= _bearMinDistance.Length)
        {
            _bearMinDistance[level - 1] = value;
        }
        else
        {
            Debug.LogError($"Invalid level for _bearMinDistance Parameter: {level}");
        }
    }

    public void SetMaxDistanceForLevel(int level, float value)
    {
        if (level >= 1 && level <= _bearMaxDistance.Length)
        {
            _bearMaxDistance[level - 1] = value;
        }
        else
        {
            Debug.LogError($"Invalid level for _bearMaxDistance Parameter: {level}");
        }
    }

    public void SetMinSpawningIntervalForLevel(int level, float value)
    {
        if (level >= 1 && level <= _bearMinSpawningInterval.Length)
        {
            _bearMinSpawningInterval[level - 1] = value;
        }
        else
        {
            Debug.LogError($"Invalid level for _bearMinSpawningInterval Parameter: {level}");
        }
    }

    public void SetMaxSpawningIntervalForLevel(int level, float value)
    {
        if (level >= 1 && level <= _bearMaxSpawningInterval.Length)
        {
            _bearMaxSpawningInterval[level - 1] = value;
        }
        else
        {
            Debug.LogError($"Invalid level for _bearMaxSpawningInterval Parameter: {level}");
        }
    }

    public void SetSpawnProbabilityForLevel(int level, float value)
    {
        if (level >= 1 && level <= _bearSpawnProbability.Length)
        {
            _bearSpawnProbability[level - 1] = value;
        }
        else
        {
            Debug.LogError($"Invalid level for _bearSpawnProbability Parameter: {level}");
        }
    }

    public void SetWarnAnimationTimeForLevel(int level, float value)
    {
        if (level >= 1 && level <= _bearWarnAnimationTime.Length)
        {
            _bearWarnAnimationTime[level - 1] = value;
        }
        else
        {
            Debug.LogError($"Invalid level for _bearWarnAnimationTime Parameter: {level}");
        }
    }

    public void SetImpactRangeForLevel(int level, float value)
    {
        if (level >= 1 && level <= _bearImpactRange.Length)
        {
            _bearImpactRange[level - 1] = value;
        }
        else
        {
            Debug.LogError($"Invalid level for _bearImpactRange Parameter: {level}");
        }
    }
}
