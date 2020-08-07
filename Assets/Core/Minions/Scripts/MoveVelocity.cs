using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVelocity : MonoBehaviour, IMoveVelocity
{
    [SerializeField] private float speed = 4;
    private Vector3 direction;
    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void SetVelocity(Vector3 direction)
    {
        this.direction = direction;
    }
    private void FixedUpdate()
    {
        rb.velocity = direction * speed;
    }
}
