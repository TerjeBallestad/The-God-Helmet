using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

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
        transform.position += UtilsClass.PixelPerfectClamp(direction * speed * Time.deltaTime, 48f);
    }
}
