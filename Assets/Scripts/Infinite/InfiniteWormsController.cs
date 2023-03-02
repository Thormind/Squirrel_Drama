using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteWormsController : MonoBehaviour
{
    public static InfiniteWormsController instance;

    public GameObject wormsParent;
    public GameObject wormPrefab;

    public List<GameObject> worms;
    private List<Vector3> _spawnedWormsPositions = new List<Vector3>();

    //Difficulty Parameters
    private float[] _wormsMinDistance = new float[9];
    private float[] _wormsMaxDistance = new float[9];
    private float[] _wormsMinSpawningInterval = new float[9];
    private float[] _wormsMaxSpawningInterval = new float[9];
    private float[] _wormsSpawnProbability = new float[9];
    private float[] _wormsInAnimationTime = new float[9];
    private float[] _wormsDerpAnimationTime = new float[9];
    private float[] _wormsAnimationSpeed = new float[9];


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

        LoadWormsMinDistance();
        LoadWormsMaxDistance();
        LoadWormsMinSpawningInterval();
        LoadWormsMaxSpawningInterval();
        LoadWormsSpawnProbability();
        LoadWormsInAnimationTime();
        LoadWormsDerpAnimationTime();
        LoadWormsAnimationSpeed();

        isAllSpawned = false;
    }

    private void Start()
    {
        isSpawning = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V) || Input.GetKeyDown(KeyCode.R))
        {
            RemoveWorms();
            SpawnWorm();
        }

        if (isSpawning && Time.time >= _nextSpawnTime && Random.value < WormsSpawnProbability)
        {
            SpawnWorm();
            _nextSpawnTime = Time.time + Random.Range(WormsMinSpawningInterval, WormsMaxSpawningInterval);
        }
    }

    public void SpawnWorms()
    {
        RemoveWorms();
        isSpawning = true;
    }

    public void SpawnWorm()
    {
        Vector3 holePosition = GetRandomHoleNearFruit();

        if (holePosition == Vector3.zero)
        {
            Debug.Log("Could not find a valid WORM spawn position after " + maxTries + " tries.");
            NotifySpawnDebug(false);
        }
        else
        {
            NotifySpawnDebug(true);
            holePosition = new Vector3(holePosition.x, holePosition.y, 1f);

            Vector3 worldPosition = wormsParent.transform.TransformPoint(holePosition);
            GameObject wormInstantiated = Instantiate(wormPrefab, worldPosition, Quaternion.Euler(-90, 0, 0));
            worms.Add(wormInstantiated);

            wormInstantiated.transform.parent = wormsParent.transform;

            float randomInAnimationTime = Random.Range(WormsInAnimationTime - 1f, WormsInAnimationTime + 1f);
            float randomDerpAnimationTime = Random.Range(WormsDerpAnimationTime - 2f, WormsDerpAnimationTime + 2f);
            float randomAnimationSpeed = Random.Range(WormsAnimationSpeed - 50f, WormsAnimationSpeed + 50f);

            wormInstantiated.GetComponent<InfiniteWormsAnimation>().HandleWormAnimationFunction(
                randomInAnimationTime, randomDerpAnimationTime, randomAnimationSpeed);

            _spawnedWormsPositions.Add(holePosition);
        }
    }

    public void RemoveWorms()
    {
        isSpawning = false;

        foreach (GameObject w in worms)
        {
            Destroy(w);
        }

        worms.Clear();

        _spawnedWormsPositions.Clear();
        _spawnedWormsPositions = new List<Vector3>();
    }

    private Vector3 GetRandomHoleNearFruit()
    {
        Vector3 fruitPosition = InfiniteGameController.instance.GetFruitLocalPosition();
        int tries = 0;
        do
        {
            if (InfiniteHolesController.instance != null)
            {
                List<Vector3> holePositions = InfiniteHolesController.instance.GetRandomSpawnedPositions();

                foreach (Vector3 spawnedPosition in holePositions)
                {
                    Vector2 tmpFruitPos = fruitPosition;
                    Vector2 tmpSpawnedPos = spawnedPosition;
                    float distance = Vector2.Distance(tmpFruitPos, tmpSpawnedPos);
                    //print($"Hole Position (from position finder) : {spawnedPosition}");
                    //print($"Distance: {distance}");

                    if (distance <= WormsMaxDistance && tmpSpawnedPos.y > fruitPosition.y && distance >= WormsMinDistance)
                    {
                        //spawnPosition = wormsParent.transform.TransformPoint(spawnedPosition);
                        spawnPosition = tmpSpawnedPos;
                    }
                }
            }

            tries++;
            if (tries >= maxTries)
            {
                spawnPosition = Vector3.zero;
                break;
            }
        } while (!IsValidPosition(spawnPosition));
        return spawnPosition;
    }

    private bool IsValidPosition(Vector3 position)
    {
        foreach (Vector3 spawnedPosition in _spawnedWormsPositions)
        {
            if (Vector3.Distance(position, spawnedPosition) < WormsMinDistance)
            {
                return false;
            }
        }
        return true;
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
    public float WormsMinDistance
    {
        get { return _wormsMinDistance[InfiniteGameController.instance.difficultyLevel - 1]; }
        set
        {
            _wormsMinDistance[InfiniteGameController.instance.difficultyLevel - 1] = value;
            SaveWormsMinDistance();
            SpawnWorms();
        }
    }

    private void SaveWormsMinDistance()
    {
        for (int i = 0; i < _wormsMinDistance.Length; i++)
        {
            PlayerPrefs.SetFloat("WormsMinDistance_Level_" + (i + 1), _wormsMinDistance[i]);
        }
    }

    private void LoadWormsMinDistance()
    {
        for (int i = 0; i < _wormsMinDistance.Length; i++)
        {
            _wormsMinDistance[i] = PlayerPrefs.GetFloat("WormsMinDistance_Level_" + (i + 1), 50);
        }
    }

    //MAX DISTANCE
    public float WormsMaxDistance
    {
        get { return _wormsMaxDistance[InfiniteGameController.instance.difficultyLevel - 1]; }
        set
        {
            _wormsMaxDistance[InfiniteGameController.instance.difficultyLevel - 1] = value;
            SaveWormsMaxDistance();
            SpawnWorms();
        }
    }

    private void SaveWormsMaxDistance()
    {
        for (int i = 0; i < _wormsMaxDistance.Length; i++)
        {
            PlayerPrefs.SetFloat("WormsMaxDistance_Level_" + (i + 1), _wormsMaxDistance[i]);
        }
    }

    private void LoadWormsMaxDistance()
    {
        for (int i = 0; i < _wormsMaxDistance.Length; i++)
        {
            _wormsMaxDistance[i] = PlayerPrefs.GetFloat("WormsMaxDistance_Level_" + (i + 1), 50);
        }
    }

    //MIN SPAWNING INTERVAL
    public float WormsMinSpawningInterval
    {
        get { return _wormsMinSpawningInterval[InfiniteGameController.instance.difficultyLevel - 1]; }
        set
        {
            _wormsMinSpawningInterval[InfiniteGameController.instance.difficultyLevel - 1] = value;
            SaveWormsMinSpawningInterval();
            SpawnWorms();
        }
    }

    private void SaveWormsMinSpawningInterval()
    {
        for (int i = 0; i < _wormsMinSpawningInterval.Length; i++)
        {
            PlayerPrefs.SetFloat("WormsMinSpawningInterval_Level_" + (i + 1), _wormsMinSpawningInterval[i]);
        }
    }

    private void LoadWormsMinSpawningInterval()
    {
        for (int i = 0; i < _wormsMinSpawningInterval.Length; i++)
        {
            _wormsMinSpawningInterval[i] = PlayerPrefs.GetFloat("WormsMinSpawningInterval_Level_" + (i + 1), 50);
        }
    }

    //MAX SPAWNING INTERVAL
    public float WormsMaxSpawningInterval
    {
        get { return _wormsMaxSpawningInterval[InfiniteGameController.instance.difficultyLevel - 1]; }
        set
        {
            _wormsMaxSpawningInterval[InfiniteGameController.instance.difficultyLevel - 1] = value;
            SaveWormsMaxSpawningInterval();
            SpawnWorms();
        }
    }

    private void SaveWormsMaxSpawningInterval()
    {
        for (int i = 0; i < _wormsMaxSpawningInterval.Length; i++)
        {
            PlayerPrefs.SetFloat("WormsMaxSpawningInterval_Level_" + (i + 1), _wormsMaxSpawningInterval[i]);
        }
    }

    private void LoadWormsMaxSpawningInterval()
    {
        for (int i = 0; i < _wormsMaxSpawningInterval.Length; i++)
        {
            _wormsMaxSpawningInterval[i] = PlayerPrefs.GetFloat("WormsMaxSpawningInterval_Level_" + (i + 1), 50);
        }
    }

    //SPAWNING PROBABILITY
    public float WormsSpawnProbability
    {
        get { return _wormsSpawnProbability[InfiniteGameController.instance.difficultyLevel - 1]; }
        set
        {
            _wormsSpawnProbability[InfiniteGameController.instance.difficultyLevel - 1] = value;
            SaveWormsSpawnProbability();
            SpawnWorms();
        }
    }

    private void SaveWormsSpawnProbability()
    {
        for (int i = 0; i < _wormsSpawnProbability.Length; i++)
        {
            PlayerPrefs.SetFloat("WormsSpawnProbability_Level_" + (i + 1), _wormsSpawnProbability[i]);
        }
    }

    private void LoadWormsSpawnProbability()
    {
        for (int i = 0; i < _wormsSpawnProbability.Length; i++)
        {
            _wormsSpawnProbability[i] = PlayerPrefs.GetFloat("WormsSpawnProbability_Level_" + (i + 1), 50);
        }
    }

    //IN ANIMATION TIME
    public float WormsInAnimationTime
    {
        get { return _wormsInAnimationTime[InfiniteGameController.instance.difficultyLevel - 1]; }
        set
        {
            _wormsInAnimationTime[InfiniteGameController.instance.difficultyLevel - 1] = value;
            SaveWormsInAnimationTime();
            SpawnWorms();
        }
    }

    private void SaveWormsInAnimationTime()
    {
        for (int i = 0; i < _wormsInAnimationTime.Length; i++)
        {
            PlayerPrefs.SetFloat("WormsInAnimationTime_Level_" + (i + 1), _wormsInAnimationTime[i]);
        }
    }

    private void LoadWormsInAnimationTime()
    {
        for (int i = 0; i < _wormsInAnimationTime.Length; i++)
        {
            _wormsInAnimationTime[i] = PlayerPrefs.GetFloat("WormsInAnimationTime_Level_" + (i + 1), 50);
        }
    }

    //DERP ANIMATION TIME
    public float WormsDerpAnimationTime
    {
        get { return _wormsDerpAnimationTime[InfiniteGameController.instance.difficultyLevel - 1]; }
        set
        {
            _wormsDerpAnimationTime[InfiniteGameController.instance.difficultyLevel - 1] = value;
            SaveWormsDerpAnimationTime();
            SpawnWorms();
        }
    }

    private void SaveWormsDerpAnimationTime()
    {
        for (int i = 0; i < _wormsDerpAnimationTime.Length; i++)
        {
            PlayerPrefs.SetFloat("WormsDerpAnimationTime_Level_" + (i + 1), _wormsDerpAnimationTime[i]);
        }
    }

    private void LoadWormsDerpAnimationTime()
    {
        for (int i = 0; i < _wormsDerpAnimationTime.Length; i++)
        {
            _wormsDerpAnimationTime[i] = PlayerPrefs.GetFloat("WormsDerpAnimationTime_Level_" + (i + 1), 50);
        }
    }

    //ANIMATION SPEED
    public float WormsAnimationSpeed
    {
        get { return _wormsAnimationSpeed[InfiniteGameController.instance.difficultyLevel - 1]; }
        set
        {
            _wormsAnimationSpeed[InfiniteGameController.instance.difficultyLevel - 1] = value;
            SaveWormsAnimationSpeed();
            SpawnWorms();
        }
    }

    private void SaveWormsAnimationSpeed()
    {
        for (int i = 0; i < _wormsAnimationSpeed.Length; i++)
        {
            PlayerPrefs.SetFloat("WormsAnimationSpeed_Level_" + (i + 1), _wormsAnimationSpeed[i]);
        }
    }

    private void LoadWormsAnimationSpeed()
    {
        for (int i = 0; i < _wormsAnimationSpeed.Length; i++)
        {
            _wormsAnimationSpeed[i] = PlayerPrefs.GetFloat("WormsAnimationSpeed_Level_" + (i + 1), 50);
        }
    }
}
