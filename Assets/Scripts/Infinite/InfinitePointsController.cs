using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InfinitePointsController : MonoBehaviour
{

    public static InfinitePointsController instance;

    public GameObject pointsParent;
    public GameObject pointPrefab;

    public List<GameObject> points;
    private List<Vector3> _spawnedPointsPositions = new List<Vector3>();

    //Difficulty Parameters
    private int[] _pointsQuantity = new int[9]; // { 10, 15, 20, 25, 30, 35, 40, 50, 60 };
    private float[] _pointsMinDistance = new float[9]; // { 1.5f, 1f, 1f, 1f, 1f, 1f, 0.5f, 0.5f, 0.5f };

    //Spawning Limits Parameters
    private float xMin = -2.5f;
    private float xMax = 2.5f;

    private float yMin = 4f;
    private float yMax = 38f;


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

        LoadPointsQuantity();
        LoadPointsMinDistance();

        isAllSpawned = false;
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.G) || Input.GetKeyDown(KeyCode.R))
        {
            SpawnPoints();
        }

    }

    public void SpawnPoints()
    {
        RemovePoints();

        int randomPointsQuantity = Random.Range(PointsQuantity - 5, PointsQuantity + 5);

        for (int i = 0; i < randomPointsQuantity; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();

            if (spawnPosition == Vector3.zero)
            {
                NotifySpawnDebug(false);
                Debug.Log("Could not find a valid POINTS spawn position after " + maxTries + " tries.");
                break;
            }

            NotifySpawnDebug(true);

            GameObject pointInstantiated = Instantiate(pointPrefab, spawnPosition, Quaternion.identity);
            points.Add(pointInstantiated);
            pointInstantiated.transform.parent = pointsParent.transform;

            _spawnedPointsPositions.Add(spawnPosition);
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 spawnPosition;
        Vector3 localPosition;
        int tries = 0;
        do
        {
            float x = Random.Range(xMin, xMax);
            float y = Random.Range(yMin, yMax);

            localPosition = new Vector3(x, y, -0.25f);
            spawnPosition = pointsParent.transform.TransformPoint(localPosition);

            tries++;
            if (tries >= maxTries)
            {
                spawnPosition = Vector3.zero;
                break;
            }
        } while (!IsValidPosition(localPosition));

        if (spawnPosition != Vector3.zero)
        {
            _spawnedPointsPositions.Add(localPosition);
        }

        return spawnPosition;
    }

    private bool IsValidPosition(Vector3 position)
    {
        foreach (Vector3 spawnedPosition in _spawnedPointsPositions)
        {
            if (Vector3.Distance(position, spawnedPosition) < PointsMinDistance)
            {
                return false;
            }
        }
        if (InfiniteHolesController.instance != null)
        {
            foreach (Vector3 spawnedPosition in InfiniteHolesController.instance.GetSpawnedPositions())
            {
                Vector2 tmpPos = position;
                Vector2 tmpSpawnedPos = spawnedPosition;
                if (Vector3.Distance(tmpPos, tmpSpawnedPos) < PointsMinDistance)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public void RemovePoints()
    {

        foreach (GameObject g in points)
        {
            Destroy(g);
        }

        points.Clear();

        _spawnedPointsPositions.Clear();
        _spawnedPointsPositions = new List<Vector3>();
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
        return _spawnedPointsPositions;
    }

    //POINTS QUANTITY
    public int PointsQuantity
    {
        get { return _pointsQuantity[InfiniteGameController.instance.difficultyLevel - 1]; }
        set
        {
            _pointsQuantity[InfiniteGameController.instance.difficultyLevel - 1] = value;
            SavePointsQuantity();
            SpawnPoints();
        }
    }

    private void SavePointsQuantity()
    {
        for (int i = 0; i < _pointsQuantity.Length; i++)
        {
            PlayerPrefs.SetInt("PointsQuantity_Level_" + (i + 1), _pointsQuantity[i]);
        }
    }

    private void LoadPointsQuantity()
    {
        for (int i = 0; i < _pointsQuantity.Length; i++)
        {
            _pointsQuantity[i] = PlayerPrefs.GetInt("PointsQuantity_Level_" + (i + 1), 50);
        }
    }

    //MIN DISTANCE
    public float PointsMinDistance
    {
        get { return _pointsMinDistance[InfiniteGameController.instance.difficultyLevel - 1]; }
        set
        {
            _pointsMinDistance[InfiniteGameController.instance.difficultyLevel - 1] = value;
            SavePointsMinDistance();
            SpawnPoints();
        }
    }

    private void SavePointsMinDistance()
    {
        for (int i = 0; i < _pointsMinDistance.Length; i++)
        {
            PlayerPrefs.SetFloat("PointsMinDistance_Level_" + (i + 1), _pointsMinDistance[i]);
        }
    }

    private void LoadPointsMinDistance()
    {
        for (int i = 0; i < _pointsMinDistance.Length; i++)
        {
            _pointsMinDistance[i] = PlayerPrefs.GetFloat("PointsMinDistance_Level_" + (i + 1), 50);
        }
    }

}
