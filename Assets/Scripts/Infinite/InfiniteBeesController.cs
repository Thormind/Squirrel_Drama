using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteBeesController : MonoBehaviour
{
    public static InfiniteBeesController instance;

    public GameObject beesParent;
    public GameObject beePrefab;

    private List<Vector3> _spawnedBeesPositions = new List<Vector3>();

    //Difficulty Parameters
    private float maxBeesYDistance;
    [SerializeField] private BeesParametersSO _beesParameters;

    //Spawning Limits Parameters
    public float beesXMin = -3.15f;
    public float beesXMax = 3.15f;

    private float beesYMin = 2f;
    private float beesYMax = 38f;

    private float currentY;
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

    public void SpawnBees()
    {
        RemoveBees();

        maxBeesYDistance = ((Mathf.Abs(beesYMin - beesYMax)) / BeesQuantity) + 0.2f;

        currentY = beesYMin;

        //int randomBeesQuantity = Random.Range(BeesQuantity, BeesQuantity + 2);

        for (int i = 1; i <= BeesQuantity; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();

            if (spawnPosition == Vector3.zero)
            {
                NotifySpawnDebug(false);
                break;
            }

            NotifySpawnDebug(true);

            Vector3 realPosition = beesParent.transform.TransformPoint(spawnPosition);

            InfiniteGameController.instance.ObstacleInstantiateAnimation(spawnPosition);
            GameObject beeInstantiated = Instantiate(beePrefab, realPosition, Quaternion.identity, beesParent.transform);

            float randomMovementSpeed = Random.Range(BeesMovementSpeed - 0.1f, BeesMovementSpeed + 0.1f);
            beeInstantiated.GetComponent<InfiniteBeeAnimation>().SetMovementSpeed(randomMovementSpeed);

            _spawnedBeesPositions.Add(spawnPosition);
        }
    }

    public void RemoveBees()
    {
        for (int i = 0; i < beesParent.transform.childCount; i++)
        {
            GameObject child = beesParent.transform.GetChild(i).gameObject;
            if (child != null)
            {
                InfiniteGameController.instance.ObstacleInstantiateAnimation(child.transform.position);
            }
            Destroy(child);
        }

        _spawnedBeesPositions.Clear();
        _spawnedBeesPositions = new List<Vector3>();
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 spawnPosition;
        int tries = 0;
        do
        {
            float randomX = Random.Range(beesXMin, beesXMax);
            float randomY = Random.Range(BeesMinDistance, maxBeesYDistance);
            currentY += randomY;
            spawnPosition = new Vector3(randomX, currentY, -0.25f);

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
        if (position.y >= beesYMax)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void NotifySpawnDebug(bool spawned)
    {
        isAllSpawned = spawned;

        if (LevelScalingController.instance != null)
        {
            LevelScalingController.instance.UpdateObstacleMessages();
        }
    }

    public List<Vector3> GetSpawnedPositions()
    {
        return _spawnedBeesPositions;
    }

    //BEES QUANTITY
    public int BeesQuantity
    {
        get { return _beesParameters.GetQuantityForLevel(InfiniteGameController.instance.difficultyLevel); }
        set
        {
            _beesParameters.SetQuantityForLevel(InfiniteGameController.instance.difficultyLevel, value);
            SpawnBees();
        }
    }

    //MIN DISTANCE
    public float BeesMinDistance
    {
        get { return _beesParameters.GetMinDistanceForLevel(InfiniteGameController.instance.difficultyLevel); }
        set
        {
            _beesParameters.SetMinDistanceForLevel(InfiniteGameController.instance.difficultyLevel, value);
            SpawnBees();
        }
    }

    //MOVEMENT SPEED
    public float BeesMovementSpeed
    {
        get { return _beesParameters.GetMovementSpeedForLevel(InfiniteGameController.instance.difficultyLevel); }
        set
        {
            _beesParameters.SetMovementSpeedForLevel(InfiniteGameController.instance.difficultyLevel, value);
            SpawnBees();
        }
    }

}
