using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    public Transform spawnPoint;
    public int spawnAmount = 3;
    private int currentSpawn;
    public GameObject ingredientPrefab;
    public KitchenObjectSO KitchenObject;
    public void SpawnBox(KitchenObjectSO kitchenObject)
    {
        if (currentSpawn < spawnAmount)
        {
            Debug.Log("We are here in Spwaner");
            GameObject obj = BoxPool.SharedInstance.GetFromPool();
            obj.transform.position = spawnPoint.position;
            obj.transform.rotation = spawnPoint.rotation;
            Debug.Log("Object = " + obj);
            Debug.Log("KitchenObject = " + kitchenObject);
            BoxWithIngredient asda = obj.GetComponent<BoxWithIngredient>();
            asda.Setup(kitchenObject);

        }
        else
        {
            //Play bad sound effect
        }
    }
}
