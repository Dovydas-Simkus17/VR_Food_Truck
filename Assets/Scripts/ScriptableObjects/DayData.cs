using UnityEngine;

[CreateAssetMenu(fileName = "DayData", menuName = "Scriptable Objects/DayData")]
public class DayData : ScriptableObject
{
    public int dayNumber;

    public int customerCount;
    public float spawnInterval;

    public int ingredientCount;

    public float customerPatience;

    public float rewardMultiplier;
}
