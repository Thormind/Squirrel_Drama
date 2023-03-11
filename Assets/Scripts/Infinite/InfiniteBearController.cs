using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteBearController : MonoBehaviour
{
    public static InfiniteBearController instance;

    public GameObject bearParent;
    public GameObject bearPrefab;
    public GameObject bearInstantiated;

    //Difficulty Parameters
    [SerializeField] private BearParametersSO _bearParameters;


    //Spawning Limits Parameters
    private float xMin = -2.5f;
    private float xMax = 2.5f;

    private float yMin = 4f;
    private float yMax = 38f;

    private Vector3 spawnPosition;
    private bool isSpawning;
    private float _nextSpawnTime;
    private int maxTries = 500;
    public bool isAllSpawned;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }

        isAllSpawned = false;
    }

    private void Start()
    {
        isSpawning = false;
    }

    private void Update()
    {
        if (isSpawning && Time.time >= _nextSpawnTime && Random.value < BearSpawnProbability && bearInstantiated == null)
        {
            SpawnBear();
            _nextSpawnTime = Time.time + Random.Range(BearMinSpawningInterval, BearMaxSpawningInterval);
        }
    }

    public void SpawnBears()
    {
        QuickRemoveBears();
        isSpawning = true;
    }

    public void SpawnBear()
    {
        Vector3 position = GetRandomPositionNearFruit();

        if (position == Vector3.zero)
        {
            NotifySpawnDebug(false);
        }
        else
        {
            NotifySpawnDebug(true);

            InstantiateAnimation(position, true);

        }
    }

    public void RemoveBears()
    {
        isSpawning = false;

        for (int i = 0; i < bearParent.transform.childCount; i++)
        {
            GameObject child = bearParent.transform.GetChild(i).gameObject;
            if (child != null)
            {
                InstantiateAnimation(child.transform.position, false, child);
            }
        }
    }

    public void QuickRemoveBears()
    {
        isSpawning = false;

        for (int i = 0; i < bearParent.transform.childCount; i++)
        {
            GameObject child = bearParent.transform.GetChild(i).gameObject;
            Destroy(child);
        }
    }

    public void InstantiateAnimation(Vector3 position, bool spawn, GameObject obj = null)
    {
        if (AnimationManager.instance != null)
        {
            AnimationManager.instance.PlayObstaclesAnimation(AnimateInstantiate(position, spawn, obj));
        }

    }

    private IEnumerator AnimateInstantiate(Vector3 position, bool spawn, GameObject obj = null)
    {
        //play sound 

        if (spawn)
        {
            Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(-45f, 45));
            bearInstantiated = Instantiate(bearPrefab, position, randomRotation, bearParent.transform);

            float randomWarnAnimationTime = Random.Range(BearWarnAnimationTime - 0.5f, BearWarnAnimationTime + 0.5f);

            bearInstantiated.GetComponent<InfiniteBearAnimation>().HandleBearAnimationFunction(
                randomWarnAnimationTime, BearImpactRange);
        }
        if (!spawn && obj != null)
        {
            Instantiate(InfiniteGameController.instance.obstacleInstanciateVFX, position, Quaternion.identity, bearParent.transform);
            Destroy(obj);
        }

        yield return new WaitForSeconds(0.025f);
    }

    private Vector3 GetRandomPositionNearFruit()
    {
        Vector3 spawnPosition;
        Vector3 localPosition;

        int tries = 0;
        do
        {

            float x = Random.Range(xMin, xMax);
            float y = Random.Range(yMin, yMax);

            localPosition = new Vector3(x, y, 0f);
            spawnPosition = bearParent.transform.TransformPoint(localPosition);

            tries++;
            if (tries >= maxTries)
            {
                spawnPosition = Vector3.zero;
                break;
            }
        } while (!IsValidPosition(localPosition));

        return spawnPosition;
    }

    private bool IsValidPosition(Vector3 position)
    {
        Vector3 fruitPosition = InfiniteGameController.instance.GetFruitLocalPosition();

        float distance = Vector2.Distance(position, fruitPosition);

        if (distance <= BearMaxDistance && position.y > fruitPosition.y + BearMinDistance && distance >= BearMinDistance)
        {
            return true;
        }

        return false;
    }

    private void NotifySpawnDebug(bool spawned)
    {
        isAllSpawned = spawned;

        if (LevelScalingController.instance != null)
        {
            LevelScalingController.instance.UpdateObstacleMessages();
        }
    }

    //MIN DISTANCE
    public float BearMinDistance
    {
        get { return _bearParameters.GetMinDistanceForLevel(InfiniteGameController.instance.difficultyLevel); }
        set
        {
            _bearParameters.SetMinDistanceForLevel(InfiniteGameController.instance.difficultyLevel, value);
            SpawnBears();
        }
    }

    //MAX DISTANCE
    public float BearMaxDistance
    {
        get { return _bearParameters.GetMaxDistanceForLevel(InfiniteGameController.instance.difficultyLevel); }
        set
        {
            _bearParameters.SetMaxDistanceForLevel(InfiniteGameController.instance.difficultyLevel, value);
            SpawnBears();
        }
    }

    //MIN SPAWNING INTERVAL
    public float BearMinSpawningInterval
    {
        get { return _bearParameters.GetMinSpawningIntervalForLevel(InfiniteGameController.instance.difficultyLevel); }
        set
        {
            _bearParameters.SetMinSpawningIntervalForLevel(InfiniteGameController.instance.difficultyLevel, value);
            SpawnBears();
        }
    }

    //MAX SPAWNING INTERVAL
    public float BearMaxSpawningInterval
    {
        get { return _bearParameters.GetMaxSpawningIntervalForLevel(InfiniteGameController.instance.difficultyLevel); }
        set
        {
            _bearParameters.SetMaxSpawningIntervalForLevel(InfiniteGameController.instance.difficultyLevel, value);
            SpawnBears();
        }
    }


    //SPAWNING PROBABILITY
    public float BearSpawnProbability
    {
        get { return _bearParameters.GetSpawnProbabilityForLevel(InfiniteGameController.instance.difficultyLevel); }
        set
        {
            _bearParameters.SetSpawnProbabilityForLevel(InfiniteGameController.instance.difficultyLevel, value);
            SpawnBears();
        }
    }


    //WARN ANIMATION TIME
    public float BearWarnAnimationTime
    {
        get { return _bearParameters.GetWarnAnimationTimeForLevel(InfiniteGameController.instance.difficultyLevel); }
        set
        {
            _bearParameters.SetWarnAnimationTimeForLevel(InfiniteGameController.instance.difficultyLevel, value);
            SpawnBears();
        }
    }

    //IMPACT RANGE
    public float BearImpactRange
    {
        get { return _bearParameters.GetImpactRangeForLevel(InfiniteGameController.instance.difficultyLevel); }
        set
        {
            _bearParameters.SetImpactRangeForLevel(InfiniteGameController.instance.difficultyLevel, value);
            SpawnBears();
        }
    }
}
