using UnityEngine;

public class Valve : MonoBehaviour
{
    public Cooking stoveTop;
    public float rotationAmount = 0f;

    public void TurnValve(float value)
    {
        rotationAmount = Mathf.Clamp01(value);
        stoveTop.SetHeat(rotationAmount);
    }
}
