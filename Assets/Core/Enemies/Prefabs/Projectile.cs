using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public bool active;
    Vector3 previousPosition;
    float timeAlive;
    float timeInactive;

    private void Start()
    {
        active = true;
    }
    private void FixedUpdate()
    {
        timeAlive += Time.fixedDeltaTime;

        if ((previousPosition - transform.position).magnitude < 0.01f && timeAlive > 3f)
        {
            timeInactive += Time.fixedDeltaTime;
        }
        else
        {
            timeInactive = 0;
        }
        previousPosition = transform.position;
        if (timeInactive > 1f)
        {
            active = false;
        }
    }


}
