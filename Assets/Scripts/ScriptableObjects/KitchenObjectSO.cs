using UnityEngine;

[CreateAssetMenu(fileName = "KitchenObjectSO", menuName = "Scriptable Objects/KitchenObjectSO")]
public class KitchenObjectSO : ScriptableObject
{
    public string stateName;

    public GameObject prefab;

    public Material material;

    public Mesh mesh;

    public float timeTillNextState;

    public KitchenObjectSO nextState;

    public bool isCuttable;
}
