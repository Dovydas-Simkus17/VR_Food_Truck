using UnityEngine;
using UnityEngine.Audio;

public class PencilScribble : MonoBehaviour
{
    public Notepad currentPad;
    public float scribbleSpeed = 60f;

    private Vector3 lastPos;
    private bool touchingPad = false;
    public AudioSource scribleSound;
    private bool isScribbling;
    void Update()
    {
        bool shouldScribble = false;
        if (touchingPad && currentPad != null && currentPad.CanWrite())
        {
            float movement = Vector3.Distance(transform.position, lastPos);

            if (movement > 0.001f)
            {
                currentPad.AddProgress(movement * scribbleSpeed);
                shouldScribble = true;
            }
        }
        if (shouldScribble && !isScribbling)
        {
            scribleSound.Play();
            isScribbling = true;
        }
        else if (!shouldScribble && isScribbling)
        {
            scribleSound.Stop();
            isScribbling = false;
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