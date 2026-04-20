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
    public float maxPaitence = 100f;

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
    private Animator animate;
    public System.Action<Customer> onCustomerFinished;
    public CustomerUI custUI;
    void Awake()
    {
        agent = GetComponentInChildren<NavMeshAgent>();
        animate = GetComponent<Animator>();
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
                Vector3 targetPosition = Camera.main.transform.position;
                targetPosition.y = transform.position.y;
                transform.LookAt(targetPosition);
                if (currentPaitence >= maxPaitence)
                {
                    Leave();
                }
                break;

            case State.Leaving:
                // nothing
                break;

        }
        float normalizedSpeed = Mathf.InverseLerp(0f, agent.speed, agent.velocity.magnitude);
        animate.SetFloat("Speed", normalizedSpeed);
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
<<<<<<< HEAD
            //play Good Effect
=======
>>>>>>> d77acdb3ce9e5695a4f345837a581a27d45b4108
            Game_Manager.sharedInstance.addPosiScore(1);
            Leave();
        }
        else
        {
            Game_Manager.sharedInstance.addNegiScore(1);
<<<<<<< HEAD
            //play Bad Effect
=======
>>>>>>> d77acdb3ce9e5695a4f345837a581a27d45b4108
            Debug.Log("Wrong order!");
            Leave();
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

        // small pause at exit (looks more natural)
        yield return new WaitForSeconds(1f);

        // Disable or destroy
        gameObject.SetActive(false); // better for pooling

        onCustomerFinished?.Invoke(this);
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
    public float getCurrentPaitence()
    {
        return currentPaitence;
    }
    public void increaseCurrentPaitence()
    {
        currentPaitence += Time.fixedDeltaTime;
        custUI.UpdateUI();
    }
}