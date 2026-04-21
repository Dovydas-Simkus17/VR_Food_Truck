using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    public static BoxSpawner sharedInstance;

    public Transform spawnPoint;
    public int spawnAmount = 3;
    private int currentSpawn = 0;
    public GameObject ingredientPrefab;
    public KitchenObjectSO KitchenObject;
    void Awake()
    {
        if (sharedInstance != null && sharedInstance != this)
        {
            Destroy(gameObject);
            return;
        }

        sharedInstance = this;
    }
    public void SpawnBox(KitchenObjectSO kitchenObject)
    {
        currentSpawn++;
        Debug.Log("We are here in Spwaner");
        GameObject obj = BoxPool.SharedInstance.GetFromPool();
        obj.transform.position = spawnPoint.position;
        obj.transform.rotation = spawnPoint.rotation;
        Debug.Log("Object = " + obj);
        Debug.Log("KitchenObject = " + kitchenObject);
        BoxWithIngredient packagedIng = obj.GetComponent<BoxWithIngredient>();
        packagedIng.Setup(kitchenObject);

        
        
    }
    public bool canSpawn()
    {
        return currentSpawn < spawnAmount;
    }
    public void decreaseSpawn()
    {
        currentSpawn--;
    }
}
