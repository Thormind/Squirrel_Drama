using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteBearController : MonoBehaviour
{
    public static InfiniteBearController instance;

    public GameObject bearParent;
    public GameObject bearPrefab;
    public GameObject bearInstantiated;

    public List<GameObject> bears;

    //Difficulty Parameters
    private float[] _bearMinDistance = new float[9]; //{ 0.3f, 0.35f, 0.40f, 0.45f, 0.55f, 0.65f, 0.75f, 0.85f, 1f } ;
    private float[] _bearMaxDistance = new float[9]; //{ 0.3f, 0.35f, 0.40f, 0.45f, 0.55f, 0.65f, 0.75f, 0.85f, 1f } ;
    private float[] _bearMinSpawningInterval = new float[9]; //{ 0.3f, 0.35f, 0.40f, 0.45f, 0.55f, 0.65f, 0.75f, 0.85f, 1f } ;
    private float[] _bearMaxSpawningInterval = new float[9]; //{ 0.3f, 0.35f, 0.40f, 0.45f, 0.55f, 0.65f, 0.75f, 0.85f, 1f } ;
    private float[] _bearSpawnProbability = new float[9]; //{ 0.3f, 0.35f, 0.40f, 0.45f, 0.55f, 0.65f, 0.75f, 0.85f, 1f } ;
    private float[] _bearWarnAnimationTime = new float[9]; //{ 0.3f, 0.35f, 0.40f, 0.45f, 0.55f, 0.65f, 0.75f, 0.85f, 1f } ;
    private float[] _bearImpactRange = new float[9]; //{ 0.3f, 0.35f, 0.40f, 0.45f, 0.55f, 0.65f, 0.75f, 0.85f, 1f } ;


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

        LoadBearMinDistance();
        LoadBearMaxDistance();
        LoadBearMinSpawningInterval();
        LoadBearMaxSpawningInterval();
        LoadBearSpawnProbability();
        LoadBearWarnAnimationTime();
        LoadBearImpactRange();

        isAllSpawned = false;
    }

    private void Start()
    {
        isSpawning = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.R))
        {
            RemoveBears();
            SpawnBear();
        }

        if (isSpawning && Time.time >= _nextSpawnTime && Random.value < BearSpawnProbability && bearInstantiated == null)
        {
            SpawnBear();
            _nextSpawnTime = Time.time + Random.Range(BearMinSpawningInterval, BearMaxSpawningInterval);
        }
    }

    public void SpawnBears()
    {
        RemoveBears();
        isSpawning = true;
    }

    public void SpawnBear()
    {
        Vector3 position = GetRandomPositionNearFruit();

        if (position == Vector3.zero)
        {
            Debug.Log("Could not find a valid BEAR spawn position after " + maxTries + " tries.");
            NotifySpawnDebug(false);
        }
        else
        {
            NotifySpawnDebug(true);

            //Vector3 worldPosition = bearParent.transform.TransformPoint(position);
            Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(-45f, 45));
            bearInstantiated = Instantiate(bearPrefab, position, randomRotation, bearParent.transform);

            float randomWarnAnimationTime = Random.Range(BearWarnAnimationTime - 1f, BearWarnAnimationTime + 1f);
            float randomImpactRange = Random.Range(BearImpactRange - 0.25f, BearImpactRange + 0.25f);

            bearInstantiated.GetComponent<InfiniteBearAnimation>().HandleBearAnimationFunction(
                randomWarnAnimationTime, randomImpactRange);

            bears.Add(bearInstantiated);
        }
    }

    public void RemoveBears()
    {
        isSpawning = false;

        foreach (GameObject g in bears)
        {
            Destroy(g);
        }

        bears.Clear();
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

            localPosition = new Vector3(x, y, 0.1f);
            spawnPosition = bearParent.transform.TransformPoint(localPosition);

            //print($"Bear Local Position (from position finder) : {localPosition}");
            //print($"Bear World Position (from position finder) : {spawnPosition}");

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

        //print($"Bear Local Position (from position finder) : {position}");
        //print($"Fruit Local Position (from position finder) : {fruitPosition}");

        float distance = Vector2.Distance(position, fruitPosition);
        //print($"Distance: {distance}");

        if (distance <= BearMaxDistance && position.y > fruitPosition.y && distance >= BearMinDistance)
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
        get { return _bearMinDistance[InfiniteGameController.instance.difficultyLevel - 1]; }
        set
        {
            _bearMinDistance[InfiniteGameController.instance.difficultyLevel - 1] = value;
            SaveBearMinDistance();
            SpawnBears();
        }
    }

    private void SaveBearMinDistance()
    {
        for (int i = 0; i < _bearMinDistance.Length; i++)
        {
            PlayerPrefs.SetFloat("BearMinDistance_Level_" + (i + 1), _bearMinDistance[i]);
        }
    }

    private void LoadBearMinDistance()
    {
        for (int i = 0; i < _bearMinDistance.Length; i++)
        {
            _bearMinDistance[i] = PlayerPrefs.GetFloat("BearMinDistance_Level_" + (i + 1), 50);
        }
    }

    //MAX DISTANCE
    public float BearMaxDistance
    {
        get { return _bearMaxDistance[InfiniteGameController.instance.difficultyLevel - 1]; }
        set
        {
            _bearMaxDistance[InfiniteGameController.instance.difficultyLevel - 1] = value;
            SaveBearMaxDistance();
            SpawnBears();
        }
    }

    private void SaveBearMaxDistance()
    {
        for (int i = 0; i < _bearMaxDistance.Length; i++)
        {
            PlayerPrefs.SetFloat("BearMaxDistance_Level_" + (i + 1), _bearMaxDistance[i]);
        }
    }

    private void LoadBearMaxDistance()
    {
        for (int i = 0; i < _bearMaxDistance.Length; i++)
        {
            _bearMaxDistance[i] = PlayerPrefs.GetFloat("BearMaxDistance_Level_" + (i + 1), 50);
        }
    }

    //MIN SPAWNING INTERVAL
    public float BearMinSpawningInterval
    {
        get { return _bearMinSpawningInterval[InfiniteGameController.instance.difficultyLevel - 1]; }
        set
        {
            _bearMinSpawningInterval[InfiniteGameController.instance.difficultyLevel - 1] = value;
            SaveBearMinSpawningInterval();
            SpawnBears();
        }
    }

    private void SaveBearMinSpawningInterval()
    {
        for (int i = 0; i < _bearMinSpawningInterval.Length; i++)
        {
            PlayerPrefs.SetFloat("BearMinSpawningInterval_Level_" + (i + 1), _bearMinSpawningInterval[i]);
        }
    }

    private void LoadBearMinSpawningInterval()
    {
        for (int i = 0; i < _bearMinSpawningInterval.Length; i++)
        {
            _bearMinSpawningInterval[i] = PlayerPrefs.GetFloat("BearMinSpawningInterval_Level_" + (i + 1), 50);
        }
    }

    //MAX SPAWNING INTERVAL
    public float BearMaxSpawningInterval
    {
        get { return _bearMaxSpawningInterval[InfiniteGameController.instance.difficultyLevel - 1]; }
        set
        {
            _bearMaxSpawningInterval[InfiniteGameController.instance.difficultyLevel - 1] = value;
            SaveBearMaxSpawningInterval();
            SpawnBears();
        }
    }

    private void SaveBearMaxSpawningInterval()
    {
        for (int i = 0; i < _bearMaxSpawningInterval.Length; i++)
        {
            PlayerPrefs.SetFloat("BearMaxSpawningInterval_Level_" + (i + 1), _bearMaxSpawningInterval[i]);
        }
    }

    private void LoadBearMaxSpawningInterval()
    {
        for (int i = 0; i < _bearMaxSpawningInterval.Length; i++)
        {
            _bearMaxSpawningInterval[i] = PlayerPrefs.GetFloat("BearMaxSpawningInterval_Level_" + (i + 1), 50);
        }
    }

    //SPAWNING PROBABILITY
    public float BearSpawnProbability
    {
        get { return _bearSpawnProbability[InfiniteGameController.instance.difficultyLevel - 1]; }
        set
        {
            _bearSpawnProbability[InfiniteGameController.instance.difficultyLevel - 1] = value;
            SaveBearSpawnProbability();
            SpawnBears();
        }
    }

    private void SaveBearSpawnProbability()
    {
        for (int i = 0; i < _bearSpawnProbability.Length; i++)
        {
            PlayerPrefs.SetFloat("BearSpawnProbability_Level_" + (i + 1), _bearSpawnProbability[i]);
        }
    }

    private void LoadBearSpawnProbability()
    {
        for (int i = 0; i < _bearSpawnProbability.Length; i++)
        {
            _bearSpawnProbability[i] = PlayerPrefs.GetFloat("BearSpawnProbability_Level_" + (i + 1), 50);
        }
    }

    //WARN ANIMATION TIME
    public float BearWarnAnimationTime
    {
        get { return _bearWarnAnimationTime[InfiniteGameController.instance.difficultyLevel - 1]; }
        set
        {
            _bearWarnAnimationTime[InfiniteGameController.instance.difficultyLevel - 1] = value;
            SaveBearWarnAnimationTime();
            SpawnBears();
        }
    }

    private void SaveBearWarnAnimationTime()
    {
        for (int i = 0; i < _bearWarnAnimationTime.Length; i++)
        {
            PlayerPrefs.SetFloat("BearWarnAnimationTime_Level_" + (i + 1), _bearWarnAnimationTime[i]);
        }
    }

    private void LoadBearWarnAnimationTime()
    {
        for (int i = 0; i < _bearWarnAnimationTime.Length; i++)
        {
            _bearWarnAnimationTime[i] = PlayerPrefs.GetFloat("BearWarnAnimationTime_Level_" + (i + 1), 50);
        }
    }

    //IMPACT RANGE
    public float BearImpactRange
    {
        get { return _bearImpactRange[InfiniteGameController.instance.difficultyLevel - 1]; }
        set
        {
            _bearImpactRange[InfiniteGameController.instance.difficultyLevel - 1] = value;
            SaveBearImpactRange();
            SpawnBears();
        }
    }

    private void SaveBearImpactRange()
    {
        for (int i = 0; i < _bearImpactRange.Length; i++)
        {
            PlayerPrefs.SetFloat("BearImpactRange_Level_" + (i + 1), _bearImpactRange[i]);
        }
    }

    private void LoadBearImpactRange()
    {
        for (int i = 0; i < _bearImpactRange.Length; i++)
        {
            _bearImpactRange[i] = PlayerPrefs.GetFloat("BearImpactRange_Level_" + (i + 1), 50);
        }
    }
}
