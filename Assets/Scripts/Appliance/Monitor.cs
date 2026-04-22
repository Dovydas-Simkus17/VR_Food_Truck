using UnityEngine;
using TMPro;

public class Monitor : MonoBehaviour
{
    public TextMeshPro levelText;
    public TextMeshPro scoreText;

    void Start()
    {
        Game_Manager.sharedInstance.OnGameUpdated += UpdateUI;
        UpdateUI();
    }
    void UpdateUI()
    {
        if (Game_Manager.sharedInstance == null) return;

        levelText.text = "Current Day: " + Game_Manager.sharedInstance.currentDayIndex;
        scoreText.text = "All Days: " + Game_Manager.sharedInstance.days.Count;
    }
    void OnDestroy()
    {
        if (Game_Manager.sharedInstance != null)
            Game_Manager.sharedInstance.OnGameUpdated -= UpdateUI;
    }
}
