using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Cooking : MonoBehaviour
{
    public bool isActive = false;
    public float heatLevel = 0f;
    public Collider stoveTop;
    public ParticleSystem fireEffect;

    //Testing
    public LayerMask m_LayerMask;
    private enum State
    {
        Idle,
        Frying,
        Fried,
        Burned
    }
    public void SetHeat(float value)
    {
        
        heatLevel = Mathf.Clamp01(value);
        isActive = heatLevel > 0;

        if (isActive && !fireEffect.isPlaying)
        {
            fireEffect.Play();
        }
        else if (!isActive && fireEffect.isPlaying)
        {
            fireEffect.Stop();
        }
    }

    public float GetHeat()
    {
        return heatLevel;
    }
    void FixedUpdate()
    {
        MyCollisions();
    }
    void MyCollisions()
    {
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, m_LayerMask);
        int i = 0;
        // Check when there is a new collider coming into contact with the box
        while (i < hitColliders.Length)
        {
            // Output all of the collider names
            Debug.Log("Hit : " + hitColliders[i].name + i);
            // Increase the number of Colliders in the array

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("SOMETHING HIT ME ");
        if (other.tag == "ing")
        {
            Debug.Log("Ingredient " + other.tag);
        }
    }

    // Draw the Box Overlap as a gizmo to show where it currently is testing. Click the Gizmos button to see this.
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        if (Application.isPlaying)
            // Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}