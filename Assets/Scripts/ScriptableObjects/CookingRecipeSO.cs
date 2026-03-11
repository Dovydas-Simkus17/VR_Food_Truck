using UnityEngine;

[CreateAssetMenu(fileName = "CookingRecipeSO", menuName = "Scriptable Objects/CookingRecipeSO")]
public class CookingRecipeSO : ScriptableObject
{
    public KitchenObjectSO input;
    public KitchenObjectSO output;
    public float cookingTimeMax;
}
