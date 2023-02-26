using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteBeesController : MonoBehaviour
{
    public static InfiniteBeesController instance;

    public GameObject beesParent;

    public GameObject beePrefab;

    public int beesQuantity = 10;
    public int maxTries = 100;
    public float minDistance = 0.5f;

    public float beesXMin = -2.5f;
    public float beesXMax = 2.5f;

    public float beesYMin = -3.5f;
    public float beesYMax = 3.5f;

    public float minBeesYDistance = 0.3f;
    public float maxBeesYDistance = 0.9f;


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

        float currentY = beesYMin;

        for (int i = 1; i <= beesQuantity; i++)
        {
            float randomX = Random.Range(beesXMin, beesXMax);
            float randomY = Random.Range(minBeesYDistance, maxBeesYDistance);
            currentY += randomY;

            Vector3 spawnPosition = new Vector3(randomX, currentY, 0);

            GameObject beeInstantiated = Instantiate(beePrefab, spawnPosition, Quaternion.identity);
            beeInstantiated.GetComponent<BeeMovement>().initialPosition = spawnPosition;

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
    }

}
