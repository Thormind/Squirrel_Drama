using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleSpawner : MonoBehaviour
{
    public static HoleSpawner instance;

    public GameObject holePrefab;

    public int holesQuantity = 50;
    public int maxTries = 100;
    public float minDistance = 2f;
    public float maxDistance = 6f;


    private List<Vector3> _spawnedHolesPositions = new List<Vector3>();

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

    public void SpawnHoles()
    {
        for (int i = 0; i < holesQuantity; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            if (spawnPosition == Vector3.zero)
            {
                Debug.Log("Could not find a valid spawn position after " + maxTries + " tries.");
                break;
            }
            Instantiate(holePrefab, spawnPosition, Quaternion.identity);
            _spawnedHolesPositions.Add(spawnPosition);
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 spawnPosition;
        int tries = 0;
        do
        {
            float x = Random.Range(-10f, 10f);
            float y = Random.Range(5f, 50f);
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
        foreach (Vector3 spawnedPosition in _spawnedHolesPositions)
        {
            if (Vector3.Distance(position, spawnedPosition) < minDistance)
            {
                return false;
            }
        }
        return true;
    }

    public void RemoveHoles()
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacles");
        foreach (GameObject obstacle in obstacles)
        {
            Destroy(obstacle);
        }
        _spawnedHolesPositions.Clear();
    }
}
