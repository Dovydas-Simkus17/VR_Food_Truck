using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Cooking : MonoBehaviour
{
    public bool isActive = false;
    public float heatLevel = 0f;
    public ParticleSystem fireEffect;

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
}
