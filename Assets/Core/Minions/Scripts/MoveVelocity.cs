using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class MoveVelocity : MonoBehaviour, IMoveVelocity
{
    [SerializeField] private float speed = 4;
    private Vector3 direction;
    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void SetVelocity(Vector3 direction)
    {
        this.direction = direction;
    }
    private void FixedUpdate()
    {
        rb.velocity = direction * speed;
        // transform.Translate(UtilsClass.PixelPerfectClamp(transform.position, 48f));
    }
}
