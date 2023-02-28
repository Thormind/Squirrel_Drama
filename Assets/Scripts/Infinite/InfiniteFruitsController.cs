using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InfiniteFruitsController : MonoBehaviour
{

    public static InfiniteFruitsController instance;

    public List<GameObject> fruits;

    public GameObject fruitsParent;

    public GameObject fruitPrefab;

    private int fruitsQuantity = 1;
    private int maxTries = 100;
    private float minDistance = 2f;

    private float xMin = -2.5f;
    private float xMax = 2.5f;

    private float yMin = 10f;
    private float yMax = 30f;


    private List<Vector3> _spawnedFruitsPositions = new List<Vector3>();

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

        if (Input.GetKeyDown(KeyCode.F))
        {
            SpawnFruits();
        }

    }

    public void SpawnFruits()
    {
        RemoveFruits();

        for (int i = 0; i < fruitsQuantity; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            if (spawnPosition == Vector3.zero)
            {
                Debug.Log("Could not find a valid spawn position after " + maxTries + " tries.");
                break;
            }
            GameObject fruitInstantiated = Instantiate(fruitPrefab, spawnPosition, Quaternion.identity);
            fruits.Add(fruitInstantiated);
            fruitInstantiated.transform.parent = fruitsParent.transform;

            _spawnedFruitsPositions.Add(spawnPosition);
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
            spawnPosition = fruitsParent.transform.TransformPoint(new Vector3(x, y, -0.25f));

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
        foreach (Vector3 spawnedPosition in _spawnedFruitsPositions)
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

    public void RemoveFruits()
    {

        foreach (GameObject g in fruits)
        {
            Destroy(g);
        }

        fruits.Clear();

        _spawnedFruitsPositions.Clear();
    }

}
