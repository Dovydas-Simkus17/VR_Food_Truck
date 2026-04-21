using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{

    public static Game_Manager sharedInstance;


    public List<DayData> days;

    public int currentDayIndex = 0;
    public bool dayActive;
    public int positiveScore = 0;
    public int negativeScore = 0;

    public DayData CurrentDay => days[currentDayIndex];

    public System.Action<DayData> OnDayStart;
    public System.Action OnDayEnd;
    public System.Action OnGameUpdated;
    void Awake()
    {
        // Singleton setup
        if (sharedInstance == null)
            sharedInstance = this;
        else
            Destroy(gameObject);
    }

    public void StartDay()
    {
        if (dayActive) return;

        dayActive = true;

        OnDayStart?.Invoke(CurrentDay);

        Debug.Log("Starting Day " + CurrentDay.dayNumber);
    }

    public void EndDay()
    {
        if (!dayActive) return;

        dayActive = false;

        OnDayEnd?.Invoke();

        AdvanceDay();
    }

    void AdvanceDay()
    {
        if (currentDayIndex < days.Count - 1)
        {
            currentDayIndex++;
        }
        else
        {
            Debug.Log("All days completed!");
            Loader.Load(Loader.Scene.EndGameScene);
        }
    }
    public void addPosiScore(int score)
    {
        positiveScore += score;
    }
    public void addNegiScore(int score)
    {
        negativeScore += score;
    }

}