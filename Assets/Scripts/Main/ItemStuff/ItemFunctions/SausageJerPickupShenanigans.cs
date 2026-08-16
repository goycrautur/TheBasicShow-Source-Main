using System.Collections.Generic;
using UnityEngine;

public class SausageJerPickupShenanigans : MonoBehaviour
{
    private void Update()
    {
        if (lifetime.CountdownWithDeltaTime() == 0 && !stopped) Destroy(gameObject);
    }
    [Header("Lifetime Settings")]
    [SerializeField] private float lifetime = 300f;
    [SerializeField] private bool stopped;

    public bool Stopped
    {
        get
        {
            return stopped;
        }

        set
        {
            stopped = value;
        }
    }
}
