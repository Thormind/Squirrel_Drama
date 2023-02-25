using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LegacyHoleController : MonoBehaviour
{

    public static LegacyHoleController instance;

    public List<GameObject> holes;

    public GameObject holesParent;

    public GameObject holePrefab;
    public GameObject holeIndicatorPrefab;

    public List<GameObject> holeIndicatorList;

    public int holesQuantity = 50;
    public int maxTries = 100;
    public float minDistance = 0.5f;

    public float holesIndicatorXMin = -1.75f;
    public float holesIndicatorXMax = 1.75f;

    public float holesIndicatorYMin = -3f;
    public float holesIndicatorYMax = 3f;

    public float minHolesIndicatorYDistance = 0.6f;
    public float maxHolesIndicatorYDistance = 0.9f;

    public float xMin = -2.25f;
    public float xMax = 2.25f;

    public float yMin = -4f;
    public float yMax = 4f;


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
        SpawnHolesIndicators();

        for (int i = 0; i < holesQuantity; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            if (spawnPosition == Vector3.zero)
            {
                Debug.Log("Could not find a valid spawn position after " + maxTries + " tries.");
                break;
            }
            GameObject holeInstantiated = Instantiate(holePrefab, spawnPosition, Quaternion.identity);
            holes.Add(holeInstantiated);
            holeInstantiated.transform.parent = holesParent.transform;

            _spawnedHolesPositions.Add(spawnPosition);
        }
    }

    public void SpawnHolesIndicators()
    {
        maxHolesIndicatorYDistance = ((Mathf.Abs(holesIndicatorYMin - holesIndicatorYMax)) / 10) + 0.2f;

        float currentY = holesIndicatorYMin;

        for (int i = 1; i <= 10; i++)
        {
            float randomX = Random.Range(holesIndicatorXMin, holesIndicatorXMax);

            Vector3 spawnPosition = new Vector3(randomX, currentY, 0);

            GameObject holeInstantiated = Instantiate(holePrefab, spawnPosition, Quaternion.identity);
            holes.Add(holeInstantiated);
            holeInstantiated.transform.parent = holesParent.transform;

            GameObject holeIndicatorInstantiated = Instantiate(holeIndicatorPrefab, spawnPosition, Quaternion.identity);
            holeIndicatorList.Add(holeIndicatorInstantiated);
            holeIndicatorInstantiated.transform.parent = holesParent.transform;

            _spawnedHolesPositions.Add(spawnPosition);

            holeIndicatorInstantiated.GetComponent<HoleIndicator>().SetHoleNumber(i);

            float randomY = Random.Range(minHolesIndicatorYDistance, maxHolesIndicatorYDistance);
            currentY += randomY;
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
        foreach (GameObject g in holeIndicatorList)
        {
            Destroy(g);
        }

        holeIndicatorList.Clear();

        foreach (GameObject g in holes)
        {
            Destroy(g);
        }

        holes.Clear();

        _spawnedHolesPositions.Clear();
    }

    [ContextMenu("Asign Hole Indicators")]
    void AsignHoleIndicators()
    {
        foreach (GameObject g in holeIndicatorList)
        {
            DestroyImmediate(g);
        }

        holeIndicatorList.Clear();

        for (int i = 0; i < holes.Count; i++)
        {
            GameObject holeIndicatorInstantiated = Instantiate(holeIndicatorPrefab, holes[i].transform.position, Quaternion.identity);
            holeIndicatorList.Add(holeIndicatorInstantiated);

            holeIndicatorInstantiated.GetComponent<HoleIndicator>().SetHoleNumber(i + 1);
        }

    }
}
