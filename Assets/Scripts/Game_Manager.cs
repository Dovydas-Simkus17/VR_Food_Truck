using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game State")]
    public int currentLevel = 1;
    public int score = 0;

    [Header("Level Settings")]
    public int[] levelThresholds = { 10, 25, 50 }; // points needed

    public System.Action OnGameUpdated;
    void Awake()
    {
        // Singleton setup
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddScore(int amount)
    {
        score += amount;
        CheckLevelUp();
        OnGameUpdated?.Invoke();
    }

    void CheckLevelUp()
    {
        if (currentLevel < levelThresholds.Length)
        {
            if (score >= levelThresholds[currentLevel - 1])
            {
                currentLevel++;
                Debug.Log("Level Up! Now level " + currentLevel);
            }
        }
    }
}