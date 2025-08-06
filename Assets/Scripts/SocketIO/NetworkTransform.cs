using System;
using UnityEngine;

public class NetworkTransform : KienNetworkBehaviour
{
    private Vector3 lastPosition;
    private Quaternion lastRotation;
    public float sendRate = 0.05f;
    private float nextSendTime;

    void Update()
    {
        if (IsOwner)
        {
            if (Time.time >= nextSendTime)
            {
                nextSendTime = Time.time + sendRate;
                if ((transform.position - lastPosition).sqrMagnitude > 0.0001f || transform.rotation != lastRotation)
                {
                    NetworkManager.Instance.getposition(transform.position,NetworkId);
                    lastPosition = transform.position;
                    lastRotation = transform.rotation;
                }
            }
        }   
    }

    public void ApplyTransform(Vector3 position, Quaternion rotation)
    {
        if (!IsOwner)
        {
            transform.position = Vector3.Lerp(transform.position, position, 0.5f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 0.5f);
        }
    }

}
