using System;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class balloonFloatScript : MonoBehaviour
{
    private Transform Player;
    private void Start()
    {
        rb = base.GetComponent<Rigidbody>();
        ChangeBallonDirection();
    }

    private void Update()
    {
        if (rb.velocity.magnitude <= 0f)
        {
            ChangeBallonDirection();
        }
        rb.velocity = direction * UnityEngine.Random.Range(minDirectionTime, maxDirectionTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        direction = Vector3.Reflect(direction, collision.contacts[0].normal);
    }

    private void ChangeBallonDirection()
    {
        direction.x = UnityEngine.Random.Range(-1f, 1f);
        direction.z = UnityEngine.Random.Range(-1f, 1f);
        direction = direction.normalized;
    }
    private Rigidbody rb;
    private Vector3 direction;
	public float minDirectionTime = 2.5f,maxDirectionTime = 10f;
}
