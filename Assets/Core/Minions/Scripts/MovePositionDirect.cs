using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePositionDirect : MonoBehaviour, IMovePosition
{
    private Vector3 movePosition;

    public void SetMovePosition(Vector3 movePosition)
    {
        this.movePosition = movePosition;
    }

    private void Update()
    {
        Vector3 direction = (movePosition - transform.position).normalized;
        GetComponent<IMoveVelocity>().SetVelocity(direction);
    }
}
