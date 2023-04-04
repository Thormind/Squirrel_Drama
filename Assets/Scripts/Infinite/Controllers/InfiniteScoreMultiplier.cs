using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteScoreMultiplier : MonoBehaviour
{
    public delegate void MultiplierChangedEventHandler(int newMultiplier);
    public static event MultiplierChangedEventHandler OnMultiplierChanged;

    public static int multiplier
    {
        get { return _multiplier; }
        set
        {
            _multiplier = value;
            OnMultiplierChanged?.Invoke(_multiplier);
        }
    }
    private static int _multiplier = 1;
    private const int minMultiplier = 1;
    private const int maxMultiplier = 5;

    public delegate void MultiplierStreakChangedEventHandler (float newMultiplierStreak);
    public static event MultiplierStreakChangedEventHandler OnMultiplierStreakChanged;

    public static float currentStreak
    {
        get { return _currentStreak; }
        set
        {
            _currentStreak = value;
            OnMultiplierStreakChanged?.Invoke(_currentStreak);
        }
    }
    private static float _currentStreak = 0;
    private const int minMultiplierStreak = 0;
    private const int maxMultiplierStreak = 5;

    public delegate void MultiplierTimeStreakChangedEventHandler(float newMultiplierTimeStreak);
    public static event MultiplierTimeStreakChangedEventHandler OnMultiplierTimeStreakChanged;

    public static float currentTimeStreak
    {
        get { return _currentTimeStreak; }
        set
        {
            _currentTimeStreak = value;
            OnMultiplierTimeStreakChanged?.Invoke(_currentTimeStreak);
        }
    }
    private static float _currentTimeStreak = 0;

    [SerializeField] private InfiniteMultiplierParametersSO scoreMultiplierParameters;
    public float AllowedTimeBetweenPoints
    {
        get { return scoreMultiplierParameters.GetAllowedTimeBetweenPoints(multiplier); }
        set
        {
            scoreMultiplierParameters.SetAllowedTimeBetweenPoints(multiplier, value);
        }
    }
    //private const float allowedTimeBetweenPoints = 3f;

    private float lastPointTime = 0f;
    private float timeSinceLastPoint = 0f;

    private bool timerRunning;

    private void Start()
    {
        OnMultiplierChanged += HandleMultiplierChanged;
        OnMultiplierStreakChanged += HandleMultiplierStreakChanged;
        OnMultiplierTimeStreakChanged += HandleMultiplierTimeStreakChanged;

        timerRunning = true;
    }

    private void OnDestroy()
    {
        OnMultiplierChanged -= HandleMultiplierChanged;
        OnMultiplierStreakChanged -= HandleMultiplierStreakChanged;
        OnMultiplierTimeStreakChanged -= HandleMultiplierTimeStreakChanged;
    }

    private void Update()
    {
        if (timerRunning)
        {
            UpdateMultiplierStreak();
        }
    }

    private void HandleMultiplierChanged(int newMultiplier)
    {
        InfiniteGameController.instance.UpdateHUD(GAME_DATA.CURRENT_MULTIPLIER);
    }

    private void HandleMultiplierStreakChanged(float newMultiplierStreak)
    {
        InfiniteGameController.instance.UpdateHUD(GAME_DATA.MULTIPLIER_STREAK);
    }

    private void HandleMultiplierTimeStreakChanged(float newMultiplierTimeStreak)
    {
        InfiniteGameController.instance.UpdateHUD(GAME_DATA.MULTIPLIER_TIME);
    }


    public void CollectInfiniteFruitOrPoint()
    {
        currentStreak++;

        if (currentStreak == maxMultiplierStreak && multiplier < maxMultiplier)
        {
            multiplier++;
            currentStreak = minMultiplierStreak;
        }
       
        lastPointTime = Time.time;
    }

    public void UpdateMultiplierStreak()
    {
        timeSinceLastPoint = Time.time - lastPointTime;

        currentTimeStreak = GetConvertedTimeLeftBeforeEndOfStreak();

        if (timeSinceLastPoint > AllowedTimeBetweenPoints)
        {
            currentStreak = minMultiplierStreak;

            int tmpMultiplier = Mathf.Max(multiplier - 1, minMultiplier);
            if (tmpMultiplier != multiplier)
            {
                multiplier = tmpMultiplier;
            }
            lastPointTime = Time.time;
        }
    }

    public int ApplyMultiplierToScore(int score)
    {
        return score * multiplier;
    }

    public int GetCurrentMultiplier()
    {
        return multiplier;
    }

    public float GetCurrentMultiplierStreak()
    {
        return currentStreak;
    }

    public float GetTimeSinceLastPoint()
    {
        return timeSinceLastPoint;
    }

    public float GetAllowedTimeBetweenPoints()
    {
        return AllowedTimeBetweenPoints;
    }

    public float GetTimeLeftBeforeEndOfStreak()
    {
        return AllowedTimeBetweenPoints - timeSinceLastPoint;
    }

    public float GetConvertedTimeLeftBeforeEndOfStreak()
    {
        float convertedStreakTimeLeft;

        if (currentStreak == minMultiplierStreak && multiplier > minMultiplier && timeSinceLastPoint > AllowedTimeBetweenPoints)
        {
            convertedStreakTimeLeft = GetTimeLeftBeforeEndOfStreak() * maxMultiplierStreak / AllowedTimeBetweenPoints;
        }
        else
        {
            convertedStreakTimeLeft = GetTimeLeftBeforeEndOfStreak() * currentStreak / AllowedTimeBetweenPoints;
        }

        return Mathf.Max(convertedStreakTimeLeft, 0);
    }

    public void ResetMultiplier()
    {
        multiplier = 1;
        currentStreak = 0;
        lastPointTime = 0;
        timeSinceLastPoint = 0;
        timerRunning = true;
    }

    public void StopMultiplier()
    {
        currentStreak = 0;
        currentTimeStreak = 0;
        lastPointTime = 0;
        timeSinceLastPoint = 0;
        timerRunning = false;
    }
}
