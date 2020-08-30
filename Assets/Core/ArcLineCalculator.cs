﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ArcLineCalculator : MonoBehaviour
{
    LineRenderer lr;

    public float velocity;
    public float angle;
    public int resolution;
    public GameObject projectilePrefab;
    Vector3 target = Vector3.zero;
    float g; //force of gravity on the y axis
    float radianAngle;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        g = Mathf.Abs(Physics2D.gravity.y);
    }

    private void OnValidate()
    {
        if (lr != null && Application.isPlaying)
        {
            RenderArc();
        }
    }

    // Start is called before the first frame update
    public void ShootProjectile(Vector3 target)
    {
        this.target = target;
        GameObject projectile = Instantiate(projectilePrefab, transform.position + Vector3.up, projectilePrefab.transform.rotation);
        CalculateVelocityAndAngle(projectile.GetComponent<Rigidbody2D>());
        RenderArc();
    }

    //initialization
    void RenderArc()
    {
        // obsolete: lr.SetVertexCount(resolution + 1);
        lr.positionCount = resolution + 1;
        lr.SetPositions(CalculateArcArray());
    }
    //Create an array of Vector 3 positions for the arc
    Vector3[] CalculateArcArray()
    {
        Vector3[] arcArray = new Vector3[resolution + 1];

        radianAngle = Mathf.Deg2Rad * angle;
        float maxDistance = (velocity * velocity * Mathf.Sin(2 * radianAngle)) / g;

        for (int i = 0; i <= resolution; i++)
        {
            float t = (float)i / (float)resolution;
            arcArray[i] = CalculateArcPoint(t, maxDistance);
        }

        return arcArray;
    }

    Vector3 CalculateArcPoint(float t, float maxDistance)
    {
        float x = t * maxDistance;
        float y = x * Mathf.Tan(radianAngle) - ((g * x * x) / (2 * velocity * velocity * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle)));
        return new Vector3(x + transform.position.x, y + transform.position.y);
    }
    void CalculateVelocityAndAngle(Rigidbody2D rigid)
    {
        Vector3 originOfLaunch = transform.position + Vector3.up;
        Vector3 targetPositionForLanding = target;

        float maxPermittedHeight = 10f;

        float heightDifference = originOfLaunch.y - targetPositionForLanding.y;

        velocity = Mathf.Sqrt(2 * -Physics.gravity.y * maxPermittedHeight);

        float trialAndErrorFudgeICouldNotRemove = 0.06f;
        float expectedTravelTime = 0f;
        if (Mathf.Sign(heightDifference) == -1)
        {
            expectedTravelTime = Mathf.Sqrt(2 * (maxPermittedHeight - heightDifference) / -Physics.gravity.y) + Mathf.Sqrt(2 * maxPermittedHeight / -Physics.gravity.y);
        }
        else if (Mathf.Sign(heightDifference) == 1)
        {
            trialAndErrorFudgeICouldNotRemove *= -1;

            expectedTravelTime = Mathf.Sqrt(2 * (maxPermittedHeight + heightDifference) / -Physics.gravity.y) + Mathf.Sqrt(2 * maxPermittedHeight / -Physics.gravity.y);
        }

        float horizontalSpeed = Vector3.Distance(originOfLaunch, targetPositionForLanding) / expectedTravelTime;

        Vector3 facingTargetWithNoY = (targetPositionForLanding - originOfLaunch);
        facingTargetWithNoY.y = 0f;

        Vector3 normalizedForwardDir = facingTargetWithNoY.normalized;
        Vector3 launchVector = new Vector3(normalizedForwardDir.x * horizontalSpeed, velocity, normalizedForwardDir.z * horizontalSpeed);
        angle = Vector3.Angle(facingTargetWithNoY, launchVector);
        //quote from the question page linked at the top.
        // If you want, "You can calculate the angle by Mathf.Atan2.Speed would be Magnitude of FinalVector." ???



        //Now to add force. Make sure it has none already.
        rigid.AddForce(-rigid.velocity, ForceMode2D.Impulse);
        // //Launch
        rigid.AddForce(launchVector, ForceMode2D.Impulse);
        // //And add some of that fudge!
        rigid.AddForce(launchVector * trialAndErrorFudgeICouldNotRemove, ForceMode2D.Impulse);
    }
}