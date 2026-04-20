using UnityEngine;

public class LoadedCallback : MonoBehaviour
{
    private bool isFirstUpdate = true;
    void Update()
    {
        if (isFirstUpdate)
        {
            isFirstUpdate = false;
        }
    }
}
