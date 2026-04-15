using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    public RecipeSO currentRecipe;

    public Transform noteSpawnPoint;
    public GameObject orderNotePrefab;
    public Notepad notepad;
    public bool playerInRange = false;

    public float progress = 0f;
    private float currentPaitence = 0f;
    public float maxPaitence = 30f;

    private bool alreadyCompleted = false;
    private GameObject spawnedNote;

    public float cooldown = 2f;
    private float lastRingTime;
    public enum State
    {
        WalkingToQueue,
        Waiting,
        BeingServed,
        Leaving
    }
    public State currentState;
    private int queueIndex;
    public Transform exitPoint;

    private NavMeshAgent agent;
    void Awake()
    {
        agent = GetComponentInChildren<NavMeshAgent>();
    }
    public void Init(Transform exit)
    {
        CustomerQueue queueManager = CustomerQueue.sharedInstance;
        exitPoint = exit;

        queueIndex = queueManager.AddCustomer(this);

        if (queueIndex == -1)
        {
            Leave(); // queue full
            return;
        }

        MoveTo(queueManager.GetPoint(queueIndex), queueIndex);
        currentState = State.WalkingToQueue;
    }
    public void SetOrder(RecipeSO recipe)
    {
        currentRecipe = recipe;

    }
    void Update()
    {
        switch (currentState)
        {
            case State.WalkingToQueue:
                if (!agent.pathPending && agent.remainingDistance < 0.2f)
                {
                    currentState = State.Waiting;
                }
                break;

            case State.Waiting:
                currentPaitence += Time.deltaTime;

                if (currentPaitence >= maxPaitence)
                {
                    Leave();
                }
                break;

            case State.Leaving:
                // nothing
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            notepad.currentCustomer = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            notepad.currentCustomer = null;
        }
    }

    public void Serve(List<Ingredient> givenIngredients)
    {
        if (Time.time - lastRingTime < cooldown)
            return;

        lastRingTime = Time.time;
        //Debug.Log("This is how many is in the PLate: " + givenIngredients.Count);
        if (CheckOrder(givenIngredients))
        {
            Debug.Log("Correct order!");
            Game_Manager.sharedInstance.addPosiScore(1);
            Leave();
        }
        else
        {
            Game_Manager.sharedInstance.addNegiScore(1);
            Debug.Log("Wrong order!");
        }
    }

    bool CheckOrder(List<Ingredient> given)
    {
        Debug.Log("Current List" + given.Count);
        if (given.Count != currentRecipe.kitchenObjectSOList.Count)
            return false;

        foreach (var req in currentRecipe.kitchenObjectSOList)
        {
            bool found = given.Exists(i => i.currentState == req);
            if (!found) return false;
        }

        return true;
    }

    void Leave()
    {
        if (currentState == State.Leaving) return;
        currentState = State.Leaving;
        Debug.Log("I am leaving");
        CustomerQueue.sharedInstance.RemoveCustomer(this);

        // Move to exit
        agent.isStopped = false;
        agent.SetDestination(exitPoint.position);

        // Start leaving process
        StartCoroutine(LeaveRoutine());
    }
    IEnumerator LeaveRoutine()
    {
        // Wait until close to exit
        while (agent.pathPending || agent.remainingDistance > 0.2f)
        {
            yield return null;
        }

        // Optional: small pause at exit (looks more natural)
        yield return new WaitForSeconds(1f);

        // Disable or destroy
        gameObject.SetActive(false); // better for pooling
    }
    public void SetRecipe(RecipeSO recipe)
    {
        currentRecipe = recipe;
    }

    public bool isComplete()
    {
        return alreadyCompleted;
    }
    public void Completed()
    {
        alreadyCompleted = true;
    }
    public void MoveTo(Transform target, int newIndex)
    {
        queueIndex = newIndex;
        agent.SetDestination(target.position);
    }
}