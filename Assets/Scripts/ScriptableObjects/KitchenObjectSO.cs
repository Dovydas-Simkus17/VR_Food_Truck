using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "KitchenObjectSO", menuName = "Scriptable Objects/KitchenObjectSO")]
public class KitchenObjectSO : ScriptableObject
{
    public string stateName;

    public GameObject prefab;

    public Material material;

    public Sprite icon;

    public Mesh mesh;

    public float timeTillNextState;

    public KitchenObjectSO nextState;

    public bool isCuttable;
}
