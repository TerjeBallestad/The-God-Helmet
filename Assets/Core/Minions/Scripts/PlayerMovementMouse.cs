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
            Minion activeMinion = MinionManager.Instance.activeMinion;
            if (activeMinion != null && activeMinion.active)
            {
                activeMinion.StartGoingToDestination(UtilsClass.GetMouseWorldPosition());
                Debug.Log(activeMinion.name);
            }
        }
    }
}
