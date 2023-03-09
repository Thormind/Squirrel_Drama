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
    [SerializeField] private HolesParametersSO _holesParameters;

    //Spawning Limits Parameters
    private float xMin = -2.75f;
    private float xMax = 2.75f;

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

        isAllSpawned = false;
    }

    public void SpawnHoles()
    {
        RemoveHoles();

        //int randomHolesQuantity = Random.Range(HolesQuantity-5, HolesQuantity+5);

        for (int i = 0; i < HolesQuantity; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();

            if (spawnPosition == Vector3.zero)
            {
                NotifySpawnDebug(false);
                Debug.Log("Could not find a valid HOLE spawn position after " + maxTries + " tries.");
                break;
            }

            NotifySpawnDebug(true);

            GameObject holeInstantiated = Instantiate(holePrefab, spawnPosition, Quaternion.identity, holesParent.transform);
            holes.Add(holeInstantiated);
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
        get { return _holesParameters.GetQuantityForLevel(InfiniteGameController.instance.difficultyLevel); }
        set
        {
            _holesParameters.SetQuantityForLevel(InfiniteGameController.instance.difficultyLevel, value);
            SpawnHoles();
        }
    }

    //MIN DISTANCE
    public float HolesMinDistance
    {
        get { return _holesParameters.GetMinDistanceForLevel(InfiniteGameController.instance.difficultyLevel); }
        set
        {
            _holesParameters.SetMinDistanceForLevel(InfiniteGameController.instance.difficultyLevel, value);
            SpawnHoles();
        }
    }
}
