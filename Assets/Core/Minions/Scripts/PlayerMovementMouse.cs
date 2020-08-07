using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class PlayerMovementMouse : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            GetComponent<IMovePosition>().SetMovePosition(UtilsClass.GetMouseWorldPosition());
        }
    }
}
