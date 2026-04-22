using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{

    public static Game_Manager sharedInstance;

    public DayButton DayButton;
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
        if (sharedInstance != null && sharedInstance != this)
        {
            Destroy(gameObject);
            return;
        }

        sharedInstance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartDay()
    {
        if (dayActive) return;

        dayActive = true;

        OnDayStart?.Invoke(CurrentDay);

        Debug.Log("Starting Day " + currentDayIndex);
    }

    public void EndDay()
    {
        if (!dayActive) return;

        dayActive = false;

        OnDayEnd?.Invoke();
        DayButton.isOpen = false;

        
        AdvanceDay();
    }

    void AdvanceDay()
    {
        if (currentDayIndex < days.Count - 1)
        {
            currentDayIndex++;
            OnGameUpdated?.Invoke();
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

    public void ResetGame()
    {
        currentDayIndex = 0;
        positiveScore = 0;
        negativeScore = 0;
        dayActive = false;

        Debug.Log("Game Reset");
    }
}