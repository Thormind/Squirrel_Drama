using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InfiniteFruitsController : MonoBehaviour
{

    public static InfiniteFruitsController instance;

    public GameObject fruitsParent;
    public GameObject fruitPrefab;

    private List<Vector3> _spawnedFruitsPositions = new List<Vector3>();

    //Difficulty Parameters
    [SerializeField] private FruitsParametersSO _fruitsParameters;

    //Spawning Limits Parameters
    private float xMin = -2.5f;
    private float xMax = 2.5f;

    private float yMin = 10f;
    private float yMax = 30f;

    private int maxTries = 500;
    public bool isAllSpawned;


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

        isAllSpawned = false;
    }

    public void SpawnFruits()
    {
        QuickRemoveFruits();

        for (int i = 0; i < FruitsQuantity; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();

            if (spawnPosition == Vector3.zero)
            {
                NotifySpawnDebug(false);
                break;
            }

            NotifySpawnDebug(true);

            InstantiateAnimation(spawnPosition, true);

            _spawnedFruitsPositions.Add(spawnPosition);
        }
    }

    public void QuickRemoveFruits()
    {

        for (int i = 0; i < fruitsParent.transform.childCount; i++)
        {
            GameObject child = fruitsParent.transform.GetChild(i).gameObject;
            Destroy(child);
        }

        _spawnedFruitsPositions.Clear();
        _spawnedFruitsPositions = new List<Vector3>();
    }

    public void RemoveFruits()
    {

        for (int i = 0; i < fruitsParent.transform.childCount; i++)
        {
            GameObject child = fruitsParent.transform.GetChild(i).gameObject;
            if (child != null)
            {
                InstantiateAnimation(child.transform.position, false, child);
            }
        }

        _spawnedFruitsPositions.Clear();
        _spawnedFruitsPositions = new List<Vector3>();
    }


    public void InstantiateAnimation(Vector3 position, bool spawn, GameObject obj = null)
    {
        if (AnimationManager.instance != null)
        {
            AnimationManager.instance.PlayObstaclesAnimation(AnimateInstantiate(position, spawn, obj));
        }

    }

    private IEnumerator AnimateInstantiate(Vector3 position, bool spawn, GameObject obj = null)
    {
        //play sound 

        Instantiate(InfiniteGameController.instance.obstacleInstanciateVFX, position, Quaternion.identity, fruitsParent.transform);

        if (spawn)
        {
            GameObject fruitInstantiated = Instantiate(fruitPrefab, position, Quaternion.identity, fruitsParent.transform);
        }
        if (!spawn && obj != null)
        {
            Destroy(obj);
        }


        yield return new WaitForSeconds(0.05f);
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

            localPosition = new Vector3(x, y, -0.1f);
            spawnPosition = fruitsParent.transform.TransformPoint(localPosition);

            tries++;
            if (tries >= maxTries)
            {
                spawnPosition = Vector3.zero;
                break;
            }
        } while (!IsValidPosition(localPosition));

        if (spawnPosition != Vector3.zero)
        {
            _spawnedFruitsPositions.Add(localPosition);
        }

        return spawnPosition;
    }

    private bool IsValidPosition(Vector3 position)
    {
        foreach (Vector2 spawnedPosition in _spawnedFruitsPositions)
        {
            if (Vector2.Distance(position, spawnedPosition) < FruitsMinDistance + 5f)
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
                if (Vector2.Distance(tmpPos, tmpSpawnedPos) < FruitsMinDistance)
                {
                    return false;
                }
            }
        }
        if (InfinitePointsController.instance != null)
        {
            foreach (Vector3 spawnedPosition in InfinitePointsController.instance.GetSpawnedPositions())
            {
                Vector2 tmpPos = position;
                Vector2 tmpSpawnedPos = spawnedPosition;
                if (Vector2.Distance(tmpPos, tmpSpawnedPos) < FruitsMinDistance)
                {
                    return false;
                }
            }
        }

        return true;
    }


    private void NotifySpawnDebug(bool spawned)
    {
        isAllSpawned = spawned;

        if (LevelScalingController.instance != null)
        {
            LevelScalingController.instance.UpdateObstacleMessages();
        }
    }

    //FRUITS QUANTITY
    public int FruitsQuantity
    {
        get { return _fruitsParameters.GetQuantityForLevel(InfiniteGameController.instance.difficultyLevel); }
        set
        {
            _fruitsParameters.SetQuantityForLevel(InfiniteGameController.instance.difficultyLevel, value);
            SpawnFruits();
        }
    }

    //MIN DISTANCE
    public float FruitsMinDistance
    {
        get { return _fruitsParameters.GetMinDistanceForLevel(InfiniteGameController.instance.difficultyLevel); }
        set
        {
            _fruitsParameters.SetMinDistanceForLevel(InfiniteGameController.instance.difficultyLevel, value);
            SpawnFruits();
        }
    }

}
