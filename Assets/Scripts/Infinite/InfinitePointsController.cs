using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InfinitePointsController : MonoBehaviour
{

    public static InfinitePointsController instance;

    public List<GameObject> points;

    public GameObject pointsParent;

    public GameObject pointPrefab;

    private int pointsQuantity = 50;
    private int maxTries = 100;
    private float minDistance = 10f;

    private float xMin = -2.5f;
    private float xMax = 2.5f;

    private float yMin = 4f;
    private float yMax = 38f;


    private List<Vector3> _spawnedPointsPositions = new List<Vector3>();

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
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.G))
        {
            SpawnPoints();
        }

    }

    public void SpawnPoints()
    {
        RemovePoints();

        for (int i = 0; i < pointsQuantity; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            if (spawnPosition == Vector3.zero)
            {
                Debug.Log("Could not find a valid spawn position after " + maxTries + " tries.");
                break;
            }
            GameObject pointInstantiated = Instantiate(pointPrefab, spawnPosition, Quaternion.identity);
            points.Add(pointInstantiated);
            pointInstantiated.transform.parent = pointsParent.transform;

            _spawnedPointsPositions.Add(spawnPosition);
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 spawnPosition;
        int tries = 0;
        do
        {
            float x = Random.Range(xMin, xMax);
            float y = Random.Range(yMin, yMax);
            spawnPosition = pointsParent.transform.TransformPoint(new Vector3(x, y, -0.25f));

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
        foreach (Vector3 spawnedPosition in _spawnedPointsPositions)
        {
            if (Vector3.Distance(position, spawnedPosition) < minDistance)
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
                if (Vector3.Distance(tmpPos, tmpSpawnedPos) < minDistance)
                {
                    return false;
                }
            }
        }
        if (InfiniteBeesController.instance != null)
        {
            foreach (Vector3 spawnedPosition in InfiniteBeesController.instance.GetSpawnedPositions())
            {
                Vector2 tmpPos = position;
                Vector2 tmpSpawnedPos = spawnedPosition;
                if (Vector3.Distance(tmpPos, tmpSpawnedPos) < minDistance)
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
    }

    public List<Vector3> GetSpawnedPositions()
    {
        return _spawnedPointsPositions;
    }

}
