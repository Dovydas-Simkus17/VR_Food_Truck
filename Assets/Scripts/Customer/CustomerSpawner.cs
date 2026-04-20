using System.Collections;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject customerPrefab;
    public Transform spawnPoint;
    public Transform exitPoint;
    public ServingWindow servingWindow;

    public RecipeSO[] possibleRecipes;
    private Coroutine spawnRoutine;
    public bool spawning = false;

    private int CurrentCust;
    private int dayMax;
    void OnEnable()
    {
        Game_Manager.sharedInstance.OnDayStart += SetupDay;
        Game_Manager.sharedInstance.OnDayEnd += StopSpawning;
    }

    void OnDisable()
    {
        Game_Manager.sharedInstance.OnDayStart -= SetupDay;
        Game_Manager.sharedInstance.OnDayEnd -= StopSpawning;
    }

    void SetupDay(DayData day)
    {
        StopSpawning();
        CurrentCust = 0;
        spawning = true;
        dayMax = day.customerCount;
        spawnRoutine = StartCoroutine(SpawnRoutine(day));

    }
    IEnumerator SpawnRoutine(DayData day)
    {
        yield return new WaitForSeconds(2f);

        int spawned = 0;

        while (spawned < day.customerCount)
        {
            if (CustomerQueue.sharedInstance.HasFreeSlot())
            {
                SpawnCustomer();
                spawned++;
            }

            yield return new WaitForSeconds(day.spawnInterval);
        }

        Debug.Log("Finished spawning customers");
        
        StopSpawning();
    }

    void StopSpawning()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
            spawning = false;
        }
    }

    void SpawnCustomer()
    {

        Transform custSpawn = spawnPoint;
        GameObject obj = CustomerPool.SharedInstance.GetFromPool();

        obj.transform.position = custSpawn.position;
        obj.transform.rotation = custSpawn.rotation;

        Customer cust = obj.GetComponent<Customer>();
        RecipeSO randomOrder = possibleRecipes[Random.Range(0, possibleRecipes.Length)];

        cust.onCustomerFinished = null;
        cust.onCustomerFinished += OnCustomerFinished;

        cust.SetRecipe(randomOrder);
        cust.Init(exitPoint);
    }
    void OnCustomerFinished(Customer c)
    {
        c.onCustomerFinished -= OnCustomerFinished;
        CurrentCust++;

        CheckDayEnd();
    }

    void CheckDayEnd()
    {
        if (!spawning && CurrentCust >= dayMax)
        {
            Game_Manager.sharedInstance.EndDay();
        }
    }
}
