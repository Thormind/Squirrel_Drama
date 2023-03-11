using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InfinitePointsController : MonoBehaviour
{

    public static InfinitePointsController instance;

    public GameObject pointsParent;
    public GameObject pointPrefab;

    private List<Vector3> _spawnedPointsPositions = new List<Vector3>();

    //Difficulty Parameters
    [SerializeField] private PointsParametersSO _pointsParameters;

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

        isAllSpawned = false;
    }

    public void SpawnPoints()
    {
        QuickRemovePoints();

        for (int i = 0; i < PointsQuantity; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();

            if (spawnPosition == Vector3.zero)
            {
                NotifySpawnDebug(false);
                break;
            }

            NotifySpawnDebug(true);

            InstantiateAnimation(spawnPosition, true);

            _spawnedPointsPositions.Add(spawnPosition);
        }
    }

    public void RemovePoints()
    {

        for (int i = 0; i < pointsParent.transform.childCount; i++)
        {
            GameObject child = pointsParent.transform.GetChild(i).gameObject;
            if (child != null)
            {
                InstantiateAnimation(child.transform.position, false, child);
            }
        }

        _spawnedPointsPositions.Clear();
        _spawnedPointsPositions = new List<Vector3>();
    }

    public void QuickRemovePoints()
    {

        for (int i = 0; i < pointsParent.transform.childCount; i++)
        {
            GameObject child = pointsParent.transform.GetChild(i).gameObject;
            Destroy(child);
        }

        _spawnedPointsPositions.Clear();
        _spawnedPointsPositions = new List<Vector3>();
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

        Instantiate(InfiniteGameController.instance.obstacleInstanciateVFX, position, Quaternion.identity, pointsParent.transform);

        if (spawn)
        {
            GameObject pointInstantiated = Instantiate(pointPrefab, position, Quaternion.identity, pointsParent.transform);
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

            localPosition = new Vector3(x, y, -0.1f);
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
        get { return _pointsParameters.GetQuantityForLevel(InfiniteGameController.instance.difficultyLevel); }
        set
        {
            _pointsParameters.SetQuantityForLevel(InfiniteGameController.instance.difficultyLevel, value);
            SpawnPoints();
        }
    }

    //MIN DISTANCE
    public float PointsMinDistance
    {
        get { return _pointsParameters.GetMinDistanceForLevel(InfiniteGameController.instance.difficultyLevel); }
        set
        {
            _pointsParameters.SetMinDistanceForLevel(InfiniteGameController.instance.difficultyLevel, value);
            SpawnPoints();
        }
    }

}
