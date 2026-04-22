using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorQuitting : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Application.Quit();
    }
}