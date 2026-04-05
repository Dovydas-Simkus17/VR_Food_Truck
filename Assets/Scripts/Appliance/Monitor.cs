using UnityEngine;
using TMPro;

public class Monitor : MonoBehaviour
{
    public TextMeshPro levelText;
    public TextMeshPro scoreText;

    void Start()
    {
        GameManager.Instance.OnGameUpdated += UpdateUI;
        UpdateUI();
    }
    void UpdateUI()
    {
        if (GameManager.Instance == null) return;

        levelText.text = "Level: " + GameManager.Instance.currentLevel;
        scoreText.text = "Score: " + GameManager.Instance.score;
    }
}
