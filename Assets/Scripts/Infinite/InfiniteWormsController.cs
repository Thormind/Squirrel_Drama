using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteWormsController : MonoBehaviour
{
    public static InfiniteWormsController instance;

    public List<GameObject> worms;

    public GameObject wormsParent;

    public GameObject wormPrefab;

    private int maxTries = 100;
    private float minDistance = 2f;
    private float maxDistance = 5f;

    private float minTime = 5f;
    private float maxTime = 10f;

    private float randomTime;
    private float time;
    private bool isSpawning;

    private Vector3 spawnPosition;
    private List<Vector3> _spawnedWormsPositions = new List<Vector3>();

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

    private void Start()
    {
        isSpawning = false;
        GetRandomTime();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            SpawnWorm();
        }
    }

    private void FixedUpdate()
    {
        if (isSpawning)
        {
            while (time > 0f)
            {
                SpawnWorm();
                GetRandomTime();
            }

            time -= Time.fixedDeltaTime / randomTime;
        }
    }

    public void SpawnWorms()
    {
        RemoveWorms();
        isSpawning = true;
    }

    public void SpawnWorm()
    {
        Vector3 holePosition = GetRandomHoleNearFruit();
        GameObject wormInstantiated = Instantiate(wormPrefab, holePosition, Quaternion.identity);
        worms.Add(wormInstantiated);
        wormInstantiated.transform.parent = wormsParent.transform;
        _spawnedWormsPositions.Add(holePosition);

        //wormInstantiated.GetComponent<InfiniteWormsAnimation>().PlayAnimation();
    }

    public void RemoveWorms()
    {
        isSpawning = false;

        foreach (GameObject w in worms)
        {
            Destroy(w);
        }

        worms.Clear();
        _spawnedWormsPositions.Clear();
    }

    private void GetRandomTime()
    {
        randomTime = Random.Range(minTime, maxTime);
        time = randomTime;
    }

    private Vector3 GetRandomHoleNearFruit()
    {
        Vector3 fruitPosition = InfiniteGameController.instance.GetFruitLocalPosition();
        int tries = 0;
        do
        {
            if (InfiniteHolesController.instance != null)
            {
                foreach (Vector3 spawnedPosition in InfiniteHolesController.instance.GetSpawnedPositions())
                {
                    Vector2 tmpFruitPos = fruitPosition;
                    Vector2 tmpSpawnedPos = spawnedPosition;
                    if (Vector2.Distance(tmpFruitPos, tmpSpawnedPos) < maxDistance || Vector2.Distance(tmpFruitPos, tmpSpawnedPos) > minDistance)
                    {
                        spawnPosition = wormsParent.transform.TransformPoint(spawnedPosition);
                    }
                }
            }

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
        foreach (Vector3 spawnedPosition in _spawnedWormsPositions)
        {
            if (Vector3.Distance(position, spawnedPosition) < minDistance)
            {
                return false;
            }
        }
        return true;
    }
}
