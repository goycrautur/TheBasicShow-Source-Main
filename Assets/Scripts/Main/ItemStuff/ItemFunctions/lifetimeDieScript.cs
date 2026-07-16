using System.Collections.Generic;
using UnityEngine;

public class lifetimeDieScript : MonoBehaviour
{
    private void Update()
    {
        if (lifetime.CountdownWithDeltaTime() == 0)
        {
            Destroy(gameObject);
        }
    }
    [Header("Lifetime Settings")]
    [SerializeField] private float lifetime = 300f;
}
