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
    [SerializeField] private WormsParametersSO _wormsParameters;


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
            GameObject wormInstantiated = Instantiate(wormPrefab, worldPosition, Quaternion.Euler(-90, 0, 0), wormsParent.transform);
            worms.Add(wormInstantiated);

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

                    if (distance <= WormsMaxDistance && tmpSpawnedPos.y > fruitPosition.y + WormsMinDistance && distance >= WormsMinDistance)
                    {
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
        get { return _wormsParameters.GetMinDistanceForLevel(InfiniteGameController.instance.difficultyLevel); }
        set
        {
            _wormsParameters.SetMinDistanceForLevel(InfiniteGameController.instance.difficultyLevel, value);
            SpawnWorms();
        }
    }


    //MAX DISTANCE
    public float WormsMaxDistance
    {
        get { return _wormsParameters.GetMaxDistanceForLevel(InfiniteGameController.instance.difficultyLevel); }
        set
        {
            _wormsParameters.SetMaxDistanceForLevel(InfiniteGameController.instance.difficultyLevel, value);
            SpawnWorms();
        }
    }


    //MIN SPAWNING INTERVAL
    public float WormsMinSpawningInterval
    {
        get { return _wormsParameters.GetMinSpawningIntervalForLevel(InfiniteGameController.instance.difficultyLevel); }
        set
        {
            _wormsParameters.SetMinSpawningIntervalForLevel(InfiniteGameController.instance.difficultyLevel, value);
            SpawnWorms();
        }
    }


    //MAX SPAWNING INTERVAL
    public float WormsMaxSpawningInterval
    {
        get { return _wormsParameters.GetMaxSpawningIntervalForLevel(InfiniteGameController.instance.difficultyLevel); }
        set
        {
            _wormsParameters.SetMaxSpawningIntervalForLevel(InfiniteGameController.instance.difficultyLevel, value);
            SpawnWorms();
        }
    }


    //SPAWNING PROBABILITY
    public float WormsSpawnProbability
    {
        get { return _wormsParameters.GetSpawnProbabilityForLevel(InfiniteGameController.instance.difficultyLevel); }
        set
        {
            _wormsParameters.SetSpawnProbabilityForLevel(InfiniteGameController.instance.difficultyLevel, value);
            SpawnWorms();
        }
    }


    //IN ANIMATION TIME
    public float WormsInAnimationTime
    {
        get { return _wormsParameters.GetInAnimationTimeForLevel(InfiniteGameController.instance.difficultyLevel); }
        set
        {
            _wormsParameters.SetInAnimationTimeForLevel(InfiniteGameController.instance.difficultyLevel, value);
            SpawnWorms();
        }
    }

    //DERP ANIMATION TIME
    public float WormsDerpAnimationTime
    {
        get { return _wormsParameters.GetDerpAnimationTimeForLevel(InfiniteGameController.instance.difficultyLevel); }
        set
        {
            _wormsParameters.SetDerpAnimationTimeForLevel(InfiniteGameController.instance.difficultyLevel, value);
            SpawnWorms();
        }
    }

    //ANIMATION SPEED
    public float WormsAnimationSpeed
    {
        get { return _wormsParameters.GetAnimationSpeedForLevel(InfiniteGameController.instance.difficultyLevel); }
        set
        {
            _wormsParameters.SetAnimationSpeedForLevel(InfiniteGameController.instance.difficultyLevel, value);
            SpawnWorms();
        }
    }

}
