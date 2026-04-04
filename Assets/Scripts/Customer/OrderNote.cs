using UnityEngine;
using TMPro;

public class OrderNote : MonoBehaviour
{
    public TextMeshPro text;

    private RecipeSO recipe;

    public void Setup(RecipeSO newRecipe)
    {
        this.recipe = newRecipe;

        string display = recipe.recipeName + "\n";

        foreach (var item in recipe.kitchenObjectSOList)
        {
            display += "- " + item.name + "\n";
        }

        text.text = display;
    }
}