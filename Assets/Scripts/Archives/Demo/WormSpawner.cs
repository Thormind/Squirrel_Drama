using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormSpawner : MonoBehaviour
{
    public GameObject wormsParent;
    public GameObject wormPrefab;

    //Difficulty Parameters
    [SerializeField] private WormsParametersSO _wormsParameters;

    private Vector3 spawnPosition;

    [SerializeField] public int difficultyLevel;

    private void Start()
    {
        difficultyLevel = 1;
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnWorm();
        }
    }

    public void SpawnWorm()
    {
        QuickRemoveWorms();

        Vector3 holePos = new Vector3(0f, 2f, 0f);
        Debug.Log("wormParent: " + wormsParent.transform.position);
        Vector3 worldPosition = wormsParent.transform.TransformPoint(holePos);
        


        InstantiateAnimation(worldPosition, true);
    }

    public void QuickRemoveWorms()
    {
        for (int i = 0; i < wormsParent.transform.childCount; i++)
        {
            GameObject child = wormsParent.transform.GetChild(i).gameObject;
            Destroy(child);
        }
    }

    public void InstantiateAnimation(Vector3 position, bool spawn, GameObject obj = null)
    {

        Debug.Log("wormParentTransformed: " + position);
        GameObject wormInstantiated = Instantiate(wormPrefab, position, Quaternion.Euler(0, 0, 0), wormsParent.transform);

        float randomInAnimationTime = Random.Range(WormsInAnimationTime - 0.5f, WormsInAnimationTime + 0.5f);
        float randomNumberOfDerpsAnimation = Random.Range(WormsDerpAnimationTime - 1f, WormsDerpAnimationTime + 1f);
        float randomAnimationSpeed = Random.Range(WormsAnimationSpeed, WormsAnimationSpeed + 0.2f);

        
        wormInstantiated.GetComponent<InfiniteWormsAnimation>().HandleNewWormAnimationFunction(
            randomInAnimationTime, (int) randomNumberOfDerpsAnimation, randomAnimationSpeed);

    }

    //MIN DISTANCE
    public float WormsMinDistance
    {
        get { return _wormsParameters.GetMinDistanceForLevel(difficultyLevel); }
        set
        {
            _wormsParameters.SetMinDistanceForLevel(difficultyLevel, value);
            SpawnWorm();
        }
    }


    //MAX DISTANCE
    public float WormsMaxDistance
    {
        get { return _wormsParameters.GetMaxDistanceForLevel(difficultyLevel); }
        set
        {
            _wormsParameters.SetMaxDistanceForLevel(difficultyLevel, value);
            SpawnWorm();
        }
    }


    //MIN SPAWNING INTERVAL
    public float WormsMinSpawningInterval
    {
        get { return _wormsParameters.GetMinSpawningIntervalForLevel(difficultyLevel); }
        set
        {
            _wormsParameters.SetMinSpawningIntervalForLevel(difficultyLevel, value);
            SpawnWorm();
        }
    }


    //MAX SPAWNING INTERVAL
    public float WormsMaxSpawningInterval
    {
        get { return _wormsParameters.GetMaxSpawningIntervalForLevel(difficultyLevel); }
        set
        {
            _wormsParameters.SetMaxSpawningIntervalForLevel(difficultyLevel, value);
            SpawnWorm();
        }
    }


    //SPAWNING PROBABILITY
    public float WormsSpawnProbability
    {
        get { return _wormsParameters.GetSpawnProbabilityForLevel(difficultyLevel); }
        set
        {
            _wormsParameters.SetSpawnProbabilityForLevel(difficultyLevel, value);
            SpawnWorm();
        }
    }


    //IN ANIMATION TIME
    public float WormsInAnimationTime
    {
        get { return _wormsParameters.GetInAnimationTimeForLevel(difficultyLevel); }
        set
        {
            _wormsParameters.SetInAnimationTimeForLevel(difficultyLevel, value);
            SpawnWorm();
        }
    }

    //DERP ANIMATION TIME
    public float WormsDerpAnimationTime
    {
        get { return _wormsParameters.GetDerpAnimationTimeForLevel(difficultyLevel); }
        set
        {
            _wormsParameters.SetDerpAnimationTimeForLevel(difficultyLevel, value);
            SpawnWorm();
        }
    }

    //ANIMATION SPEED
    public float WormsAnimationSpeed
    {
        get { return _wormsParameters.GetAnimationSpeedForLevel(difficultyLevel); }
        set
        {
            _wormsParameters.SetAnimationSpeedForLevel(difficultyLevel, value);
            SpawnWorm();
        }
    }

}
