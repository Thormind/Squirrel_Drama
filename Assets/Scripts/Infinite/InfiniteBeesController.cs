using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteBeesController : MonoBehaviour
{
    public static InfiniteBeesController instance;

    public GameObject beesParent;

    public GameObject beePrefab;

    private int beesQuantity = 30;
    private int maxTries = 100;

    public float beesXMin = -3.15f;
    public float beesXMax = 3.15f;

    private float beesYMin = 2f;
    private float beesYMax = 38f;

    private float minBeesYDistance = 2f;
    private float maxBeesYDistance = 3f;

    private float currentY;

    private List<Vector3> _spawnedBeesPositions = new List<Vector3>();


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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            SpawnBees();
        }
    }

    public void SpawnBees()
    {
        RemoveBees();

        maxBeesYDistance = ((Mathf.Abs(beesYMin - beesYMax)) / beesQuantity) + 0.2f;

        currentY = beesYMin;

        for (int i = 1; i <= beesQuantity; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();

            if (spawnPosition == Vector3.zero)
            {
                Debug.Log("Could not find a valid spawn position after " + maxTries + " tries.");
                break;
            }

            Vector3 realPosition = beesParent.transform.TransformPoint(spawnPosition);

            GameObject beeInstantiated = Instantiate(beePrefab, realPosition, Quaternion.identity);
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
            float randomY = Random.Range(minBeesYDistance, maxBeesYDistance);
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

    public List<Vector3> GetSpawnedPositions()
    {
        return _spawnedBeesPositions;
    }

}
