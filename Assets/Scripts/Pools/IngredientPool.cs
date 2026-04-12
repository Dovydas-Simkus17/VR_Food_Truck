using System.Collections.Generic;
using UnityEngine;

public class IngredientPool : MonoBehaviour
{
    public static IngredientPool SharedInstance;
    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int amountToPool;

    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectToPool);
            tmp.gameObject.SetActive(false);
            pooledObjects.Add(tmp);
        }
    }
    public GameObject GetFromPool(KitchenObjectSO ingredient)
    {
        foreach (var obj in pooledObjects)
        {
            if (!obj.activeInHierarchy)
            {
                obj.GetComponent<Rigidbody>().useGravity = true;
                obj.GetComponent<Ingredient>().Setup(ingredient);
                obj.SetActive(true);

                return obj;
            }
        }

        return null; // or expand pool if needed
    }
    public IngredientPool GetInstance()
    {
        return this;
    }
}
