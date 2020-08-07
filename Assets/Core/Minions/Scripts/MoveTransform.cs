using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTransform : MonoBehaviour, IMoveVelocity
{
    [SerializeField] private float speed = 4;
    private Vector3 direction;

    public void SetVelocity(Vector3 direction)
    {
        this.direction = direction;
    }

    private void Update()
    {
        transform.position += PixelPerfectClamp(direction * speed * Time.deltaTime, 48f);
    }
    private Vector3 PixelPerfectClamp(Vector3 velocity, float pixelsPerUnit)
    {
        Vector3 VelocityInPixels = new Vector3(
            Mathf.RoundToInt(velocity.x * pixelsPerUnit),
            Mathf.RoundToInt(velocity.y * pixelsPerUnit), 0
        );
        return VelocityInPixels / pixelsPerUnit;
    }
}
