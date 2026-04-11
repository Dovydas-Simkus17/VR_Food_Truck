using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
public class IngredientMonitorManager : MonoBehaviour
{
    public static IngredientMonitorManager Instance;

    public List<KitchenObjectSO> availableIngredients = new List<KitchenObjectSO>();
    private KitchenObjectSO currentIngredient;
    void Awake()
    {
        Instance = this;
    }

    private int currentIndex = 0;

    [Header("UI")]
    public Image iconImage;
    public TextMeshProUGUI nameText;
    public BoxSpawner boxSpawn;
   
    void Start()
    {
        UpdateUI();
    }

    public void Next()
    {
        currentIndex++;

        if (currentIndex >= availableIngredients.Count)
            currentIndex = 0;
        Debug.Log("Next" + currentIndex);
        UpdateUI();
    }

    public void Previous()
    {
        currentIndex--;

        if (currentIndex < 0)
            currentIndex = availableIngredients.Count - 1;
        Debug.Log("Prevoius" + currentIndex);
        UpdateUI();
    }

    void UpdateUI()
    {
        var ingredient = availableIngredients[currentIndex];
        iconImage.sprite = ingredient.icon;
        nameText.text = ingredient.stateName;
    }

    public void OrderCurrent()
    {
        var ingredient = availableIngredients[currentIndex];

        currentIngredient = ingredient;
  
        Debug.Log("Ordered: " + currentIngredient.stateName);
        boxSpawn.SpawnBox(currentIngredient);

    }
}
