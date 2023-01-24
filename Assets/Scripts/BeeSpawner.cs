using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeSpawner : MonoBehaviour
{
    public static BeeSpawner instance;

    public GameObject beePrefab;

    public int beesQuantity = 50;
    public int maxTries = 100;
    public float minDistance = 2f;
    public float maxDistance = 6f;

    public float xMin = -10f;
    public float xMax = 10f;

    public float yMin = 5f;
    public float yMax = 50f;


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

    public void SpawnBees()
    {
        for (int i = 0; i < beesQuantity; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            if (spawnPosition == Vector3.zero)
            {
                Debug.Log("Could not find a valid spawn position after " + maxTries + " tries.");
                break;
            }
            Instantiate(beePrefab, spawnPosition, Quaternion.identity);
            _spawnedBeesPositions.Add(spawnPosition);
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
            spawnPosition = new Vector3(x, y, 0);
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
        foreach (Vector3 spawnedPosition in _spawnedBeesPositions)
        {
            if (Vector3.Distance(position, spawnedPosition) < minDistance)
            {
                return false;
            }
        }
        return true;
    }

    public void RemoveBees()
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacles");
        foreach (GameObject obstacle in obstacles)
        {
            Destroy(obstacle);
        }
        _spawnedBeesPositions.Clear();
    }
}
