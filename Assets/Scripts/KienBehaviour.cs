using System;
using UnityEngine;

public abstract class KienBehaviour : MonoBehaviour
{
    protected virtual void Start()
    {
        
    }
    private void Reset()
    {
        LoadComponent();
    }

    protected abstract void LoadComponent();

}
