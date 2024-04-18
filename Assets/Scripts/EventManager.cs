using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;
    public static EventManager Instance { get { return instance; } }
    public event Action<bool> RecreatePathMessageReceived;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SendRecreatePathMessage(bool initialPath)
    {
        Debug.Log("Event sent");
        RecreatePathMessageReceived?.Invoke(initialPath);
    }
}
