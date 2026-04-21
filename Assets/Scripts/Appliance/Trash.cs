using JetBrains.Annotations;
using UnityEngine;

public class Trash : MonoBehaviour
{
    public AudioClip trashIn;
    public AudioSource trashSound;
    private void OnTriggerEnter(Collider other)
    {
 
        if (other.CompareTag("ing"))
        {
            //Debug.Log("Ingredient " + other.tag);
            
            other.gameObject.SetActive(false);
            trashSound.PlayOneShot(trashIn);
        }
    }
}
