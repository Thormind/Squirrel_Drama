using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelScalingController : MonoBehaviour
{
    public static LevelScalingController instance;

    public TMP_Dropdown difficultyDropdown;
    public TMP_Dropdown obstacleDropdown;
    public Transform slidersParent;
    public Slider sliderPrefab;
    public Transform messagesParent;
    public TextMeshProUGUI messagePrefab;

    private void Awake()
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
        // Initialize difficulty dropdown
        difficultyDropdown.ClearOptions();
        for (int i = 1; i <= 10; i++)
        {
            difficultyDropdown.options.Add(new TMP_Dropdown.OptionData(i.ToString()));
        }
        difficultyDropdown.onValueChanged.AddListener(delegate { OnDifficultyChanged(difficultyDropdown.value); });

        // Initialize obstacle dropdown
        obstacleDropdown.ClearOptions();
        obstacleDropdown.options.Add(new TMP_Dropdown.OptionData("Holes"));
        obstacleDropdown.options.Add(new TMP_Dropdown.OptionData("Bees"));
        obstacleDropdown.options.Add(new TMP_Dropdown.OptionData("Worms"));
        obstacleDropdown.options.Add(new TMP_Dropdown.OptionData("Bear"));
        obstacleDropdown.options.Add(new TMP_Dropdown.OptionData("Points"));
        obstacleDropdown.options.Add(new TMP_Dropdown.OptionData("Fruits"));
        obstacleDropdown.onValueChanged.AddListener(delegate { OnObstacleChanged(obstacleDropdown.value); });

        difficultyDropdown.RefreshShownValue();
        obstacleDropdown.RefreshShownValue();

        // Initialize sliders for first obstacle
        UpdateObstacleParameters(0);
    }

    private void OnDifficultyChanged(int value)
    {
        InfiniteGameController.instance.SetLevel(value + 1);
        UpdateObstacleParameters(obstacleDropdown.value);
    }

    private void OnObstacleChanged(int value)
    {
        UpdateObstacleParameters(value);
    }

    public void InstantiateSlider(string obstacleName, string parameterName, float minValue, float maxValue, float currentValue, int index)
    {
        var sliderObj = Instantiate(sliderPrefab, slidersParent);
        sliderObj.transform.localPosition = new Vector3(0f, -50f * index, 0f);
        sliderObj.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = parameterName;

        Slider slider = sliderObj.GetComponent<Slider>();
        slider.minValue = minValue;
        slider.maxValue = maxValue;
        slider.value = currentValue;

        bool wholeNumbers = parameterName switch
        {
            "holesQuantity" => true,
            "beesQuantity" => true,
            "pointsQuantity" => true,
            "fruitsQuantity" => true,
            _ => false,
        };

        // Set the wholeNumbers property on the slider
        slider.wholeNumbers = wholeNumbers;

        SliderUpdater sliderUpdater = sliderObj.GetComponent<SliderUpdater>();
        sliderUpdater.UpdateSlider(parameterName, currentValue);

        slider.onValueChanged.AddListener(delegate { sliderUpdater.UpdateSlider(parameterName, slider.value); });

        switch (obstacleName)
        {
            case "Holes":
                switch (parameterName)
                {
                    case "holesQuantity":
                        slider.onValueChanged.AddListener(delegate { InfiniteHolesController.instance.HolesQuantity = (int)slider.value; });
                        break;
                    case "minDistance":
                        slider.onValueChanged.AddListener(delegate { InfiniteHolesController.instance.HolesMinDistance = slider.value; });
                        break;
                }
                break;

            case "Bees":
                switch (parameterName)
                {
                    case "beesQuantity":
                        slider.onValueChanged.AddListener(delegate { InfiniteBeesController.instance.BeesQuantity = (int)slider.value; });
                        break;
                    case "minBeesYDistance":
                        slider.onValueChanged.AddListener(delegate { InfiniteBeesController.instance.BeesMinDistance = slider.value; });
                        break;
                    case "movementSpeed":
                        slider.onValueChanged.AddListener(delegate { InfiniteBeesController.instance.BeesMovementSpeed = slider.value; });
                        break;
                }
                break;
            case "Worms":
                switch (parameterName)
                {
                    case "minDistance":
                        slider.onValueChanged.AddListener(delegate { InfiniteWormsController.instance.WormsMinDistance = slider.value; });
                        break;
                    case "maxDistance":
                        slider.onValueChanged.AddListener(delegate { InfiniteWormsController.instance.WormsMaxDistance = slider.value; });
                        break;
                    case "minSpawningInterval":
                        slider.onValueChanged.AddListener(delegate { InfiniteWormsController.instance.WormsMinSpawningInterval = slider.value; });
                        break;
                    case "maxSpawningInterval":
                        slider.onValueChanged.AddListener(delegate { InfiniteWormsController.instance.WormsMaxSpawningInterval = slider.value; });
                        break;
                    case "spawnProbability":
                        slider.onValueChanged.AddListener(delegate { InfiniteWormsController.instance.WormsSpawnProbability = slider.value; });
                        break;
                    case "inAnimationTime":
                        slider.onValueChanged.AddListener(delegate { InfiniteWormsController.instance.WormsInAnimationTime = slider.value; });
                        break;
                    case "derpAnimationTime":
                        slider.onValueChanged.AddListener(delegate { InfiniteWormsController.instance.WormsDerpAnimationTime = slider.value; });
                        break;
                    case "animationSpeed":
                        slider.onValueChanged.AddListener(delegate { InfiniteWormsController.instance.WormsAnimationSpeed = slider.value; });
                        break;
                }
                break;
            case "Bear":
                switch (parameterName)
                {
                    case "minDistance":
                        slider.onValueChanged.AddListener(delegate { InfiniteBearController.instance.BearMinDistance = slider.value; });
                        break;
                    case "maxDistance":
                        slider.onValueChanged.AddListener(delegate { InfiniteBearController.instance.BearMaxDistance = slider.value; });
                        break;
                    case "minSpawningInterval":
                        slider.onValueChanged.AddListener(delegate { InfiniteBearController.instance.BearMinSpawningInterval = slider.value; });
                        break;
                    case "maxSpawningInterval":
                        slider.onValueChanged.AddListener(delegate { InfiniteBearController.instance.BearMaxSpawningInterval = slider.value; });
                        break;
                    case "spawnProbability":
                        slider.onValueChanged.AddListener(delegate { InfiniteBearController.instance.BearSpawnProbability = slider.value; });
                        break;
                    case "warnAnimationTime":
                        slider.onValueChanged.AddListener(delegate { InfiniteBearController.instance.BearWarnAnimationTime = slider.value; });
                        break;
                    case "impactRange":
                        slider.onValueChanged.AddListener(delegate { InfiniteBearController.instance.BearImpactRange = slider.value; });
                        break;
                }
                break;
            case "Points":
                switch (parameterName)
                {
                    case "pointsQuantity":
                        slider.onValueChanged.AddListener(delegate { InfinitePointsController.instance.PointsQuantity = (int)slider.value; });
                        break;
                    case "minDistance":
                        slider.onValueChanged.AddListener(delegate { InfinitePointsController.instance.PointsMinDistance = slider.value; });
                        break;
                }
                break;

            case "Fruits":
                switch (parameterName)
                {
                    case "fruitsQuantity":
                        slider.onValueChanged.AddListener(delegate { InfiniteFruitsController.instance.FruitsQuantity = (int)slider.value; });
                        break;
                    case "minDistance":
                        slider.onValueChanged.AddListener(delegate { InfiniteFruitsController.instance.FruitsMinDistance = slider.value; });
                        break;
                }
                break;

            default:
                Debug.LogError("Obstacle name not found:");
                break;
        }
    }

    public void InstantiateMessage(string obstacleName, int index)
    {
        var messageObj = Instantiate(messagePrefab, messagesParent);
        messageObj.transform.localPosition = new Vector3(0f, -75f * index, 0f);

        string message;
        bool isAllSpawned;

        switch (obstacleName)
        {
            case "Holes":
                isAllSpawned = InfiniteHolesController.instance.isAllSpawned;
                break;
            case "Bees":
                isAllSpawned = InfiniteBeesController.instance.isAllSpawned;
                break;
            case "Worms":
                isAllSpawned = InfiniteWormsController.instance.isAllSpawned;
                break;
            case "Bear":
                isAllSpawned = InfiniteBearController.instance.isAllSpawned;
                break;
            case "Points":
                isAllSpawned = InfinitePointsController.instance.isAllSpawned;
                break;
            case "Fruits":
                isAllSpawned = InfiniteFruitsController.instance.isAllSpawned;
                break;
            default:
                isAllSpawned = false;
                Debug.LogError("Obstacle index not found:");
                break;
        }

        if (isAllSpawned)
        {
            message = $"All {obstacleName} found a spawn position";
            messageObj.GetComponent<TMPro.TextMeshProUGUI>().text = message;
            messageObj.GetComponent<TMPro.TextMeshProUGUI>().color = Color.green;
        }
        else
        {
            message = $"Not all {obstacleName} found a spawn position";
            messageObj.GetComponent<TMPro.TextMeshProUGUI>().text = message;
            messageObj.GetComponent<TMPro.TextMeshProUGUI>().color = Color.red;
        }
    }

    private void UpdateObstacleParameters(int obstacleIndex)
    {
        // Clear previous sliders
        foreach (Transform child in slidersParent.transform)
        {
            Destroy(child.gameObject);
        }

        // Instantiate new sliders for selected obstacle
        switch (obstacleIndex)
        {
            case 0: // Holes
                if (InfiniteHolesController.instance != null)
                {
                    InstantiateSlider("Holes", "holesQuantity", 0, 150, InfiniteHolesController.instance.HolesQuantity, 0);
                    InstantiateSlider("Holes", "minDistance", 0f, 3f, InfiniteHolesController.instance.HolesMinDistance, 1);
                }
                break;
            case 1: // Bees
                if (InfiniteBeesController.instance != null)
                {
                    InstantiateSlider("Bees", "beesQuantity", 0, 60, InfiniteBeesController.instance.BeesQuantity, 0);
                    InstantiateSlider("Bees", "minBeesYDistance", 0f, 10f, InfiniteBeesController.instance.BeesMinDistance, 1);
                    InstantiateSlider("Bees", "movementSpeed", 0f, 2f, InfiniteBeesController.instance.BeesMovementSpeed, 2);
                }
                break;
            case 2: // Worms
                if (InfiniteWormsController.instance != null)
                {
                    InstantiateSlider("Worms", "minDistance", 0f, 6f, InfiniteWormsController.instance.WormsMinDistance, 0);
                    InstantiateSlider("Worms", "maxDistance", 0f, 6f, InfiniteWormsController.instance.WormsMaxDistance, 1);
                    InstantiateSlider("Worms", "minSpawningInterval", 0f, 10f, InfiniteWormsController.instance.WormsMinSpawningInterval, 2);
                    InstantiateSlider("Worms", "maxSpawningInterval", 0f, 10f, InfiniteWormsController.instance.WormsMaxSpawningInterval, 3);
                    InstantiateSlider("Worms", "spawnProbability", 0f, 1f, InfiniteWormsController.instance.WormsSpawnProbability, 4);
                    InstantiateSlider("Worms", "inAnimationTime", 0f, 10f, InfiniteWormsController.instance.WormsInAnimationTime, 5);
                    InstantiateSlider("Worms", "derpAnimationTime", 0f, 10f, InfiniteWormsController.instance.WormsDerpAnimationTime, 6);
                    InstantiateSlider("Worms", "animationSpeed", 100f, 1000f, InfiniteWormsController.instance.WormsAnimationSpeed, 7);
                }
                break;
            case 3: // Bears
                if (InfiniteBearController.instance != null)
                {
                    InstantiateSlider("Bear", "minDistance", 0f, 6f, InfiniteBearController.instance.BearMinDistance, 0);
                    InstantiateSlider("Bear", "maxDistance", 0f, 6f, InfiniteBearController.instance.BearMaxDistance, 1);
                    InstantiateSlider("Bear", "minSpawningInterval", 0f, 10f, InfiniteBearController.instance.BearMinSpawningInterval, 2);
                    InstantiateSlider("Bear", "maxSpawningInterval", 0f, 10f, InfiniteBearController.instance.BearMaxSpawningInterval, 3);
                    InstantiateSlider("Bear", "spawnProbability", 0f, 1f, InfiniteBearController.instance.BearSpawnProbability, 4);
                    InstantiateSlider("Bear", "warnAnimationTime", 0f, 10f, InfiniteBearController.instance.BearWarnAnimationTime, 5);
                    InstantiateSlider("Bear", "impactRange", 0f, 10f, InfiniteBearController.instance.BearImpactRange, 6);
                }
                break;
            case 4: // Points
                if (InfinitePointsController.instance != null)
                {
                    InstantiateSlider("Points", "pointsQuantity", 0, 100, InfinitePointsController.instance.PointsQuantity, 0);
                    InstantiateSlider("Points", "minDistance", 0f, 2f, InfinitePointsController.instance.PointsMinDistance, 1);
                }
                break;
            case 5: // Fruits
                if (InfiniteFruitsController.instance != null)
                {
                    InstantiateSlider("Fruits", "fruitsQuantity", 0, 3, InfiniteFruitsController.instance.FruitsQuantity, 0);
                    InstantiateSlider("Fruits", "minDistance", 0f, 2f, InfiniteFruitsController.instance.FruitsMinDistance, 1);
                }
                break;
            default:
                Debug.LogError("Parameter name not found:");
                break;
        }
    }

    public void UpdateObstacleMessages()
    {
        // Clear previous sliders
        foreach (Transform child in messagesParent.transform)
        {
            Destroy(child.gameObject);
        }

        int i = 0;
        foreach (TMP_Dropdown.OptionData child in obstacleDropdown.options)
        {
            InstantiateMessage(child.text, i);
            i++;
        }
    }
}
