using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public KitchenObjectSO currentState;

    float cookTimer = 0f;

    bool isCooking;
    Renderer mat;
    public void Start()
    {
        mat = GetComponent<Renderer>();
        UpdateVisuals();
    }
    public void Cook(float amount)
    {
        if (currentState.nextState == null)
        {
            return;
        }
        if (isCooking)
        {
            cookTimer += amount;
        }
        else
        {
            return;
        }

        if (cookTimer > currentState.timeTillNextState)
        {
            cookTimer = 0f;
            currentState = currentState.nextState;

            UpdateVisuals();
        }


    }

    void UpdateVisuals()
    {
        mat.material = currentState.material;

    }

    public void SetIsCookingTrue()
    {
        this.isCooking = true;
    }
    public void SetIsCookingfalse()
    {
        this.isCooking = false;

    }
}