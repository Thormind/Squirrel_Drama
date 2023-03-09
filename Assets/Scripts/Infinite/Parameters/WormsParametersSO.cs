using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WormsParameters", menuName = "Obstacles Parameters/Worms Parameters")]
public class WormsParametersSO : ScriptableObject
{
    [SerializeField] private float[] _wormsMinDistance = new float[9];
    [SerializeField] private float[] _wormsMaxDistance = new float[9];
    [SerializeField] private float[] _wormsMinSpawningInterval = new float[9];
    [SerializeField] private float[] _wormsMaxSpawningInterval = new float[9];
    [SerializeField] private float[] _wormsSpawnProbability = new float[9];
    [SerializeField] private float[] _wormsInAnimationTime = new float[9];
    [SerializeField] private float[] _wormsDerpAnimationTime = new float[9];
    [SerializeField] private float[] _wormsAnimationSpeed = new float[9];


    // =================================== //
    // ============= GETTERS ============= //
    // =================================== //

    public float GetMinDistanceForLevel(int level)
    {
        if (level >= 1 && level <= _wormsMinDistance.Length)
        {
            return _wormsMinDistance[level - 1];
        }
        else
        {
            Debug.LogError($"Invalid level for _wormsMinDistance Parameter: {level}");
            return 0;
        }
    }

    public float GetMaxDistanceForLevel(int level)
    {
        if (level >= 1 && level <= _wormsMaxDistance.Length)
        {
            return _wormsMaxDistance[level - 1];
        }
        else
        {
            Debug.LogError($"Invalid level for _wormsMaxDistance Parameter: {level}");
            return 0;
        }
    }

    public float GetMinSpawningIntervalForLevel(int level)
    {
        if (level >= 1 && level <= _wormsMinSpawningInterval.Length)
        {
            return _wormsMinSpawningInterval[level - 1];
        }
        else
        {
            Debug.LogError($"Invalid level for _wormsMinSpawningInterval Parameter: {level}");
            return 0;
        }
    }

    public float GetMaxSpawningIntervalForLevel(int level)
    {
        if (level >= 1 && level <= _wormsMaxSpawningInterval.Length)
        {
            return _wormsMaxSpawningInterval[level - 1];
        }
        else
        {
            Debug.LogError($"Invalid level for _wormsMaxSpawningInterval Parameter: {level}");
            return 0;
        }
    }

    public float GetSpawnProbabilityForLevel(int level)
    {
        if (level >= 1 && level <= _wormsSpawnProbability.Length)
        {
            return _wormsSpawnProbability[level - 1];
        }
        else
        {
            Debug.LogError($"Invalid level for _wormsSpawnProbability Parameter: {level}");
            return 0;
        }
    }

    public float GetInAnimationTimeForLevel(int level)
    {
        if (level >= 1 && level <= _wormsInAnimationTime.Length)
        {
            return _wormsInAnimationTime[level - 1];
        }
        else
        {
            Debug.LogError($"Invalid level for _wormsInAnimationTime Parameter: {level}");
            return 0;
        }
    }

    public float GetDerpAnimationTimeForLevel(int level)
    {
        if (level >= 1 && level <= _wormsDerpAnimationTime.Length)
        {
            return _wormsDerpAnimationTime[level - 1];
        }
        else
        {
            Debug.LogError($"Invalid level for _wormsDerpAnimationTime Parameter: {level}");
            return 0;
        }
    }

    public float GetAnimationSpeedForLevel(int level)
    {
        if (level >= 1 && level <= _wormsAnimationSpeed.Length)
        {
            return _wormsAnimationSpeed[level - 1];
        }
        else
        {
            Debug.LogError($"Invalid level for _wormsAnimationSpeed Parameter: {level}");
            return 0;
        }
    }





    // =================================== //
    // ============= SETTERS ============= //
    // =================================== //

    public void SetMinDistanceForLevel(int level, float value)
    {
        if (level >= 1 && level <= _wormsMinDistance.Length)
        {
            _wormsMinDistance[level - 1] = value;
        }
        else
        {
            Debug.LogError($"Invalid level for _wormsMinDistance Parameter: {level}");
        }
    }

    public void SetMaxDistanceForLevel(int level, float value)
    {
        if (level >= 1 && level <= _wormsMaxDistance.Length)
        {
            _wormsMaxDistance[level - 1] = value;
        }
        else
        {
            Debug.LogError($"Invalid level for _wormsMaxDistance Parameter: {level}");
        }
    }

    public void SetMinSpawningIntervalForLevel(int level, float value)
    {
        if (level >= 1 && level <= _wormsMinSpawningInterval.Length)
        {
            _wormsMinSpawningInterval[level - 1] = value;
        }
        else
        {
            Debug.LogError($"Invalid level for _wormsMinSpawningInterval Parameter: {level}");
        }
    }

    public void SetMaxSpawningIntervalForLevel(int level, float value)
    {
        if (level >= 1 && level <= _wormsMaxSpawningInterval.Length)
        {
            _wormsMaxSpawningInterval[level - 1] = value;
        }
        else
        {
            Debug.LogError($"Invalid level for _wormsMaxSpawningInterval Parameter: {level}");
        }
    }

    public void SetSpawnProbabilityForLevel(int level, float value)
    {
        if (level >= 1 && level <= _wormsSpawnProbability.Length)
        {
            _wormsSpawnProbability[level - 1] = value;
        }
        else
        {
            Debug.LogError($"Invalid level for _wormsSpawnProbability Parameter: {level}");
        }
    }

    public void SetInAnimationTimeForLevel(int level, float value)
    {
        if (level >= 1 && level <= _wormsInAnimationTime.Length)
        {
            _wormsInAnimationTime[level - 1] = value;
        }
        else
        {
            Debug.LogError($"Invalid level for _wormsInAnimationTime Parameter: {level}");
        }
    }

    public void SetDerpAnimationTimeForLevel(int level, float value)
    {
        if (level >= 1 && level <= _wormsDerpAnimationTime.Length)
        {
            _wormsDerpAnimationTime[level - 1] = value;
        }
        else
        {
            Debug.LogError($"Invalid level for _wormsDerpAnimationTime Parameter: {level}");
        }
    }

    public void SetAnimationSpeedForLevel(int level, float value)
    {
        if (level >= 1 && level <= _wormsAnimationSpeed.Length)
        {
            _wormsAnimationSpeed[level - 1] = value;
        }
        else
        {
            Debug.LogError($"Invalid level for _wormsAnimationSpeed Parameter: {level}");
        }
    }
}
