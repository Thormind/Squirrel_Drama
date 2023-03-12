using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InfiniteHolesController : MonoBehaviour
{
    public static InfiniteHolesController instance;

    public GameObject holesParent;
    public GameObject holePrefab;

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
        QuickRemoveHoles();

        for (int i = 0; i < HolesQuantity; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();

            if (spawnPosition == Vector3.zero)
            {
                NotifySpawnDebug(false);
                break;
            }

            NotifySpawnDebug(true);

            InstantiateAnimation(spawnPosition, true);
        }
    }

    
    public void RemoveHoles()
    {
        for (int i = 0; i < holesParent.transform.childCount; i++)
        {
            GameObject child = holesParent.transform.GetChild(i).gameObject;
            if (child != null)
            {
                InstantiateAnimation(child.transform.position, false, child);
            }
        }

        _spawnedHolesPositions.Clear();
        _spawnedHolesPositions = new List<Vector3>();
    }

    public void QuickRemoveHoles()
    {
        for (int i = 0; i < holesParent.transform.childCount; i++)
        {
            GameObject child = holesParent.transform.GetChild(i).gameObject;
            Destroy(child);
        }

        _spawnedHolesPositions.Clear();
        _spawnedHolesPositions = new List<Vector3>();
    }

    public void InstantiateAnimation(Vector3 position, bool spawn, GameObject obj = null)
    {
        if (AnimationManager.instance != null)
        {
            AnimationManager.instance.PlayObstaclesAnimation(AnimateInstantiate(position, spawn, obj));
        }
    }

    private IEnumerator AnimateInstantiate(Vector3 position, bool spawn, GameObject obj = null)
    {
        //play sound 

        Instantiate(InfiniteGameController.instance.obstacleInstanciateVFX, position, Quaternion.identity, holesParent.transform);

        if (spawn)
        {
            GameObject holeInstantiated = Instantiate(holePrefab, position, Quaternion.identity, holesParent.transform);
        }
        if (!spawn && obj != null)
        {
            Destroy(obj);
        }

        yield return new WaitForSeconds(0.025f);
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
