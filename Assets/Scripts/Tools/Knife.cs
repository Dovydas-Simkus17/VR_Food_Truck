using UnityEngine;

public class XRKnife : MonoBehaviour
{
    public Transform bladeTip;
    public float cutSpeedThreshold = 1.0f;
    public float directionThreshold = 0.5f;

    private Vector3 lastTipPosition;
    private Vector3 velocity;

    void FixedUpdate()
    {
        // Calculate velocity of blade tip
        velocity = (bladeTip.position - lastTipPosition) / Time.fixedDeltaTime;
        lastTipPosition = bladeTip.position;
    }

    private void OnCollisionStay(Collision collision)
    {
        Ingredient ingredient = collision.collider.GetComponentInParent<Ingredient>();

        if (ingredient == null) return;

        TryCut(ingredient, collision);
    }

    void TryCut(Ingredient ingredient, Collision collision)
    {
        float speed = velocity.magnitude;

        if (speed < cutSpeedThreshold) return;

        // Check slicing direction (knife forward direction)
        Vector3 bladeDirection = transform.forward;

        float alignment = Vector3.Dot(velocity.normalized, bladeDirection);

        if (alignment < directionThreshold) return;

        //Valid Cut
        ingredient.Cut();
    }
}