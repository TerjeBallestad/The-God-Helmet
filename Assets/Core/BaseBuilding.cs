using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBuilding : MonoBehaviour
{
    [HideInInspector]
    public Vector3 spawnPosition;
    private void Awake()
    {
        spawnPosition = transform.position + new Vector3(4, 0);
    }
}
