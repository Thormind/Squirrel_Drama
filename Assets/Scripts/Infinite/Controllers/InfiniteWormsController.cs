using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteWormsController : MonoBehaviour
{
    public static InfiniteWormsController instance;

    public GameObject wormsParent;
    public GameObject wormPrefab;

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

    private void FixedUpdate()
    {
        if (isSpawning && Time.time >= _nextSpawnTime && Random.value < WormsSpawnProbability)
        {
            SpawnWorm();
            _nextSpawnTime = Time.time + Random.Range(WormsMinSpawningInterval, WormsMaxSpawningInterval);
        }
    }

    public void SpawnWorms()
    {
        QuickRemoveWorms();
        isSpawning = true;
    }

    public void SpawnWorm()
    {
        Vector3 holePosition = GetRandomHoleNearFruit();

        if (holePosition == Vector3.zero)
        {
            NotifySpawnDebug(false);
        }
        else
        {
            NotifySpawnDebug(true);
            holePosition = new Vector3(holePosition.x, holePosition.y, 1f);

            Vector3 worldPosition = wormsParent.transform.TransformPoint(holePosition);

            InstantiateAnimation(worldPosition, holePosition, true);

            _spawnedWormsPositions.Add(holePosition);

        }
    }

    public void QuickRemoveWorms()
    {
        isSpawning = false;

        for (int i = 0; i < wormsParent.transform.childCount; i++)
        {
            GameObject child = wormsParent.transform.GetChild(i).gameObject;
            Destroy(child);
        }

        _spawnedWormsPositions.Clear();
        _spawnedWormsPositions = new List<Vector3>();
    }

    public void RemoveWorms()
    {
        isSpawning = false;

        for (int i = 0; i < wormsParent.transform.childCount; i++)
        {
            GameObject child = wormsParent.transform.GetChild(i).gameObject;
            if (child != null)
            {
                InstantiateAnimation(child.transform.position, child.transform.localPosition, false, child);
            }
        }

        _spawnedWormsPositions.Clear();
        _spawnedWormsPositions = new List<Vector3>();
    }

    public void InstantiateAnimation(Vector3 worldPosition, Vector3 holePosition, bool spawn, GameObject obj = null)
    {
        if (AnimationManager.instance != null)
        {
            AnimationManager.instance.PlayObstaclesAnimation(AnimateInstantiate(worldPosition, holePosition, spawn, obj));
        }

    }

    private IEnumerator AnimateInstantiate(Vector3 worldPosition, Vector3 holePosition, bool spawn,  GameObject obj = null)
    {
        if (spawn)
        {
            GameObject wormInstantiated = Instantiate(wormPrefab, worldPosition, Quaternion.Euler(-90, 0, 0), wormsParent.transform);

            float randomInAnimationTime = Random.Range(WormsInAnimationTime - 0.5f, WormsInAnimationTime + 0.5f);
            float randomDerpAnimationTime = Random.Range(WormsDerpAnimationTime - 1f, WormsDerpAnimationTime + 1f);
            float randomAnimationSpeed = Random.Range(WormsAnimationSpeed - 50f, WormsAnimationSpeed + 50f);

            wormInstantiated.GetComponent<InfiniteWormsAnimation>().HandleWormAnimationFunction(
                randomInAnimationTime, randomDerpAnimationTime, randomAnimationSpeed, holePosition);
        }
        if (!spawn && obj != null)
        {
            Instantiate(InfiniteGameController.instance.obstacleInstanciateVFX, worldPosition, Quaternion.identity, wormsParent.transform);
            Destroy(obj);
            AudioManager.instance.PlaySound(SOUND.OBSTACLE_SPAWN);
        }

        yield return new WaitForSeconds(0.05f);
    }


    private Vector3 GetRandomHoleNearFruit()
    {
        Vector3 spawnPosition = Vector3.zero;
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

    public void RemoveSpawnedPosition(Vector3 position)
    {
        bool removed = _spawnedWormsPositions.Remove(position);
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
