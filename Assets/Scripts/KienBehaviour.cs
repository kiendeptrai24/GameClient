using System;
using UnityEngine;

public abstract class KienBehaviour : MonoBehaviour
{
    protected virtual void Start()
    {
        
    }
    protected virtual void Awake()
    {

    }

    private void Reset()
    {
        LoadComponent();
    }

    protected virtual void LoadComponent() { }

}
