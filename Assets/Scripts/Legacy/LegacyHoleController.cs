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

    private int holesQuantity;
    private float minDistance;

    public float holesIndicatorXMin = -1.75f;
    public float holesIndicatorXMax = 1.75f;

    public float holesIndicatorYMin = -2.75f;
    public float holesIndicatorYMax = 2.75f;

    public float minHolesIndicatorYDistance = 0.6f;
    public float maxHolesIndicatorYDistance = 0.9f;

    public float xMin = -2.25f;
    public float xMax = 2.25f;

    public float yMin = -3.75f;
    public float yMax = 3.75f;

    public int maxTries = 100;


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

            GameObject holeInstantiated = Instantiate(holePrefab, spawnPosition, Quaternion.identity, holesParent.transform);
            holes.Add(holeInstantiated);
        }
    }

    public void SpawnHolesIndicators()
    {
        maxHolesIndicatorYDistance = ((Mathf.Abs(holesIndicatorYMin - holesIndicatorYMax)) / 10) + 0.2f;

        float currentY = holesIndicatorYMin;

        for (int i = 1; i <= 10; i++)
        {
            float randomX = Random.Range(holesIndicatorXMin, holesIndicatorXMax);

            Vector3 localPosition = new Vector3(randomX, currentY, 0);
            Vector3 spawnPosition = holesParent.transform.TransformPoint(localPosition);

            GameObject holeInstantiated = Instantiate(holePrefab, spawnPosition, Quaternion.identity, holesParent.transform);
            holes.Add(holeInstantiated);

            GameObject holeIndicatorInstantiated = Instantiate(holeIndicatorPrefab, spawnPosition, Quaternion.identity, holesParent.transform);
            holeIndicatorList.Add(holeIndicatorInstantiated);

            _spawnedHolesPositions.Add(localPosition);

            holeIndicatorInstantiated.GetComponent<HoleIndicator>().SetHoleNumber(i);

            float randomY = Random.Range(minHolesIndicatorYDistance, maxHolesIndicatorYDistance);
            currentY += randomY;
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
        } while (!IsValidPosition(localPosition));


        if (spawnPosition != Vector3.zero)
        {
            _spawnedHolesPositions.Add(localPosition);
        }

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

    public void SetHolesQuantity(int quantity)
    {
        holesQuantity = quantity;
    }

    public void SetHolesMinDistance(float minDis)
    {
        minDistance = minDis;
    }
}
