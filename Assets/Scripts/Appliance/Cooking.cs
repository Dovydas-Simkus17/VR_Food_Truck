using NUnit.Framework;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VRTemplate;
using UnityEngine;

public class Cooking : MonoBehaviour
{
    public bool isActive = false;
    public float heatLevel = 0f;
    public GameObject Lever;

    public Collider stoveTop;
    public ParticleSystem fireEffect;
    private List<Ingredient> ingredientsCooking = new List<Ingredient>();
    //Testing
    public LayerMask m_LayerMask;

    public void SetHeat(float value)
    {
        HingeJoint gamer = Lever.GetComponent<HingeJoint>();
        heatLevel = Lever.GetComponent<HingeJoint>().angle;
        Debug.Log(heatLevel);
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
        FindIngredients();
        MyCooking();
    }
    public Vector3 halfExtents = new Vector3(1, 1, 1);
    void FindIngredients()
    {   
        //Debug.Log("We are in MyCollisions");
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, halfExtents, gameObject.transform.rotation, m_LayerMask);
        //Debug.Log(hitColliders);

        // Check when there is a new collider coming into contact with the box
        foreach (Collider collider in hitColliders)
        {
            Ingredient ing = collider.GetComponent<Ingredient>();

            if (ing != null)
            {
                ingredientsCooking.Add(ing);
            }
        }
            
    }

    void MyCooking()
    {
        SetHeat(GetHeat());
        ingredientsCooking.ForEach(cook => {
            //Debug.Log("We are cooking: " + cook);
            //Debug.Log("At the rate of: " + heatLevel * Time.fixedDeltaTime);
            cook.Cook(GetHeat() * Time.fixedDeltaTime);
        });
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ing")
        {
            Debug.Log("Ingredient " + other.tag);
            other.gameObject.GetComponent<Ingredient>().SetIsCookingTrue();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (ingredientsCooking.Count == 1)
        {
            ingredientsCooking.Clear();
        }
        else
        {
            other.gameObject.GetComponent<Ingredient>().SetIsCookingfalse();
            ingredientsCooking.Remove(other.GetComponent<Ingredient>());
        }
        Debug.Log(ingredientsCooking);
    }

    // Draw the Box Overlap as a gizmo to show where it currently is testing. Click the Gizmos button to see this.
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        if (Application.isPlaying)
            // Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.DrawWireCube(transform.position, halfExtents);
    }
}