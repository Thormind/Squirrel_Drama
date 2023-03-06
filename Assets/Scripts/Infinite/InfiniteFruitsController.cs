using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InfiniteFruitsController : MonoBehaviour
{

    public static InfiniteFruitsController instance;

    public GameObject fruitsParent;
    public GameObject fruitPrefab;

    public List<GameObject> fruits;
    private List<Vector3> _spawnedFruitsPositions = new List<Vector3>();

    //Difficulty Parameters
    private int[] _fruitsQuantity = new int[9]; //{ 1, 1, 1, 2, 2, 2, 3, 3, 3 };
    private float[] _fruitsMinDistance = new float[9]; // { 1f, 1f, 1f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f } ;

    //Spawning Limits Parameters
    private float xMin = -2.5f;
    private float xMax = 2.5f;

    private float yMin = 10f;
    private float yMax = 30f;

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

        LoadFruitsQuantity();
        LoadFruitsMinDistance();

        isAllSpawned = false;
    }

    public void SpawnFruits()
    {
        RemoveFruits();

        int randomFruitsQuantity = Random.Range(1, FruitsQuantity + 1);

        for (int i = 0; i < randomFruitsQuantity; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();

            if (spawnPosition == Vector3.zero)
            {
                NotifySpawnDebug(false);
                Debug.Log("Could not find a valid FRUIT spawn position after " + maxTries + " tries.");
                break;
            }

            NotifySpawnDebug(true);

            GameObject fruitInstantiated = Instantiate(fruitPrefab, spawnPosition, Quaternion.identity, fruitsParent.transform);
            fruits.Add(fruitInstantiated);

            _spawnedFruitsPositions.Add(spawnPosition);
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
            spawnPosition = fruitsParent.transform.TransformPoint(localPosition);

            tries++;
            if (tries >= maxTries)
            {
                spawnPosition = Vector3.zero;
                break;
            }
        } while (!IsValidPosition(localPosition));

        if (spawnPosition != Vector3.zero)
        {
            _spawnedFruitsPositions.Add(localPosition);
        }

        return spawnPosition;
    }

    private bool IsValidPosition(Vector3 position)
    {
        foreach (Vector2 spawnedPosition in _spawnedFruitsPositions)
        {
            if (Vector2.Distance(position, spawnedPosition) < FruitsMinDistance)
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
                if (Vector2.Distance(tmpPos, tmpSpawnedPos) < FruitsMinDistance)
                {
                    return false;
                }
            }
        }
        if (InfinitePointsController.instance != null)
        {
            foreach (Vector3 spawnedPosition in InfinitePointsController.instance.GetSpawnedPositions())
            {
                Vector2 tmpPos = position;
                Vector2 tmpSpawnedPos = spawnedPosition;
                if (Vector2.Distance(tmpPos, tmpSpawnedPos) < FruitsMinDistance)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public void RemoveFruits()
    {

        foreach (GameObject g in fruits)
        {
            Destroy(g);
        }

        fruits.Clear();

        _spawnedFruitsPositions.Clear();
        _spawnedFruitsPositions = new List<Vector3>();
    }

    private void NotifySpawnDebug(bool spawned)
    {
        isAllSpawned = spawned;

        if (LevelScalingController.instance != null)
        {
            LevelScalingController.instance.UpdateObstacleMessages();
        }
    }

    //FRUITS QUANTITY
    public int FruitsQuantity
    {
        get { return _fruitsQuantity[InfiniteGameController.instance.difficultyLevel - 1]; }
        set
        {
            _fruitsQuantity[InfiniteGameController.instance.difficultyLevel - 1] = value;
            SaveFruitsQuantity();
            SpawnFruits();
        }
    }

    private void SaveFruitsQuantity()
    {
        for (int i = 0; i < _fruitsQuantity.Length; i++)
        {
            PlayerPrefs.SetInt("FruitsQuantity_Level_" + (i + 1), _fruitsQuantity[i]);
        }
    }

    private void LoadFruitsQuantity()
    {
        for (int i = 0; i < _fruitsQuantity.Length; i++)
        {
            _fruitsQuantity[i] = PlayerPrefs.GetInt("FruitsQuantity_Level_" + (i + 1), 50);
        }
    }

    //MIN DISTANCE
    public float FruitsMinDistance
    {
        get { return _fruitsMinDistance[InfiniteGameController.instance.difficultyLevel - 1]; }
        set
        {
            _fruitsMinDistance[InfiniteGameController.instance.difficultyLevel - 1] = value;
            SaveFruitsMinDistance();
            SpawnFruits();
        }
    }

    private void SaveFruitsMinDistance()
    {
        for (int i = 0; i < _fruitsMinDistance.Length; i++)
        {
            PlayerPrefs.SetFloat("FruitsMinDistance_Level_" + (i + 1), _fruitsMinDistance[i]);
        }
    }

    private void LoadFruitsMinDistance()
    {
        for (int i = 0; i < _fruitsMinDistance.Length; i++)
        {
            _fruitsMinDistance[i] = PlayerPrefs.GetFloat("FruitsMinDistance_Level_" + (i + 1), 50);
        }
    }

}
