using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InfiniteHolesController : MonoBehaviour
{

    public static InfiniteHolesController instance;

    public List<GameObject> holes;

    public GameObject holesParent;

    public GameObject holePrefab;

    public int holesQuantity = 50;
    public int maxTries = 100;
    public float minDistance = 0.5f;

    public float xMin = -2.75f;
    public float xMax = 2.75f;

    public float yMin = 4f;
    public float yMax = 36f;


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

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.R))
        {
            SpawnHoles();
        }

    }

    public void SpawnHoles()
    {
        RemoveHoles();

        for (int i = 0; i < holesQuantity; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            if (spawnPosition == Vector3.zero)
            {
                Debug.Log("Could not find a valid spawn position after " + maxTries + " tries.");
                break;
            }

            //Vector3 worldPosition = holesParent.transform.TransformPoint(spawnPosition);
            GameObject holeInstantiated = Instantiate(holePrefab, spawnPosition, Quaternion.identity);
            holes.Add(holeInstantiated);
            holeInstantiated.transform.parent = holesParent.transform;

            _spawnedHolesPositions.Add(spawnPosition);
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
        } while (!IsValidPosition(spawnPosition));
        _spawnedHolesPositions.Add(localPosition);
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

        foreach (GameObject g in holes)
        {
            Destroy(g);
        }

        holes.Clear();

        _spawnedHolesPositions.Clear();
    }

    public List<Vector3> GetSpawnedPositions()
    {
        return _spawnedHolesPositions;
    }

}
