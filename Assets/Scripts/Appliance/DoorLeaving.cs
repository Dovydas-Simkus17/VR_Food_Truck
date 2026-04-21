using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorLeaving : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) { return; }
        if (Game_Manager.sharedInstance != null)
        {
            Game_Manager.sharedInstance.ResetGame();
        }
        if (MusicManager.sharedInstance != null)
        {
            MusicManager.sharedInstance.changeSong(MusicManager.sharedInstance.baseSong);
        }

        Loader.Load(Loader.Scene.BasicTruckScene);
    }
}