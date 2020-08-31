// LICENSE
//
//   This software is dual-licensed to the public domain and under the following
//   license: you are granted a perpetual, irrevocable license to copy, modify,
//   publish, and distribute this file as you see fit.

using UnityEngine;
using System.Collections;

public class BallisticMotion : MonoBehaviour
{

    // Private fields
    Vector3 lastPos;
    Vector3 impulse;
    float gravity;

    bool regularPhysics = false;


    // Methods
    // void Awake()
    // {
    //     // Keep scene heirarchy clean
    //     transform.parent = GameObject.FindGameObjectWithTag("Projectiles").transform;
    // }

    public void Initialize(Vector3 pos, float gravity)
    {
        transform.position = pos;

        lastPos = transform.position;
        this.gravity = gravity;
        StartCoroutine(TurnOnPhysicsTimed());
    }



    void FixedUpdate()
    {
        if (!regularPhysics)
        {
            // Simple verlet integration
            float dt = Time.fixedDeltaTime;
            Vector3 accel = -gravity * Vector3.up;

            Vector3 curPos = transform.position;
            Vector3 newPos = curPos + (curPos - lastPos) + impulse * dt + accel * dt * dt;
            lastPos = curPos;
            transform.position = newPos;

            Debug.Log(GetComponent<Rigidbody2D>().velocity);
            impulse = Vector3.zero;
        }
        // Z-kill
        // if (transform.position.y < -5f)
        //     Destroy(gameObject);
    }

    public void AddImpulse(Vector3 impulse)
    {
        this.impulse += impulse;
    }

    IEnumerator TurnOnPhysicsTimed()
    {
        yield return new WaitForSeconds(0.1f);
        regularPhysics = true;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        Vector3 NewVelocity = (transform.position - lastPos) / Time.fixedDeltaTime;
        yield return new WaitForFixedUpdate();

        rb.velocity = NewVelocity;
        rb.bodyType = RigidbodyType2D.Dynamic;

    }
}