using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipeListSO", menuName = "Scriptable Objects/RecipeListSO")]
public class RecipeListSO : ScriptableObject
{
    public System.Collections.Generic.List<RecipeSO> recipeSOList;
}
