using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipeSO", menuName = "Scriptable Objects/RecipeSO")]
public class RecipeSO : ScriptableObject
{
    public System.Collections.Generic.List<KitchenObjectSO> kitchenObjectSOList;
    public string recipeName;
}
