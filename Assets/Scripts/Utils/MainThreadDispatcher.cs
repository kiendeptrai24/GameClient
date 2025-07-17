using System;
using System.Collections.Generic;
using UnityEngine;

public class MainThreadDispatcher : KienBehaviour
{
    private static readonly Queue<Action> mainThreadQueue = new Queue<Action>();
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
    public static void RunOnMainThread(Action action)
    {
        lock (mainThreadQueue)
        {
            mainThreadQueue.Enqueue(action);
        }
    }

    void Update()
    {
        lock (mainThreadQueue)
        {
            while (mainThreadQueue.Count > 0)
            {
                var action = mainThreadQueue.Dequeue();
                action?.Invoke();
            }
        }
    }
}
