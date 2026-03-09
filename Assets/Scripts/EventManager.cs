using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public EventManager current;

    private void Awake()
    {
        current = this;
    }

    public event Action eventCradleActive;
}
