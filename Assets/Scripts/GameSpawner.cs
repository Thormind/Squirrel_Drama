using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSpawner : MonoBehaviour
{
    public static GameSpawner instance;

    public GameObject beesPrefab;
    public GameObject fliesPrefab;

    public float xMin = -10f;
    public float xMax = 10f;
    public float yMin = 25f;
    public float yMax = 45f;

    [SerializeField] public int beesQuantity = 1; // Number of bees to instantiate
    [SerializeField] public int fliesQuantity = 1; // Number of flies to instantiate

    [SerializeField] public float minBeesDistance = 2f;  // Minimum distance between each instantiated obstacles on the Y axis
    [SerializeField] public float minFliesDistance = 2f;  // Minimum distance between each instantiated obstacles on the Y axis

    private List<float> spawnedBeesYPositions = new List<float>(); // List to keep track of already spawned bees Y positions
    private List<float> spawnedFliesYPositions = new List<float>(); // List to keep track of already spawned bees Y positions

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

    void Start()
    {
        SpawnObstacles();
    }

    public void SpawnObstacles()
    {
        SpawnBees();
        SpawnFlies();
    }

    private void SpawnBees()
    {
        for (int i = 0; i < beesQuantity; i++)
        {
            float xPos = Random.Range(xMin, xMax);
            float yPos = Random.Range(yMin, yMax);

            // Keep generating a new Y position until a valid one is found
            while (spawnedBeesYPositions.Contains(yPos) || IsBeesTooClose(yPos))
            {
                yPos = Random.Range(yMin, yMax);
            }

            Vector3 spawnPos = new Vector3(xPos, yPos, 0f);
            Instantiate(beesPrefab, spawnPos, Quaternion.identity);
            spawnedBeesYPositions.Add(yPos);
        }
    }

    private void SpawnFlies()
    {
        for (int i = 0; i < fliesQuantity; i++)
        {
            float xPos = Random.Range(xMin, xMax);
            float yPos = Random.Range(yMin, yMax);

            // Keep generating a new Y position until a valid one is found
            while (spawnedFliesYPositions.Contains(yPos) || IsFliesTooClose(yPos))
            {
                yPos = Random.Range(yMin, yMax);
            }

            Vector3 spawnPos = new Vector3(xPos, yPos, 0f);
            Instantiate(fliesPrefab, spawnPos, Quaternion.identity);
            spawnedFliesYPositions.Add(yPos);
        }
    }

    private bool IsBeesTooClose(float yPos)
    {
        foreach (float pos in spawnedBeesYPositions)
        {
            if (Mathf.Abs(pos - yPos) < minBeesDistance)
            {
                return true;
            }
        }
        return false;
    }

    private bool IsFliesTooClose(float yPos)
    {
        foreach (float pos in spawnedFliesYPositions)
        {
            if (Mathf.Abs(pos - yPos) < minFliesDistance)
            {
                return true;
            }
        }
        return false;
    }

    public void RemoveObstacles()
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacles");
        foreach (GameObject obstacle in obstacles)
        {
            Destroy(obstacle);
        }
        spawnedBeesYPositions.Clear();
        spawnedFliesYPositions.Clear();
    }
}
