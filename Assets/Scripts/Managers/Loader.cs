using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    {
        BasicTruckScene,
        EndGameScene,
        LoadingScene
    }

    private static Scene targetScene;

    public static void Load(Scene scene)
    {
        Loader.targetScene = scene;

        SceneManager.LoadScene(Scene.LoadingScene.ToString());



        SceneManager.LoadScene(scene.ToString());
    }

    public static void LoaderCallback()
    {
        SceneManager.LoadScene(targetScene.ToString());
    }
}
