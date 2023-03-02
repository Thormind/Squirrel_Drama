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
    private int[] _beesQuantity = new int[9]; //{ 5, 5, 10, 10, 20, 20, 30, 30, 40 } ;
    private float[] _beesMinDistance = new float[9]; //{ 5f, 4f, 4f, 3f, 3f, 2f, 2f, 1f, 1f };
    private float[] _beesMovementSpeed = new float[9]; //{ 0.3f, 0.35f, 0.40f, 0.45f, 0.55f, 0.65f, 0.75f, 0.85f, 1f } ;

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

        LoadBeesQuantity();
        LoadBeesMinDistance();
        LoadBeesMovementSpeed();

        isAllSpawned = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.R))
        {
            SpawnBees();
        }
    }

    public void SpawnBees()
    {
        RemoveBees();

        maxBeesYDistance = ((Mathf.Abs(beesYMin - beesYMax)) / BeesQuantity) + 0.2f;

        currentY = beesYMin;

        int randomBeesQuantity = Random.Range(BeesQuantity, BeesQuantity + 2);

        for (int i = 1; i <= randomBeesQuantity; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();

            if (spawnPosition == Vector3.zero)
            {
                NotifySpawnDebug(false);
                Debug.Log("Could not find a valid BEE spawn position after " + maxTries + " tries.");
                break;
            }

            NotifySpawnDebug(true);

            Vector3 realPosition = beesParent.transform.TransformPoint(spawnPosition);

            GameObject beeInstantiated = Instantiate(beePrefab, realPosition, Quaternion.identity);

            float randomMovementSpeed = Random.Range(BeesMovementSpeed - 0.1f, BeesMovementSpeed + 0.1f);
            beeInstantiated.GetComponent<InfiniteBeeAnimation>().SetMovementSpeed(randomMovementSpeed);

            beeInstantiated.transform.parent = beesParent.transform;
            _spawnedBeesPositions.Add(spawnPosition);
        }
    }

    public void RemoveBees()
    {
        for (int i = 0; i < beesParent.transform.childCount; i++)
        {
            GameObject child = beesParent.transform.GetChild(i).gameObject;
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
        get { return _beesQuantity[InfiniteGameController.instance.difficultyLevel - 1]; }
        set
        {
            _beesQuantity[InfiniteGameController.instance.difficultyLevel - 1] = value;
            SaveBeesQuantity();
            SpawnBees();
        }
    }

    private void SaveBeesQuantity()
    {
        for (int i = 0; i < _beesQuantity.Length; i++)
        {
            PlayerPrefs.SetInt("BeesQuantity_Level_" + (i + 1), _beesQuantity[i]);
        }
    }

    private void LoadBeesQuantity()
    {
        for (int i = 0; i < _beesQuantity.Length; i++)
        {
            _beesQuantity[i] = PlayerPrefs.GetInt("BeesQuantity_Level_" + (i + 1), 50);
        }
    }

    //MIN DISTANCE
    public float BeesMinDistance
    {
        get { return _beesMinDistance[InfiniteGameController.instance.difficultyLevel - 1]; }
        set
        {
            _beesMinDistance[InfiniteGameController.instance.difficultyLevel - 1] = value;
            SaveBeesMinDistance();
            SpawnBees();
        }
    }

    private void SaveBeesMinDistance()
    {
        for (int i = 0; i < _beesMinDistance.Length; i++)
        {
            PlayerPrefs.SetFloat("BeesMinDistance_Level_" + (i + 1), _beesMinDistance[i]);
        }
    }

    private void LoadBeesMinDistance()
    {
        for (int i = 0; i < _beesMinDistance.Length; i++)
        {
            _beesMinDistance[i] = PlayerPrefs.GetFloat("BeesMinDistance_Level_" + (i + 1), 50);
        }
    }

    //MOVEMENT SPEED
    public float BeesMovementSpeed
    {
        get { return _beesMovementSpeed[InfiniteGameController.instance.difficultyLevel - 1]; }
        set
        {
            _beesMovementSpeed[InfiniteGameController.instance.difficultyLevel - 1] = value;
            SaveBeesMovementSpeed();
            SpawnBees();
        }
    }

    private void SaveBeesMovementSpeed()
    {
        for (int i = 0; i < _beesMovementSpeed.Length; i++)
        {
            PlayerPrefs.SetFloat("BeesMovementSpeed_Level_" + (i + 1), _beesMovementSpeed[i]);
        }
    }

    private void LoadBeesMovementSpeed()
    {
        for (int i = 0; i < _beesMovementSpeed.Length; i++)
        {
            _beesMovementSpeed[i] = PlayerPrefs.GetFloat("BeesMovementSpeed_Level_" + (i + 1), 50);
        }
    }

}
