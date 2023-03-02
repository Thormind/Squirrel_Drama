using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InfiniteHolesController : MonoBehaviour
{
    public static InfiniteHolesController instance;

    public GameObject holesParent;
    public GameObject holePrefab;

    public List<GameObject> holes;
    private List<Vector3> _spawnedHolesPositions = new List<Vector3>();

    //Difficulty Parameters
    private int[] _holesQuantity = new int[9]; //{ 20, 40, 50, 60, 70, 80, 90, 100, 150} 
    private float[] _holesMinDistance = new float[9]; // { 2f, 2f, 2f, 1f, 1f, 1f, 0.5f, 0.5f, 0.5f } ;

    //Spawning Limits Parameters
    private float xMin = -2.8f;
    private float xMax = 2.8f;

    private float yMin = 2f;
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

        LoadHolesQuantity();
        LoadHolesMinDistance();

        isAllSpawned = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H) || Input.GetKeyDown(KeyCode.R))
        {
            SpawnHoles();
            InfiniteGameController.instance.SetLevel(InfiniteGameController.instance.currentLevel);
        }
    }

    public void SpawnHoles()
    {
        RemoveHoles();

        int randomHolesQuantity = Random.Range(HolesQuantity-5, HolesQuantity+5);

        for (int i = 0; i < randomHolesQuantity; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();

            if (spawnPosition == Vector3.zero)
            {
                NotifySpawnDebug(false);
                Debug.Log("Could not find a valid HOLE spawn position after " + maxTries + " tries.");
                break;
            }

            NotifySpawnDebug(true);

            GameObject holeInstantiated = Instantiate(holePrefab, spawnPosition, Quaternion.identity);
            holes.Add(holeInstantiated);
            holeInstantiated.transform.parent = holesParent.transform;
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

            localPosition = new Vector3(x, y, 0);
            spawnPosition = holesParent.transform.TransformPoint(localPosition);

            tries++;
            if (tries >= maxTries)
            {
                spawnPosition = Vector3.zero;
                break;
            }
        } while (!IsValidPosition(localPosition));

        if (spawnPosition != Vector3.zero)
        {
            _spawnedHolesPositions.Add(localPosition);
        }

        return spawnPosition;
    }

    private bool IsValidPosition(Vector3 position)
    {
        foreach (Vector3 spawnedPosition in _spawnedHolesPositions)
        {
            if (Vector3.Distance(position, spawnedPosition) < HolesMinDistance)
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

    public void RemoveHoles()
    {

        foreach (GameObject g in holes)
        {
            Destroy(g);
        }

        holes.Clear();

        _spawnedHolesPositions.Clear();
        _spawnedHolesPositions = new List<Vector3>();
    }

    public List<Vector3> GetSpawnedPositions()
    {
        return _spawnedHolesPositions;
    }

    public List<Vector3> GetRandomSpawnedPositions()
    {
        List<Vector3> holePositions = _spawnedHolesPositions;

        for (int i = 0; i < holePositions.Count; i++)
        {
            int randomIndex = Random.Range(i, holePositions.Count);
            Vector3 temp = holePositions[i];
            holePositions[i] = holePositions[randomIndex];
            holePositions[randomIndex] = temp;
        }

        return holePositions;
    }

    //HOLES QUANTITY
    public int HolesQuantity
    {
        get { return _holesQuantity[InfiniteGameController.instance.difficultyLevel - 1]; }
        set
        {
            _holesQuantity[InfiniteGameController.instance.difficultyLevel - 1] = value;
            SaveHolesQuantity();
            SpawnHoles();
        }
    }

    private void SaveHolesQuantity()
    {
        for (int i = 0; i < _holesQuantity.Length; i++)
        {
            PlayerPrefs.SetInt("HolesQuantity_Level_" + (i + 1), _holesQuantity[i]);
        }
    }

    private void LoadHolesQuantity()
    {
        for (int i = 0; i < _holesQuantity.Length; i++)
        {
            _holesQuantity[i] = PlayerPrefs.GetInt("HolesQuantity_Level_" + (i + 1), 50);
        }
    }

    //MIN DISTANCE
    public float HolesMinDistance
    {
        get { return _holesMinDistance[InfiniteGameController.instance.difficultyLevel - 1]; }
        set
        {
            _holesMinDistance[InfiniteGameController.instance.difficultyLevel - 1] = value;
            SaveHolesMinDistance();
            SpawnHoles();
        }
    }

    private void SaveHolesMinDistance()
    {
        for (int i = 0; i < _holesMinDistance.Length; i++)
        {
            PlayerPrefs.SetFloat("HolesMinDistance_Level_" + (i + 1), _holesMinDistance[i]);
        }
    }

    private void LoadHolesMinDistance()
    {
        for (int i = 0; i < _holesMinDistance.Length; i++)
        {
            _holesMinDistance[i] = PlayerPrefs.GetFloat("HolesMinDistance_Level_" + (i + 1), 50);
        }
    }

}
