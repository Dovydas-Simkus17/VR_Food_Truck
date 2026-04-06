using System.Collections.Generic;
using UnityEngine;

public class IngredientBox : MonoBehaviour
{
    public static IngredientBox SharedInstance;
    public List<Ingredient> pooledObjects;
    public Ingredient objectToPool;
    public int amountToPool;

    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        pooledObjects = new List<Ingredient>();
        Ingredient tmp;
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectToPool);
            tmp.gameObject.SetActive(false);
            pooledObjects.Add(tmp);
        }
    }
}
