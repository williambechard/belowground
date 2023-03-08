using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    private Dictionary<string, Action> eventDictionary;

    private static EventManager eventManager;

    public static EventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindFirstObjectByType(typeof(EventManager)) as EventManager;

                if (!eventManager)
                {
                    Debug.Log("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    eventManager.Init();
                }
            }

            return eventManager;
        }
    }

    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, Action>();
        }
    }

    public static void StartListening(string eventName, Action listener)
    {
        if (instance.eventDictionary.ContainsKey(eventName))
        {
            instance.eventDictionary[eventName] += listener;
        }
        else
        {
            instance.eventDictionary.Add(eventName, listener);
        }
    }

    public static void StopListening(string eventName, Action listener)
    {
        if (instance.eventDictionary.ContainsKey(eventName))
        {
            instance.eventDictionary[eventName] -= listener;
        }
    }

    public static void TriggerEvent(string eventName)
    {
        Action thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }
}