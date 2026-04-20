using UnityEngine;

public class PencilScribble : MonoBehaviour
{
    public Notepad currentPad;
    public float scribbleSpeed = 60f;

    private Vector3 lastPos;
    private bool touchingPad = false;

    void Update()
    {
        if (touchingPad && currentPad != null && currentPad.CanWrite())
        {
            float movement = Vector3.Distance(transform.position, lastPos);

            if (movement > 0.001f)
            {
                //Play Sound Effect
                currentPad.AddProgress(movement * scribbleSpeed);
            }
        }

        lastPos = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NotePad"))
        {
            currentPad = other.GetComponent<Notepad>();
            touchingPad = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NotePad"))
        {
            touchingPad = false;
            currentPad = null;
        }
    }
}