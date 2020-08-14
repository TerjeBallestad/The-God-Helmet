using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementKeys : MonoBehaviour
{
    private void Update()
    {
        float x = 0f, y = 0f;

        if (Input.GetKey(KeyCode.W)) y = +1f;
        if (Input.GetKey(KeyCode.S)) y = -1f;
        if (Input.GetKey(KeyCode.D)) x = +1f;
        if (Input.GetKey(KeyCode.A)) x = -1f;

        Vector3 direction = new Vector3(x, y).normalized;

        GameManager.Instance.selectedMinion.GetComponent<IMoveVelocity>().SetVelocity(direction);
    }
}
